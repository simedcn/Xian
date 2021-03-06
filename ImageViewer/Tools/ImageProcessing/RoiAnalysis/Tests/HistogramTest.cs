#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

#if	UNIT_TESTS

#pragma warning disable 1591,0419,1574,1587

using System;
using System.Collections;
using System.Drawing;
using NUnit.Framework;

namespace ClearCanvas.ImageViewer.Tools.ImageProcessing.RoiAnalysis
{
	[TestFixture]
	public class HistogramTest
	{
		public HistogramTest()
		{
		}

		[TestFixtureSetUp]
		public void Init()
		{
		}
	
		[TestFixtureTearDown]
		public void Cleanup()
		{
		}

		[Test]
		public void Histogram()
		{
			int[] data = { 10, 20, 30, 40, 10, 20, 30, 40, 50, 50, 10, 20, 30 };
			Histogram histogram = new Histogram(0, 60, 5, data);

			int[] bins = histogram.Bins;
		}
	}
}

#endif