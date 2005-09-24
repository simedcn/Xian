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

public class OFDateTime : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal OFDateTime(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(OFDateTime obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~OFDateTime() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_OFDateTime(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public OFDateTime() : this(DCMTKPINVOKE.new_OFDateTime__SWIG_0(), true) {
  }

  public OFDateTime(OFDateTime dateTime) : this(DCMTKPINVOKE.new_OFDateTime__SWIG_1(OFDateTime.getCPtr(dateTime)), true) {
  }

  public OFDateTime(OFDate dateVal, OFTime timeVal) : this(DCMTKPINVOKE.new_OFDateTime__SWIG_2(OFDate.getCPtr(dateVal), OFTime.getCPtr(timeVal)), true) {
  }

  public virtual void clear() {
    DCMTKPINVOKE.OFDateTime_clear(swigCPtr);
  }

  public virtual bool isValid() {
    return DCMTKPINVOKE.OFDateTime_isValid(swigCPtr);
  }

  public bool setDateTime(uint year, uint month, uint day, uint hour, uint minute, double second, double timeZone) {
    return DCMTKPINVOKE.OFDateTime_setDateTime__SWIG_0(swigCPtr, year, month, day, hour, minute, second, timeZone);
  }

  public bool setDateTime(uint year, uint month, uint day, uint hour, uint minute, double second) {
    return DCMTKPINVOKE.OFDateTime_setDateTime__SWIG_1(swigCPtr, year, month, day, hour, minute, second);
  }

  public bool setDate(OFDate dateVal) {
    return DCMTKPINVOKE.OFDateTime_setDate(swigCPtr, OFDate.getCPtr(dateVal));
  }

  public bool setTime(OFTime timeVal) {
    return DCMTKPINVOKE.OFDateTime_setTime(swigCPtr, OFTime.getCPtr(timeVal));
  }

  public bool setCurrentDateTime() {
    return DCMTKPINVOKE.OFDateTime_setCurrentDateTime(swigCPtr);
  }

  public bool setISOFormattedDateTime(string formattedDateTime) {
    return DCMTKPINVOKE.OFDateTime_setISOFormattedDateTime(swigCPtr, formattedDateTime);
  }

  public OFDate getDate() {
    return new OFDate(DCMTKPINVOKE.OFDateTime_getDate(swigCPtr), false);
  }

  public OFTime getTime() {
    return new OFTime(DCMTKPINVOKE.OFDateTime_getTime(swigCPtr), false);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime, bool showSeconds, bool showFraction, bool showTimeZone, bool showDelimiter) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_0(swigCPtr, formattedDateTime, showSeconds, showFraction, showTimeZone, showDelimiter);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime, bool showSeconds, bool showFraction, bool showTimeZone) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_1(swigCPtr, formattedDateTime, showSeconds, showFraction, showTimeZone);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime, bool showSeconds, bool showFraction) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_2(swigCPtr, formattedDateTime, showSeconds, showFraction);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime, bool showSeconds) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_3(swigCPtr, formattedDateTime, showSeconds);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_4(swigCPtr, formattedDateTime);
  }

  public bool getISOFormattedDateTime(StringBuilder formattedDateTime, bool showSeconds, bool showFraction, bool showTimeZone, bool showDelimiter, string dateTimeSeparator) {
    return DCMTKPINVOKE.OFDateTime_getISOFormattedDateTime__SWIG_5(swigCPtr, formattedDateTime, showSeconds, showFraction, showTimeZone, showDelimiter, dateTimeSeparator);
  }

  public static OFDateTime getCurrentDateTime() {
    return new OFDateTime(DCMTKPINVOKE.OFDateTime_getCurrentDateTime(), true);
  }

}

}
