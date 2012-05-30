﻿using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ClearCanvas.Dicom;
using System;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.ImageViewer.StudyManagement.Core.Storage.DicomQuery
{
    internal class PropertyFilters<TDatabaseObject>
        where TDatabaseObject : class
    {
        private readonly DicomAttributeCollection _criteria;
        private IList<IPropertyFilter<TDatabaseObject>> _filters;

        public PropertyFilters(DicomAttributeCollection criteria)
        {
            _criteria = criteria;
        }

        private IEnumerable<IPropertyFilter<TDatabaseObject>> Filters
        {
            get { return _filters ?? (_filters = CreateFilters(_criteria)); }
        }

        protected virtual List<IPropertyFilter<TDatabaseObject>> CreateFilters(DicomAttributeCollection criteria)
        {
            var types = typeof (PropertyFilters<TDatabaseObject>).Assembly.GetTypes()
                .Where(t => typeof (IPropertyFilter<TDatabaseObject>).IsAssignableFrom(t)).ToList();

            var typesWithRightConstructor = (from type in types
                                            let constructor = type.GetConstructor(new[] {typeof (DicomAttributeCollection)})
                                            where constructor != null
                                            select type).ToList();
#if DEBUG
            var typesMissingConstructor = types.Where(type => !typesWithRightConstructor.Contains(type)).ToList();
            Debug.Assert(typesMissingConstructor.Count == 0);
#endif
            return typesWithRightConstructor.Select(type => Activator.CreateInstance(type, new object[] {criteria}))
                    .Cast<IPropertyFilter<TDatabaseObject>>().ToList();
        }

        protected virtual IQueryable<TDatabaseObject> Query(IQueryable<TDatabaseObject> initialQuery)
        {
            return Filters.Aggregate(initialQuery, (current, filter) => filter.AddToQuery(current));
        }

        public IEnumerable<TDatabaseObject> Query(Table<TDatabaseObject> table)
        {
            var query = Query(table.AsQueryable());
            return FilterResults(query.AsEnumerable());
        }

        public IEnumerable<TDatabaseObject> FilterResults(IEnumerable<TDatabaseObject> items)
        {
            return Filters.Aggregate(items, (current, filter) => filter.FilterResults(current));
        }

        public List<DicomAttributeCollection> ConvertResultsToDataSets(IEnumerable<TDatabaseObject> results)
        {
            var dicomResults = new List<DicomAttributeCollection>();
            foreach (var result in results)
            {
                var dicomResult = new DicomAttributeCollection();
                foreach (var filter in Filters)
                    filter.SetAttributeValue(result, dicomResult);

                dicomResults.Add(dicomResult);
            }

            return dicomResults;
        }
    }

    internal class StudyPropertyFilters : PropertyFilters<Study>
    {
        public StudyPropertyFilters(DicomAttributeCollection criteria)
            : base(criteria)
        {
        }

        protected override List<IPropertyFilter<Study>> CreateFilters(DicomAttributeCollection criteria)
        {
            var filters = base.CreateFilters(criteria);
            var modalitiesInStudyPath = new DicomTagPath(DicomTags.ModalitiesInStudy);
            var dicomFilters = filters.OfType<DicomPropertyFilter<Study>>().ToList();
            var modalitiesInStudyIndex = dicomFilters.FindIndex(f => f.Path.Equals(modalitiesInStudyPath));
            var modalitiesInStudyFilter = filters[modalitiesInStudyIndex];

            //Because of the potentially complex joins of the same initial query over and over, move this one to the front.
            filters.RemoveAt(modalitiesInStudyIndex);
            filters.Insert(0, modalitiesInStudyFilter);
            return filters;
        }

        protected override IQueryable<Study> Query(IQueryable<Study> initialQuery)
        {
            var query = base.Query(initialQuery);

            //We don't want to return anything that is scheduled to be deleted or reindexed.
            query = query.Where(s => !s.Deleted && !s.Reindex);

            return query;
        }
    }

    internal class SeriesPropertyFilters : PropertyFilters<Series>
    {
        public SeriesPropertyFilters(DicomAttributeCollection criteria)
            : base(criteria)
        {
        }
    }

    internal class SopInstancePropertyFilters : PropertyFilters<SopInstance>
    {
        public SopInstancePropertyFilters(DicomAttributeCollection criteria)
            : base(criteria)
        {
        }
    }
}