﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca

// For information about the licensing and copyright of this software please
// contact ClearCanvas, Inc. at info@clearcanvas.ca

#endregion

#if UNIT_TESTS

using System.Linq;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Common.DicomServer.Tests;
using ClearCanvas.ImageViewer.StudyManagement.Core.ServiceProviders;
using NUnit.Framework;

namespace ClearCanvas.ImageViewer.StudyManagement.Core.Storage.DicomQuery.Tests
{
    [TestFixture]
    public class SeriesQueryTests : TestBase
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            var extensionFactory = new UnitTestExtensionFactory
                                       {
                                            { typeof(ServiceProviderExtensionPoint), typeof(DicomServerTestServiceProvider) },
                                            { typeof(ServiceProviderExtensionPoint), typeof(StudyStoreQueryServiceProvider) },
                                            { typeof (ServiceProviderExtensionPoint), typeof (ServerDirectoryServiceProvider) }
                                       };
            
            Platform.SetExtensionFactory(extensionFactory);
        }

        [Test]
        public void SelectAllSeries()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            var filters = new SeriesPropertyFilters(criteria);
            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(4, results.Count());
        }

        [Test]
        public void SelectBySeriesUid_Single()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.SeriesInstanceUid].SetStringValue("1.2.3.2");
            var filters = new SeriesPropertyFilters(criteria);
            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void SelectBySeriesUid_Multiple()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.SeriesInstanceUid].SetStringValue("1.2.3.2\\1.2.3.3");
            var filters = new SeriesPropertyFilters(criteria);
            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void SelectBySeriesDescription_Equals()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.SeriesDescription].SetString(0, "Series1");
            var filters = new SeriesPropertyFilters(criteria);

            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void SelectBySeriesDescription_Wildcard()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.SeriesDescription].SetString(0, "*1");
            var filters = new SeriesPropertyFilters(criteria);

            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void SelectByModality_Equals()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.Modality].SetString(0, "MR");
            var filters = new SeriesPropertyFilters(criteria);

            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(2, results.Count());

            criteria[DicomTags.Modality].SetString(0, "KO");
            filters = new SeriesPropertyFilters(criteria);

            results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void SelectBySeriesNumber()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            criteria[DicomTags.SeriesNumber].SetInt32(0, 2);
            var filters = new SeriesPropertyFilters(criteria);

            var results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(1, results.Count());

            criteria[DicomTags.SeriesNumber].SetInt32(0, 6);
            filters = new SeriesPropertyFilters(criteria);

            results = filters.FilterResults(study.GetSeries().Cast<Series>());
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void AssertUniqueKeys()
        {
            Study study = CreateTestStudy1();
            var criteria = new DicomAttributeCollection();
            var filters = new SeriesPropertyFilters(criteria);
            var results = filters.FilterResults(study.GetSeries().Cast<Series>()).ToList();
            var converted = filters.ConvertResultsToDataSets(results);
            foreach (var result in converted)
            {
                //It's 5 because of InstanceAvailability, RetrieveAE, SpecificCharacterSet.
                Assert.AreEqual(5, result.Count);
                Assert.IsNotEmpty(result[DicomTags.StudyInstanceUid]);
                Assert.IsNotEmpty(result[DicomTags.SeriesInstanceUid]);
            }
        }

    }
}

#endif