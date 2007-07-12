using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Rendering;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// An image where pixel values are indices into a LUT.
	/// </summary>
	public abstract class IndexedImageGraphic : ImageGraphic
	{
		#region Private fields

		private int _bitsStored;
		private int _highBit;
		private bool _isSigned;
		//private IndexedPixelData _pixelDataWrapper;

		#endregion

		#region Protected constructors

		protected IndexedImageGraphic(
			int rows,
			int columns,
			int bitsAllocated,
			int bitsStored,
			int highBit,
			bool isSigned)
			: base(
				rows,
				columns,
				bitsAllocated)
		{
			Initialize(bitsStored, highBit, isSigned);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="IndexedImageGraphic"/>
		/// with the specified image parameters.
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		/// <param name="bitsAllocated"></param>
		/// <param name="bitsStored"></param>
		/// <param name="highBit"></param>
		/// <param name="isSigned"></param>
		/// <param name="pixelData"></param>
		protected IndexedImageGraphic(
			int rows,
			int columns,
			int bitsAllocated,
			int bitsStored,
			int highBit,
			bool isSigned,
			byte[] pixelData)
			: base(
				rows,
				columns,
				bitsAllocated,
				pixelData)
		{
			Initialize(bitsStored, highBit, isSigned);
		}

		protected IndexedImageGraphic(
			int rows,
			int columns,
			int bitsAllocated,
			int bitsStored,
			int highBit,
			bool isSigned,
			PixelDataGetter pixelDataGetter)
			: base(
				rows,
				columns,
				bitsAllocated,
				pixelDataGetter)
		{
			Initialize(bitsStored, highBit, isSigned);
		}

		private void Initialize(int bitsStored, int highBit, bool isSigned)
		{
			DicomValidator.ValidateBitsStored(bitsStored);
			DicomValidator.ValidateHighBit(highBit);

			_bitsStored = bitsStored;
			_highBit = highBit;
			_isSigned = isSigned;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets the number of bits stored in the image.
		/// </summary>
		/// <remarks>The number of bits stored does not necessarily equal the number
		/// of bits allocated. Values of 8, 10, 12 and 16 are typical.</remarks>
		public int BitsStored
		{
			get { return _bitsStored; }
		}

		/// <summary>
		/// Gets the high bit in the image.
		/// </summary>
		/// <remarks>Theoretically, the high bit does not necessarily have to equal
		/// Bits Stored - 1.  But in almost all cases this assumption is true; we
		/// too make this assumption.</remarks>
		public int HighBit
		{
			get { return _highBit; }
		}

		/// <summary>
		/// Gets a value indicating whether the image's pixel data is signed.
		/// </summary>
		public bool IsSigned
		{
			get { return _isSigned; }
		}

		public new IndexedPixelData PixelData
		{
			get
			{
				return base.PixelData as IndexedPixelData;
			}
		}

		/// <summary>
		/// The output of the LUT pipeline.
		/// </summary>
		/// <remarks>
		/// Each entry in the <see cref="OutputLUT"/> array is 32-bit ARGB value.
		/// When an <see cref="IRenderer"/> renders an <see cref="IndexedImageGraphic"/>, it should
		/// use <see cref="OutputLUT"/> to determine the ARGB value to display for a given pixel value.
		/// </remarks>
		public abstract int[] OutputLUT { get; }

		#endregion

		protected override PixelData CreatePixelDataWrapper()
		{
			if (this.PixelDataRaw != null)
			{
				return new IndexedPixelData(
									this.Rows,
									this.Columns,
									this.BitsPerPixel,
									this.BitsStored,
									this.HighBit,
									this.IsSigned,
									this.PixelDataRaw);
			}
			else
			{
				return new IndexedPixelData(
									this.Rows,
									this.Columns,
									this.BitsPerPixel,
									this.BitsStored,
									this.HighBit,
									this.IsSigned,
									this.PixelDataGetter);
			}
		}
	}
}
