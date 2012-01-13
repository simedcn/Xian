#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0//

#endregion

// This file is auto-generated by the ClearCanvas.Model.SqlServer.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model
{
    using System;
    using System.Xml;
    using ClearCanvas.Dicom;
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class ServerSopClass: ServerEntity
    {
        #region Constructors
        public ServerSopClass():base("ServerSopClass")
        {}
        public ServerSopClass(
             String _sopClassUid_
            ,String _description_
            ,Boolean _nonImage_
            ):base("ServerSopClass")
        {
            SopClassUid = _sopClassUid_;
            Description = _description_;
            NonImage = _nonImage_;
        }
        #endregion

        #region Public Properties
        [DicomField(DicomTags.SopClassUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="ServerSopClass", ColumnName="SopClassUid")]
        public String SopClassUid
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerSopClass", ColumnName="Description")]
        public String Description
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ServerSopClass", ColumnName="NonImage")]
        public Boolean NonImage
        { get; set; }
        #endregion

        #region Static Methods
        static public ServerSopClass Load(ServerEntityKey key)
        {
            using (var read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public ServerSopClass Load(IPersistenceContext read, ServerEntityKey key)
        {
            var broker = read.GetBroker<IServerSopClassEntityBroker>();
            ServerSopClass theObject = broker.Load(key);
            return theObject;
        }
        static public ServerSopClass Insert(ServerSopClass entity)
        {
            using (var update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                ServerSopClass newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public ServerSopClass Insert(IUpdateContext update, ServerSopClass entity)
        {
            var broker = update.GetBroker<IServerSopClassEntityBroker>();
            var updateColumns = new ServerSopClassUpdateColumns();
            updateColumns.SopClassUid = entity.SopClassUid;
            updateColumns.Description = entity.Description;
            updateColumns.NonImage = entity.NonImage;
            ServerSopClass newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
