﻿using System;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.ServiceModel.Query;
using ClearCanvas.ImageViewer.Common.StudyManagement;

namespace ClearCanvas.ImageViewer.Explorer.Dicom.SeriesDetails
{
    public class SeriesTableItem : ISeriesIdentifier, ISeriesEntryData
    {
        private readonly SeriesEntry _entry;

        public SeriesTableItem(SeriesEntry entry)
        {
            _entry = entry;
        }

        private ISeriesIdentifier Series { get { return _entry.Series; } }
        private ISeriesEntryData Data { get { return _entry.Data; } }

        public DateTime? DeleteTime
        {
            get { return Data.DeleteTime; }
            set { Data.DeleteTime = value; }
        }

        public string[] SourceAETitlesInSeries
        {
            get { return Data.SourceAETitlesInSeries; }
            set { Data.SourceAETitlesInSeries = value; }
        }

        public string StudyInstanceUid
        {
            get { return Series.StudyInstanceUid; }
        }

        public string SeriesInstanceUid
        {
            get { return Series.SeriesInstanceUid; }
        }

        public string Modality
        {
            get { return Series.Modality; }
        }

        public string SeriesDescription
        {
            get { return Series.SeriesDescription; }
        }

        public int? NumberOfSeriesRelatedInstances
        {
            get { return Series.NumberOfSeriesRelatedInstances; }
        }

        public string SpecificCharacterSet
        {
            get { return Series.SpecificCharacterSet; }
        }

        public string RetrieveAeTitle
        {
            get { return Series.RetrieveAeTitle; }
        }

        public string InstanceAvailability
        {
            get { return Series.InstanceAvailability; }
        }

        public IApplicationEntity RetrieveAE
        {
            get { return Series.RetrieveAE; }
        }

        public int? SeriesNumber
        {
            get { return Series.SeriesNumber; }
        }

        #region ISeriesIdentifier Members

        int? ISeriesIdentifier.SeriesNumber
        {
            get { return Series.SeriesNumber; }
        }

        #endregion

        #region ISeriesData Members

        int ISeriesData.SeriesNumber
        {
            get { return SeriesNumber.HasValue ? SeriesNumber.Value : 0; }
        }

        #endregion
    }
}
