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

public class T_DIMSE_C_StoreRSP : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal T_DIMSE_C_StoreRSP(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(T_DIMSE_C_StoreRSP obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~T_DIMSE_C_StoreRSP() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_T_DIMSE_C_StoreRSP(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public ushort MessageIDBeingRespondedTo {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_MessageIDBeingRespondedTo(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_MessageIDBeingRespondedTo(swigCPtr);
    } 
  }

  public string AffectedSOPClassUID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_AffectedSOPClassUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_AffectedSOPClassUID(swigCPtr);
    } 
  }

  public T_DIMSE_DataSetType DataSetType {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_DataSetType(swigCPtr, (int)value);
    } 
    get {
      return (T_DIMSE_DataSetType)DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_DataSetType(swigCPtr);
    } 
  }

  public ushort DimseStatus {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_DimseStatus(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_DimseStatus(swigCPtr);
    } 
  }

  public string AffectedSOPInstanceUID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_AffectedSOPInstanceUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_AffectedSOPInstanceUID(swigCPtr);
    } 
  }

  public uint opts {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_StoreRSP_opts(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_StoreRSP_opts(swigCPtr);
    } 
  }

  public T_DIMSE_C_StoreRSP() : this(DCMTKPINVOKE.new_T_DIMSE_C_StoreRSP(), true) {
  }

}

}
