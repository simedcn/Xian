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
    using ClearCanvas.Dicom;
    using ClearCanvas.ImageServer.Enterprise;

   public class WorkQueueUidUpdateColumns : EntityUpdateColumns
   {
       public WorkQueueUidUpdateColumns()
       : base("WorkQueueUid")
       {}
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="WorkQueueGUID")]
        public ServerEntityKey WorkQueueKey
        {
            set { SubParameters["WorkQueueKey"] = new EntityUpdateColumn<ServerEntityKey>("WorkQueueKey", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Failed")]
        public Boolean Failed
        {
            set { SubParameters["Failed"] = new EntityUpdateColumn<Boolean>("Failed", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Duplicate")]
        public Boolean Duplicate
        {
            set { SubParameters["Duplicate"] = new EntityUpdateColumn<Boolean>("Duplicate", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="FailureCount")]
        public Int16 FailureCount
        {
            set { SubParameters["FailureCount"] = new EntityUpdateColumn<Int16>("FailureCount", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="GroupID")]
        public String GroupID
        {
            set { SubParameters["GroupID"] = new EntityUpdateColumn<String>("GroupID", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="RelativePath")]
        public String RelativePath
        {
            set { SubParameters["RelativePath"] = new EntityUpdateColumn<String>("RelativePath", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="Extension")]
        public String Extension
        {
            set { SubParameters["Extension"] = new EntityUpdateColumn<String>("Extension", value); }
        }
       [DicomField(DicomTags.SeriesInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SeriesInstanceUid")]
        public String SeriesInstanceUid
        {
            set { SubParameters["SeriesInstanceUid"] = new EntityUpdateColumn<String>("SeriesInstanceUid", value); }
        }
       [DicomField(DicomTags.SopInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="WorkQueueUid", ColumnName="SopInstanceUid")]
        public String SopInstanceUid
        {
            set { SubParameters["SopInstanceUid"] = new EntityUpdateColumn<String>("SopInstanceUid", value); }
        }
    }
}
