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

using NUnit.Framework;

namespace ClearCanvas.ImageViewer.Tests
{
	[TestFixture]
	public class ImageBoxAndDisplaySetInteractionTest
	{
		public ImageBoxAndDisplaySetInteractionTest()
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
		public void SetDisplaySet()
		{
			ImageBox imageBox = new ImageBox();
			IDisplaySet displaySet1 = new DisplaySet();
			IDisplaySet displaySet2 = new DisplaySet();
			PresentationImage image1 = new TestPresentationImage();
			PresentationImage image2 = new TestPresentationImage();
			displaySet1.PresentationImages.Add(image1);
			displaySet2.PresentationImages.Add(image2);

			imageBox.DisplaySet = displaySet1;
			Assert.IsTrue(displaySet1.Visible);
			Assert.AreEqual(imageBox, displaySet1.ImageBox);

			imageBox.DisplaySet = null;
			Assert.IsFalse(displaySet1.Visible);
			Assert.IsNull(displaySet1.ImageBox);

			imageBox.DisplaySet = displaySet1;
			Assert.IsTrue(displaySet1.Visible);
			Assert.AreEqual(imageBox, displaySet1.ImageBox);

			imageBox.DisplaySet = displaySet2;
			Assert.IsTrue(displaySet2.Visible);
			Assert.IsFalse(displaySet1.Visible);
			Assert.AreEqual(imageBox, displaySet2.ImageBox);
			Assert.IsNull(displaySet1.ImageBox);
		}

		[Test]
		public void LayoutImageBoxes()
		{
			IImageViewer viewer = new ImageViewerComponent();
			viewer.PhysicalWorkspace.SetImageBoxGrid(2, 1);

			IDisplaySet displaySet1 = new DisplaySet();
			IDisplaySet displaySet2 = new DisplaySet();
			PresentationImage image1 = new TestPresentationImage();
			PresentationImage image2 = new TestPresentationImage();
			displaySet1.PresentationImages.Add(image1);
			displaySet2.PresentationImages.Add(image2);

			viewer.PhysicalWorkspace.ImageBoxes[0].DisplaySet = displaySet1;
			viewer.PhysicalWorkspace.ImageBoxes[1].DisplaySet = displaySet2;
			viewer.PhysicalWorkspace.SetImageBoxGrid(1, 1);

			Assert.IsFalse(displaySet1.Visible);
			Assert.IsFalse(displaySet2.Visible);
		}
	}
}

#endif