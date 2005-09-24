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

public class DcmProducer : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal DcmProducer(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmProducer obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected DcmProducer() : this(IntPtr.Zero, false) {
  }

  ~DcmProducer() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmProducer(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public virtual bool good() {
    return DCMTKPINVOKE.DcmProducer_good(swigCPtr);
  }

  public virtual OFCondition status() {
    return new OFCondition(DCMTKPINVOKE.DcmProducer_status(swigCPtr), true);
  }

  public virtual bool eos() {
    return DCMTKPINVOKE.DcmProducer_eos(swigCPtr);
  }

  public virtual uint avail() {
    return DCMTKPINVOKE.DcmProducer_avail(swigCPtr);
  }

  public virtual uint read(SWIGTYPE_p_void buf, uint buflen) {
    return DCMTKPINVOKE.DcmProducer_read(swigCPtr, SWIGTYPE_p_void.getCPtr(buf), buflen);
  }

  public virtual uint skip(uint skiplen) {
    return DCMTKPINVOKE.DcmProducer_skip(swigCPtr, skiplen);
  }

  public virtual void putback(uint num) {
    DCMTKPINVOKE.DcmProducer_putback(swigCPtr, num);
  }

}

}
