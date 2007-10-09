using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.InteractiveGraphics
{
	public class PointsMemento : IMemento, IEnumerable<PointF>
	{
		List<PointF> _anchorPoints = new List<PointF>();

		public PointsMemento()
		{
		}

		public void Add(PointF point)
		{
			_anchorPoints.Add(point);
		}

		#region IEnumerable<PointF> Members

		public IEnumerator<PointF> GetEnumerator()
		{
			return _anchorPoints.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _anchorPoints.GetEnumerator();
		}

		#endregion
	}
}
