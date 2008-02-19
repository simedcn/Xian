using System;
using System.Drawing;
using ClearCanvas.ImageViewer.Annotations;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.AnnotationProviders.Dicom
{
	internal class SliceLocationAnnotationItem : AnnotationItem
	{
		public SliceLocationAnnotationItem()
			: base("Dicom.ImagePlane.SliceLocation", new AnnotationResourceResolver(typeof(SliceLocationAnnotationItem).Assembly))
		{
		}

		public override string GetAnnotationText(IPresentationImage presentationImage)
		{
			if (presentationImage is IImageSopProvider)
			{
				ImageSop sop = ((IImageSopProvider) presentationImage).ImageSop;
				Vector3D normal = sop.ImagePlaneHelper.GetNormalVector();
				Vector3D positionCenterOfImage = sop.ImagePlaneHelper.ConvertToPatient(new PointF((sop.Columns - 1) / 2F, (sop.Rows - 1) / 2F));

				if (normal != null && positionCenterOfImage != null)
				{
					// Try to be a bit more specific when we have spatial information
					// by showing directional information (L, R, H, F, A, P) as well as
					// the slice location.
					float absX = Math.Abs(normal.X);
					float absY = Math.Abs(normal.Y);
					float absZ = Math.Abs(normal.Z);

					// Get the primary direction based on the largest component of the normal.
					if (absZ >= absY && absZ >= absX)
					{
						//mostly axial because Z >= X and Y
						string directionString = (positionCenterOfImage.Z >= 0F) ? SR.ValueDirectionalMarkersHead : SR.ValueDirectionalMarkersFoot;
						return string.Format("{0}{1:F1}", directionString, Math.Abs(positionCenterOfImage.Z));
					}
					else if (absY >= absX && absY >= absZ)
					{
						//mostly coronal because Y >= X and Z
						string directionString = (positionCenterOfImage.Y >= 0F) ? SR.ValueDirectionalMarkersPosterior : SR.ValueDirectionalMarkersAnterior;
						return string.Format("{0}{1:F1}", directionString, Math.Abs(positionCenterOfImage.Y));
					}
					else
					{
						//mostly sagittal because X >= Y and Z
						string directionString = (positionCenterOfImage.X >= 0F) ? SR.ValueDirectionalMarkersLeft : SR.ValueDirectionalMarkersRight;
						return string.Format("{0}{1:F1}", directionString, Math.Abs(positionCenterOfImage.X));
					}
				}
				else
				{
					return sop.SliceLocation.ToString("F1");
				}
			}

			return "";
		}
	}
}
