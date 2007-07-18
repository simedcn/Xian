using System;
using System.Collections.Generic;
using System.Text;


namespace ClearCanvas.Dicom
{
    public class DicomSequenceItem : DicomAttributeCollection
    {
        public DicomSequenceItem()
            : base()
        {
        }

        internal DicomSequenceItem(DicomAttributeCollection source, bool copyBinary)
            : base(source, copyBinary)
        {
        }

        public override DicomAttributeCollection Copy()
        {
            return Copy(true);
        }

        public override DicomAttributeCollection Copy(bool copyBinary)
        {
            return new DicomSequenceItem(this, copyBinary);
        }

    }
}

