/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 1.3.24
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace ClearCanvas.Common.Dicom {

using System;
using System.Text;

public class DcmCharString : DcmByteString {
  private IntPtr swigCPtr;

  internal DcmCharString(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmCharStringUpcast(cPtr), cMemoryOwn) {
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmCharString obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected DcmCharString() : this(IntPtr.Zero, false) {
  }

  ~DcmCharString() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmCharString(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmCharString(DcmTag tag, uint len) : this(DCMTKPINVOKE.new_DcmCharString__SWIG_0(DcmTag.getCPtr(tag), len), true) {
  }

  public DcmCharString(DcmCharString old) : this(DCMTKPINVOKE.new_DcmCharString__SWIG_1(DcmCharString.getCPtr(old)), true) {
  }

}

}
