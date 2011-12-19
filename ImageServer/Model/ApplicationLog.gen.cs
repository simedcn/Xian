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
    public partial class ApplicationLog: ServerEntity
    {
        #region Constructors
        public ApplicationLog():base("ApplicationLog")
        {}
        public ApplicationLog(
             String _host_
            ,DateTime _timestamp_
            ,String _logLevel_
            ,String _thread_
            ,String _message_
            ,String _exception_
            ):base("ApplicationLog")
        {
            Host = _host_;
            Timestamp = _timestamp_;
            LogLevel = _logLevel_;
            Thread = _thread_;
            Message = _message_;
            Exception = _exception_;
        }
        #endregion

        #region Public Properties
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Host")]
        public String Host
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Timestamp")]
        public DateTime Timestamp
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="LogLevel")]
        public String LogLevel
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Thread")]
        public String Thread
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Message")]
        public String Message
        { get; set; }
        [EntityFieldDatabaseMappingAttribute(TableName="ApplicationLog", ColumnName="Exception")]
        public String Exception
        { get; set; }
        #endregion

        #region Static Methods
        static public ApplicationLog Load(ServerEntityKey key)
        {
            using (IReadContext read = PersistentStoreRegistry.GetDefaultStore().OpenReadContext())
            {
                return Load(read, key);
            }
        }
        static public ApplicationLog Load(IPersistenceContext read, ServerEntityKey key)
        {
            IApplicationLogEntityBroker broker = read.GetBroker<IApplicationLogEntityBroker>();
            ApplicationLog theObject = broker.Load(key);
            return theObject;
        }
        static public ApplicationLog Insert(ApplicationLog entity)
        {
            using (IUpdateContext update = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                ApplicationLog newEntity = Insert(update, entity);
                update.Commit();
                return newEntity;
            }
        }
        static public ApplicationLog Insert(IUpdateContext update, ApplicationLog entity)
        {
            IApplicationLogEntityBroker broker = update.GetBroker<IApplicationLogEntityBroker>();
            ApplicationLogUpdateColumns updateColumns = new ApplicationLogUpdateColumns();
            updateColumns.Host = entity.Host;
            updateColumns.Timestamp = entity.Timestamp;
            updateColumns.LogLevel = entity.LogLevel;
            updateColumns.Thread = entity.Thread;
            updateColumns.Message = entity.Message;
            updateColumns.Exception = entity.Exception;
            ApplicationLog newEntity = broker.Insert(updateColumns);
            return newEntity;
        }
        #endregion
    }
}
