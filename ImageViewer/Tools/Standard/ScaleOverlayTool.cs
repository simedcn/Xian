#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
	[MenuAction("showHide", "imageviewer-contextmenu/MenuShowHideScaleOverlay", "ShowHide", InitiallyAvailable = false)]
	[MenuAction("showHide", "global-menus/MenuTools/MenuStandard/MenuShowHideScaleOverlay", "ShowHide")]
	[Tooltip("showHide", "TooltipShowHideScaleOverlay")]
	[GroupHint("showHide", "Tools.Image.Overlays.Scale.ShowHide")]
	[IconSet("showHide", IconScheme.Colour, "Icons.ScaleOverlayToolSmall.png", "Icons.ScaleOverlayToolMedium.png", "Icons.ScaleOverlayToolLarge.png")]
	//
	[ButtonAction("toggle", "overlays-dropdown/ToolbarScaleOverlay", "ShowHide")]
	[CheckedStateObserver("toggle", "Checked", "CheckedChanged")]
	[Tooltip("toggle", "TooltipScaleOverlay")]
	[GroupHint("toggle", "Tools.Image.Overlays.Scale.ShowHide")]
	[IconSet("toggle", IconScheme.Colour, "Icons.ScaleOverlayToolSmall.png", "Icons.ScaleOverlayToolMedium.png", "Icons.ScaleOverlayToolLarge.png")]
	//
	[ExtensionOf(typeof (ImageViewerToolExtensionPoint))]
	public class ScaleOverlayTool : OverlayToolBase
	{
		public ScaleOverlayTool()
		{
		}

		protected override void UpdateVisibility(IPresentationImage image, bool visible)
		{
			CompositeScaleGraphic scale = GetCompositeScaleGraphic(image, visible);
			if (scale != null)
				scale.Visible = visible;
		}

		private static CompositeScaleGraphic GetCompositeScaleGraphic(IPresentationImage image, bool createIfNull)
		{
			if (image is IApplicationGraphicsProvider)
			{
				GraphicCollection overlayGraphics = ((IApplicationGraphicsProvider)image).ApplicationGraphics;
				CompositeScaleGraphic scale = CollectionUtils.SelectFirst(overlayGraphics,
				                                                          delegate(IGraphic graphic) { return graphic is CompositeScaleGraphic; }
				                              	) as CompositeScaleGraphic;

				if (scale == null && createIfNull)
					overlayGraphics.Insert(0, scale = new CompositeScaleGraphic());

				return scale;
			}

			return null;
		}

		[Cloneable(false)]
		private class CompositeScaleGraphic : CompositeGraphic
		{
			private const string _horizontalName = "Horizontal scale";
			private const string _verticalName = "Vertical scale";

			[CloneIgnore]
			private ScaleGraphic _horizontalScale;
			[CloneIgnore]
			private ScaleGraphic _verticalScale;

			public CompositeScaleGraphic()
			{
				base.Graphics.Add(_horizontalScale = new ScaleGraphic {Name = _horizontalName });
				base.Graphics.Add(_verticalScale = new ScaleGraphic { Name = _verticalName });

				_horizontalScale.Visible = false;
				_horizontalScale.IsMirrored = true;

				_verticalScale.Visible = false;
				_verticalScale.IsMirrored = true;
			}

			protected CompositeScaleGraphic(CompositeScaleGraphic source, ICloningContext context)
			{
				context.CloneFields(source, this);
			}

			[OnCloneComplete]
			private void OnCloneComplete()
			{
				_horizontalScale = (ScaleGraphic)CollectionUtils.SelectFirst(Graphics, graphic => graphic is ScaleGraphic && graphic.Name == _horizontalName);
				_verticalScale = (ScaleGraphic)CollectionUtils.SelectFirst(Graphics, graphic => graphic is ScaleGraphic && graphic.Name == _verticalName);
			}

			/// <summary>
			/// Fires the <see cref="Graphic.Drawing"/> event.  Should be called by an <see cref="IRenderer"/>
			/// for each object just before it is drawn/rendered, hence the reason it is public.
			/// </summary>
			public override void OnDrawing()
			{
				base.CoordinateSystem = CoordinateSystem.Destination;
				_horizontalScale.CoordinateSystem = CoordinateSystem.Destination;
				_verticalScale.CoordinateSystem = CoordinateSystem.Destination;

				try
				{
					Rectangle hScaleBounds = ComputeScaleBounds(base.ParentPresentationImage, 0.10f, 0.05f);
					_horizontalScale.SetEndPoints(new PointF(hScaleBounds.Left, hScaleBounds.Bottom), new SizeF(hScaleBounds.Width, 0));
					_horizontalScale.Visible = true;

					Rectangle vScaleBounds = ComputeScaleBounds(base.ParentPresentationImage, 0.05f, 0.10f);
					_verticalScale.SetEndPoints(new PointF(vScaleBounds.Right, vScaleBounds.Top), new SizeF(0, vScaleBounds.Height));
					_verticalScale.Visible = true;
				}
				finally
				{
					base.ResetCoordinateSystem();
					_horizontalScale.ResetCoordinateSystem();
					_verticalScale.ResetCoordinateSystem();
				}

				base.OnDrawing();
			}

			/// <summary>
			/// Computes the maximum bounds for scales on a given <see cref="IPresentationImage"/>.
			/// </summary>
			/// <param name="presentationImage">The image to compute bounds for.</param>
			/// <param name="horizontalReduction">The percentage of width to subtract from both the client bounds and the source image bounds.</param>
			/// <param name="verticalReduction">The percentage of height to subtract from both the client bounds and the source image bounds.</param>
			/// <returns>The maximum scale bounds.</returns>
			private static Rectangle ComputeScaleBounds(IPresentationImage presentationImage, float horizontalReduction, float verticalReduction)
			{
				RectangleF clientBounds = presentationImage.ClientRectangle;
				float hReduction = horizontalReduction*Math.Min(1000f, clientBounds.Width);
				float vReduction = verticalReduction*Math.Min(1000f, clientBounds.Height);

				clientBounds = new RectangleF(clientBounds.X + hReduction, clientBounds.Y + vReduction, clientBounds.Width - 2*hReduction, clientBounds.Height - 2*vReduction);

				if (presentationImage is IImageGraphicProvider)
				{
					ImageGraphic imageGraphic = ((IImageGraphicProvider) presentationImage).ImageGraphic;
					Rectangle srcRectangle = new Rectangle(0, 0, imageGraphic.Columns, imageGraphic.Rows);

					RectangleF imageBounds = imageGraphic.SpatialTransform.ConvertToDestination(srcRectangle);
					imageBounds = RectangleUtilities.ConvertToPositiveRectangle(imageBounds);
					hReduction = horizontalReduction*imageBounds.Width;
					vReduction = verticalReduction*imageBounds.Height;

					imageBounds = new RectangleF(imageBounds.X + hReduction, imageBounds.Y + vReduction, imageBounds.Width - 2*hReduction, imageBounds.Height - 2*vReduction);
					return Rectangle.Round(RectangleUtilities.Intersect(imageBounds, clientBounds));
				}
				else
				{
					return Rectangle.Round(clientBounds);
				}
			}
		}
	}
}