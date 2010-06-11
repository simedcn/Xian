﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Drawing;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.ImageViewer.InteractiveGraphics;

namespace ClearCanvas.ImageViewer.AdvancedImaging.Fusion
{
	[Cloneable(false)]
	internal partial class FusionOverlayCompositeGraphic : CompositeGraphic, IVoiLutProvider, IColorMapProvider, ILayerOpacityProvider
	{
		[CloneIgnore]
		private IFusionOverlayFrameDataReference _overlayFrameDataReference;

		[CloneIgnore]
		private GrayscaleImageGraphic _overlayImageGraphic;

		[CloneIgnore]
		private bool _isLoadingProgressShown;

		private VoiLutManagerProxy _voiLutManagerProxy;
		private ColorMapManagerProxy _colorMapManagerProxy;

		public FusionOverlayCompositeGraphic(FusionOverlayFrameData overlayFrameData)
		{
			_overlayFrameDataReference = overlayFrameData.CreateTransientReference();
			_overlayFrameDataReference.FusionOverlayFrameData.Unloaded += HandleOverlayFrameDataUnloaded;
			_voiLutManagerProxy = new VoiLutManagerProxy();
			_colorMapManagerProxy = new ColorMapManagerProxy();
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected FusionOverlayCompositeGraphic(FusionOverlayCompositeGraphic source, ICloningContext context)
		{
			context.CloneFields(source, this);

			_overlayFrameDataReference = source._overlayFrameDataReference.Clone();
			_overlayFrameDataReference.FusionOverlayFrameData.Unloaded += HandleOverlayFrameDataUnloaded;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_overlayImageGraphic = null;
				_voiLutManagerProxy = null;
				_colorMapManagerProxy = null;

				if (_overlayFrameDataReference != null)
				{
					_overlayFrameDataReference.FusionOverlayFrameData.Unloaded -= HandleOverlayFrameDataUnloaded;
					_overlayFrameDataReference.Dispose();
					_overlayFrameDataReference = null;
				}
			}

			base.Dispose(disposing);
		}

		public IVoiLutManager VoiLutManager
		{
			get { return _voiLutManagerProxy; }
		}

		public IColorMapManager ColorMapManager
		{
			get { return _colorMapManagerProxy; }
		}

		public ILayerOpacityManager LayerOpacityManager
		{
			get { return _colorMapManagerProxy; }
		}

		public FusionOverlayFrameData OverlayFrameData
		{
			get { return _overlayFrameDataReference.FusionOverlayFrameData; }
		}

		public GrayscaleImageGraphic OverlayImageGraphic
		{
			get { return _overlayImageGraphic; }
			private set
			{
				if (_overlayImageGraphic != value)
				{
					if (_overlayImageGraphic != null)
					{
						base.Graphics.Remove(_overlayImageGraphic);
						_overlayImageGraphic.Dispose();
						_voiLutManagerProxy.SetRealVoiLutManager(null);
						_colorMapManagerProxy.SetRealColorMapManager(null);
					}

					_overlayImageGraphic = value;

					if (_overlayImageGraphic != null)
					{
						_voiLutManagerProxy.SetRealVoiLutManager(_overlayImageGraphic.VoiLutManager);
						_colorMapManagerProxy.SetRealColorMapManager(_overlayImageGraphic.ColorMapManager);
						base.Graphics.Add(_overlayImageGraphic);
					}
				}
			}
		}

		public override void OnDrawing()
		{
			if (_overlayImageGraphic == null)
			{
				float progress;
				string message;
				if (_overlayFrameDataReference.FusionOverlayFrameData.BeginLoad(out progress, out message))
				{
					OverlayImageGraphic = _overlayFrameDataReference.FusionOverlayFrameData.CreateImageGraphic();

#if DEBUG
					if (this.OverlayFrameData.BaseFrame.FrameOfReferenceUid != this.OverlayFrameData.OverlayFrameOfReferenceUid)
					{
						if (!CollectionUtils.Contains(base.Graphics, g => g is ITextGraphic))
						{
							ITextGraphic warning = new InvariantTextPrimitive("Frame of Reference (0020,0052) MISMATCH");
							warning.CoordinateSystem = CoordinateSystem.Destination;
							warning.Location = new PointF(base.ParentPresentationImage.ClientRectangle.Width/2f, base.ParentPresentationImage.ClientRectangle.Height/2f);
							warning.ResetCoordinateSystem();
							this.Graphics.Add(warning);
						}
					}
#endif

					_isLoadingProgressShown = false;
				}
				else if (!_isLoadingProgressShown)
				{
					_isLoadingProgressShown = true;
					this.Graphics.Add(new ProgressGraphic(_overlayFrameDataReference.FusionOverlayFrameData, true, ProgressBarGraphicStyle.Continuous));
				}
			}
			base.OnDrawing();
		}

		private void HandleOverlayFrameDataUnloaded(object sender, EventArgs e)
		{
			OverlayImageGraphic = null;
		}
	}
}