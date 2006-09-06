using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.OffisWrapper;

namespace ClearCanvas.ImageViewer.TextOverlay.Dicom.GeneralImage
{
	internal class ContentDateAnnotationItem : DicomDateAnnotationItem
	{
		public ContentDateAnnotationItem(IAnnotationItemProvider ownerProvider)
			: base("GeneralImage.ContentDate", ownerProvider)
		{
		}

		protected override DcmTagKey DicomTag
		{
			get { return Dcm.ContentDate; }
		}
	}
}
