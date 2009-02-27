﻿#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.InteractiveGraphics;
using System.Drawing;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.RoiGraphics;

namespace ClearCanvas.ImageViewer.Tools.Measurement
{
	[Cloneable]
	internal class ProtractorInteractiveGraphic : PolyLineInteractiveGraphic
	{
		[CloneIgnore]
		private InvariantArcPrimitive _arc;
		private readonly int _arcRadius = 20;

		protected ProtractorInteractiveGraphic (ProtractorInteractiveGraphic source, ICloningContext context)
			: base(source, context)
		{
			context.CloneFields(source, this);
		}

		public ProtractorInteractiveGraphic() : base(3)
		{
			base.Graphics.Add(_arc = new InvariantArcPrimitive());
			_arc.Visible = false;
		}

		public override Color Color
		{
			get
			{
				return base.Color;
			}
			set
			{
				_arc.Color = value;
				base.Color = value;
			}
		}

		public override void OnDrawing()
		{
			base.OnDrawing();
			
			if (this.PolyLine.Count == 3)
			{
				_arc.Visible = IsArcVisible();

				if (_arc.Visible)
					CalculateArc();
			}
		}

		public override bool HitTest(Point point)
		{
			if (_arc.Visible)
				return _arc.HitTest(point) || base.HitTest(point);
			else
				return base.HitTest(point);
		}

		private void CalculateArc()
		{
			this.PolyLine.CoordinateSystem = CoordinateSystem.Destination;
			_arc.CoordinateSystem = CoordinateSystem.Destination;

			// The arc center is the vertex of the protractor
			_arc.AnchorPoint = this.PolyLine[1];

			_arc.InvariantTopLeft = new PointF(-_arcRadius, -_arcRadius);
			_arc.InvariantBottomRight = new PointF(_arcRadius, _arcRadius);

			float startAngle, sweepAngle;
			CalculateAngles(out startAngle, out sweepAngle);
			_arc.StartAngle = startAngle;
			_arc.SweepAngle = sweepAngle;
	
			_arc.ResetCoordinateSystem();
			this.PolyLine.ResetCoordinateSystem();
		}

		private void CalculateAngles(out float startAngle, out float sweepAngle)
		{
			this.PolyLine.CoordinateSystem = CoordinateSystem.Destination;

			sweepAngle = -(float)Vector.SubtendedAngle(
				this.PolyLine[0],
				this.PolyLine[1],
				this.PolyLine[2]);


			// Define a horizontal ray
			PointF zeroDegreePoint = this.PolyLine[1];
			zeroDegreePoint.X += 50;

			startAngle = (float) Vector.SubtendedAngle(
			                      	this.PolyLine[0],
			                      	this.PolyLine[1],
			                      	zeroDegreePoint);

			this.PolyLine.ResetCoordinateSystem();
		}

		private bool IsArcVisible()
		{
			// Arc should only be visible if the arc radius is smaller than both of the
			// two arms of the angle
			this.PolyLine.CoordinateSystem = CoordinateSystem.Destination;
			Vector3D vertexPositionVector = new Vector3D(this.PolyLine[1].X, this.PolyLine[1].Y, 0);
			Vector3D a = new Vector3D(this.PolyLine[0].X, this.PolyLine[0].Y, 0) - vertexPositionVector;
			Vector3D b = new Vector3D(this.PolyLine[2].X, this.PolyLine[2].Y, 0) - vertexPositionVector;
			this.PolyLine.ResetCoordinateSystem();

			return a.Magnitude > _arcRadius && b.Magnitude > _arcRadius;
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			_arc = CollectionUtils.SelectFirst(base.Graphics,
				delegate(IGraphic test) { return test is InvariantArcPrimitive; }) as InvariantArcPrimitive;

			Platform.CheckForNullReference(_arc, "_arc");
		}

		public override Roi CreateRoiInformation()
		{
			return new ProtractorRoiInfo(this);
		}
	}
}
