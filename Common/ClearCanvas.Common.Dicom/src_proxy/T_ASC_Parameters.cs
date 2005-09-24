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

public class T_ASC_Parameters : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal T_ASC_Parameters(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(T_ASC_Parameters obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~T_ASC_Parameters() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_T_ASC_Parameters(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public string ourImplementationClassUID {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_ourImplementationClassUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_ourImplementationClassUID(swigCPtr);
    } 
  }

  public string ourImplementationVersionName {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_ourImplementationVersionName(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_ourImplementationVersionName(swigCPtr);
    } 
  }

  public string theirImplementationClassUID {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_theirImplementationClassUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_theirImplementationClassUID(swigCPtr);
    } 
  }

  public string theirImplementationVersionName {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_theirImplementationVersionName(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_theirImplementationVersionName(swigCPtr);
    } 
  }

  public SWIGTYPE_p_DUL_ModeCallback modeCallback {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_modeCallback(swigCPtr, SWIGTYPE_p_DUL_ModeCallback.getCPtr(value));
    } 
    get {
      IntPtr cPtr = DCMTKPINVOKE.get_T_ASC_Parameters_modeCallback(swigCPtr);
      return (cPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_DUL_ModeCallback(cPtr, false);
    } 
  }

  public SWIGTYPE_p_DUL_ASSOCIATESERVICEPARAMETERS DULparams {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_DULparams(swigCPtr, SWIGTYPE_p_DUL_ASSOCIATESERVICEPARAMETERS.getCPtr(value));
    } 
    get {
      return new SWIGTYPE_p_DUL_ASSOCIATESERVICEPARAMETERS(DCMTKPINVOKE.get_T_ASC_Parameters_DULparams(swigCPtr), true);
    } 
  }

  public int ourMaxPDUReceiveSize {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_ourMaxPDUReceiveSize(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_ourMaxPDUReceiveSize(swigCPtr);
    } 
  }

  public int theirMaxPDUReceiveSize {
    set {
      DCMTKPINVOKE.set_T_ASC_Parameters_theirMaxPDUReceiveSize(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_ASC_Parameters_theirMaxPDUReceiveSize(swigCPtr);
    } 
  }

  public T_ASC_Parameters() : this(DCMTKPINVOKE.new_T_ASC_Parameters(), true) {
  }

}

}
