using System;
using System.Drawing;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.InputManagement;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	public class CreateRoiGraphicState : CreateGraphicState
	{
		private StatefulCompositeGraphic _childGraphic;

		public CreateRoiGraphicState(RoiGraphic roiGraphic)
			: base(roiGraphic)
		{
		}

		protected RoiGraphic ROIGraphic
		{
			get { return this.StandardStatefulGraphic as RoiGraphic; }
		}

		public override bool Start(IMouseInformation mouseInformation)
		{
			if (_childGraphic == null)
			{
				PointF mousePoint = new PointF(mouseInformation.Location.X, mouseInformation.Location.Y);
#if MONO
				Size offset = new Size(0, 30);
#else
				SizeF offset = new SizeF(0, 30);
#endif

				PointF calloutLocation = mousePoint - offset;
				this.ROIGraphic.Callout.CoordinateSystem = CoordinateSystem.Destination;
				this.ROIGraphic.Callout.Location = calloutLocation;
				this.ROIGraphic.Callout.EndPoint = mousePoint;
				this.ROIGraphic.Callout.ResetCoordinateSystem();

				this.ROIGraphic.Roi.CoordinateSystem = CoordinateSystem.Destination;

				for (int i = 0; i < this.ROIGraphic.Roi.ControlPoints.Count; i++ )
					this.ROIGraphic.Roi.ControlPoints[i] = mousePoint;

				this.ROIGraphic.Roi.ResetCoordinateSystem();

				this.ROIGraphic.Callout.Draw();

				_childGraphic = this.ROIGraphic.Roi;
				_childGraphic.State.SupportUndo = false;
				_childGraphic.StateChanged += new EventHandler<GraphicStateChangedEventArgs>(OnRoiStateChanged);
			}

			return _childGraphic.Start(mouseInformation);
		}


		public override bool Track(IMouseInformation mouseInformation)
		{
			// Route mouse move message to the child roi object
			if (_childGraphic != null)
				return _childGraphic.Track(mouseInformation);

			return false;
		}

		public override bool Stop(IMouseInformation mouseInformation)
		{
			// Route mouse move message to the child roi object
			if (_childGraphic != null)
				return _childGraphic.Stop(mouseInformation);

			return false;
		}

		private void OnRoiStateChanged(object sender, GraphicStateChangedEventArgs e)
		{
			// When the child ROI graphic transitions to the focus selected state,
			// it cause "this" to transition to selected state too.
			if (e.NewState is FocussedSelectedGraphicState)
			{
				_childGraphic.StateChanged -= new EventHandler<GraphicStateChangedEventArgs>(OnRoiStateChanged);
				_childGraphic = null;

				this.ROIGraphic.State = this.ROIGraphic.CreateFocussedSelectedState();

				// We're done creating, so create a command
				this.Command = new PositionGraphicCommand(this.ROIGraphic, PositionGraphicCommand.CreateOperation.Create);
				this.Command.Name = SR.CommandCreateROIGraphic;
				this.ROIGraphic.ImageViewer.CommandHistory.AddCommand(base.Command);
			}
		}

		public override string ToString()
		{
			return "CreateROIGraphicState\n";
		}
	}
}
