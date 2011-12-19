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
    using ClearCanvas.Enterprise.Core;
    using ClearCanvas.ImageServer.Enterprise;
    using ClearCanvas.ImageServer.Model.EntityBrokers;

    [Serializable]
    public partial class PartitionSopClass: ServerEntity
    {
        #region Constructors
        public PartitionSopClass():base("PartitionSopClass")
        {}
        public PartitionSopClass(
             ServerEntityKey _serverPartitionKey_
            ,ServerEntityKey _serverSopClassKey_
            ,Boolean _enabled_
            ):base("PartitionSopClass")
        {
            ServerPartitionKey = _serverPartitionKey_;
            ServerSopClassKey = _serverSopClassKey_;
            Enabled = _enabled_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="ServerPartitionGUID")]
        public ServerEntityKey ServerPartitionKey
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="ServerSopClassGUID")]
        public ServerEntityKey ServerSopClassKey
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="PartitionSopClass", ColumnName="Enabled")]
        public Boolean Enabled
        { get; set; }
        #endregion

        #region Static Methods
        static public PartitionSopClass Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public PartitionSopClass Load(IPersistenceContext read, ServerEntityKey key)
        {
            IPartitionSopClassEntityBroker broker = read.GetBroker<IPartitionSopClassEntityBroker>();
            PartitionSopClass theObject = broker.Load(key);
            return theObject;
        }
        static public PartitionSopClass Insert(PartitionSopClass entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                PartitionSopClass newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public PartitionSopClass Insert(IUpdateContext update, PartitionSopClass entity)
        {
            IPartitionSopClassEntityBroker broker = update.GetBroker<IPartitionSopClassEntityBroker>();
            PartitionSopClassUpdateColumns updateColumns = new PartitionSopClassUpdateColumns();
            updateColumns.ServerPartitionKey = entity.ServerPartitionKey;
            updateColumns.ServerSopClassKey = entity.ServerSopClassKey;
            updateColumns.Enabled = entity.Enabled;
            PartitionSopClass newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
