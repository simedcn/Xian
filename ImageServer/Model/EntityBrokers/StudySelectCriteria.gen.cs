#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0//

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;

    public partial class StudySelectCriteria : EntitySelectCriteria
    {
        public StudySelectCriteria()
        : base("Study")
        {}
        public StudySelectCriteria(StudySelectCriteria other)
        : base(other)
        {}
        public override object Clone()
        {
            return new StudySelectCriteria(this);
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyInstanceUid")]
        public ISearchCondition<String> StudyInstanceUid
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyInstanceUid"))
              {
                 SubCriteria["StudyInstanceUid"] = new SearchCondition<String>("StudyInstanceUid");
              }
              return (ISearchCondition<String>)SubCriteria["StudyInstanceUid"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ServerPartitionGUID")]
        public ISearchCondition<ServerEntityKey> ServerPartitionKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("ServerPartitionKey"))
              {
                 SubCriteria["ServerPartitionKey"] = new SearchCondition<ServerEntityKey>("ServerPartitionKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["ServerPartitionKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientGUID")]
        public ISearchCondition<ServerEntityKey> PatientKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientKey"))
              {
                 SubCriteria["PatientKey"] = new SearchCondition<ServerEntityKey>("PatientKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["PatientKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="NumberOfStudyRelatedSeries")]
        public ISearchCondition<Int32> NumberOfStudyRelatedSeries
        {
            get
            {
              if (!SubCriteria.ContainsKey("NumberOfStudyRelatedSeries"))
              {
                 SubCriteria["NumberOfStudyRelatedSeries"] = new SearchCondition<Int32>("NumberOfStudyRelatedSeries");
              }
              return (ISearchCondition<Int32>)SubCriteria["NumberOfStudyRelatedSeries"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="NumberOfStudyRelatedInstances")]
        public ISearchCondition<Int32> NumberOfStudyRelatedInstances
        {
            get
            {
              if (!SubCriteria.ContainsKey("NumberOfStudyRelatedInstances"))
              {
                 SubCriteria["NumberOfStudyRelatedInstances"] = new SearchCondition<Int32>("NumberOfStudyRelatedInstances");
              }
              return (ISearchCondition<Int32>)SubCriteria["NumberOfStudyRelatedInstances"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudySizeInKB")]
        public ISearchCondition<Decimal> StudySizeInKB
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudySizeInKB"))
              {
                 SubCriteria["StudySizeInKB"] = new SearchCondition<Decimal>("StudySizeInKB");
              }
              return (ISearchCondition<Decimal>)SubCriteria["StudySizeInKB"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="SpecificCharacterSet")]
        public ISearchCondition<String> SpecificCharacterSet
        {
            get
            {
              if (!SubCriteria.ContainsKey("SpecificCharacterSet"))
              {
                 SubCriteria["SpecificCharacterSet"] = new SearchCondition<String>("SpecificCharacterSet");
              }
              return (ISearchCondition<String>)SubCriteria["SpecificCharacterSet"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyStorageGUID")]
        public ISearchCondition<ServerEntityKey> StudyStorageKey
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyStorageKey"))
              {
                 SubCriteria["StudyStorageKey"] = new SearchCondition<ServerEntityKey>("StudyStorageKey");
              }
              return (ISearchCondition<ServerEntityKey>)SubCriteria["StudyStorageKey"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsName")]
        public ISearchCondition<String> PatientsName
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientsName"))
              {
                 SubCriteria["PatientsName"] = new SearchCondition<String>("PatientsName");
              }
              return (ISearchCondition<String>)SubCriteria["PatientsName"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientId")]
        public ISearchCondition<String> PatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientId"))
              {
                 SubCriteria["PatientId"] = new SearchCondition<String>("PatientId");
              }
              return (ISearchCondition<String>)SubCriteria["PatientId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="IssuerOfPatientId")]
        public ISearchCondition<String> IssuerOfPatientId
        {
            get
            {
              if (!SubCriteria.ContainsKey("IssuerOfPatientId"))
              {
                 SubCriteria["IssuerOfPatientId"] = new SearchCondition<String>("IssuerOfPatientId");
              }
              return (ISearchCondition<String>)SubCriteria["IssuerOfPatientId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsBirthDate")]
        public ISearchCondition<String> PatientsBirthDate
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientsBirthDate"))
              {
                 SubCriteria["PatientsBirthDate"] = new SearchCondition<String>("PatientsBirthDate");
              }
              return (ISearchCondition<String>)SubCriteria["PatientsBirthDate"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsAge")]
        public ISearchCondition<String> PatientsAge
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientsAge"))
              {
                 SubCriteria["PatientsAge"] = new SearchCondition<String>("PatientsAge");
              }
              return (ISearchCondition<String>)SubCriteria["PatientsAge"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="PatientsSex")]
        public ISearchCondition<String> PatientsSex
        {
            get
            {
              if (!SubCriteria.ContainsKey("PatientsSex"))
              {
                 SubCriteria["PatientsSex"] = new SearchCondition<String>("PatientsSex");
              }
              return (ISearchCondition<String>)SubCriteria["PatientsSex"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyDate")]
        public ISearchCondition<String> StudyDate
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyDate"))
              {
                 SubCriteria["StudyDate"] = new SearchCondition<String>("StudyDate");
              }
              return (ISearchCondition<String>)SubCriteria["StudyDate"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyTime")]
        public ISearchCondition<String> StudyTime
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyTime"))
              {
                 SubCriteria["StudyTime"] = new SearchCondition<String>("StudyTime");
              }
              return (ISearchCondition<String>)SubCriteria["StudyTime"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="AccessionNumber")]
        public ISearchCondition<String> AccessionNumber
        {
            get
            {
              if (!SubCriteria.ContainsKey("AccessionNumber"))
              {
                 SubCriteria["AccessionNumber"] = new SearchCondition<String>("AccessionNumber");
              }
              return (ISearchCondition<String>)SubCriteria["AccessionNumber"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyId")]
        public ISearchCondition<String> StudyId
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyId"))
              {
                 SubCriteria["StudyId"] = new SearchCondition<String>("StudyId");
              }
              return (ISearchCondition<String>)SubCriteria["StudyId"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="StudyDescription")]
        public ISearchCondition<String> StudyDescription
        {
            get
            {
              if (!SubCriteria.ContainsKey("StudyDescription"))
              {
                 SubCriteria["StudyDescription"] = new SearchCondition<String>("StudyDescription");
              }
              return (ISearchCondition<String>)SubCriteria["StudyDescription"];
            } 
        }
        [EntityFieldDatabaseMappingAttribute(TableName="Study", ColumnName="ReferringPhysiciansName")]
        public ISearchCondition<String> ReferringPhysiciansName
        {
            get
            {
              if (!SubCriteria.ContainsKey("ReferringPhysiciansName"))
              {
                 SubCriteria["ReferringPhysiciansName"] = new SearchCondition<String>("ReferringPhysiciansName");
              }
              return (ISearchCondition<String>)SubCriteria["ReferringPhysiciansName"];
            } 
        }
    }
}
