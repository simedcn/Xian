USE [ImageServer]
GO
/****** Object:  Table [dbo].[FilesystemTier]    Script Date: 09/14/2007 12:55:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemTier]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FilesystemTier](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_FilesystemTier_GUID]  DEFAULT (newid()),
	[Description] [nvarchar](128) NOT NULL,
	[TierId] [int] NOT NULL,
 CONSTRAINT [PK_FilesystemTier] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
/****** Object:  Table [dbo].[ServerTransferSyntax]    Script Date: 09/14/2007 12:56:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerTransferSyntax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerTransferSyntax](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerTransferSyntax_GUID]  DEFAULT (newid()),
	[Uid] [varchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_ServerTransferSyntax] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerPartition]    Script Date: 09/14/2007 12:55:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerPartition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerPartition](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerPartition_GUID]  DEFAULT (newid()),
	[Enabled] [bit] NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[AeTitle] [varchar](16) NOT NULL,
	[Port] [int] NOT NULL,
	[PartitionFolder] [nvarchar](16) NOT NULL,
 CONSTRAINT [PK_ServerPartition] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StatusEnum]    Script Date: 09/14/2007 12:56:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StatusEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StatusEnum_GUID]  DEFAULT (newid()),
	[StatusEnum] [smallint] NOT NULL,
	[Lookup] [varchar](16) NOT NULL,
	[Description] [nvarchar](16) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_StatusEnum] PRIMARY KEY CLUSTERED 
(
	[StatusEnum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TypeEnum]    Script Date: 09/14/2007 12:56:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TypeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_TypeEnum_GUID]  DEFAULT (newid()),
	[TypeEnum] [smallint] NOT NULL,
	[Lookup] [varchar](16) NOT NULL,
	[Description] [nvarchar](16) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_WorkQueueTypeEnum] PRIMARY KEY CLUSTERED 
(
	[TypeEnum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerSopClass]    Script Date: 09/14/2007 12:56:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerSopClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerSopClass](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_SopClass_GUID]  DEFAULT (newid()),
	[SopClassUid] [varchar](64) NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[NonImage] [bit] NOT NULL,
 CONSTRAINT [PK_SopClass] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ServerSopClass]') AND name = N'IX_SopClass_SopClassUid')
CREATE UNIQUE NONCLUSTERED INDEX [IX_SopClass_SopClassUid] ON [dbo].[ServerSopClass] 
(
	[SopClassUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
GO
/****** Object:  Table [dbo].[PartitionSopClass]    Script Date: 09/14/2007 12:55:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PartitionSopClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PartitionSopClass](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_PartitionSopClass_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[ServerSopClassGUID] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_PartitionSopClass] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Filesystem]    Script Date: 09/14/2007 12:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Filesystem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Filesystem](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Filesystem_GUID]  DEFAULT (newid()),
	[FilesystemTierGUID] [uniqueidentifier] NULL,
	[FilesystemPath] [nvarchar](256) NULL,
	[Enabled] [bit] NULL,
	[ReadOnly] [bit] NULL,
	[WriteOnly] [bit] NULL,
	[Description] [nvarchar](128) NULL,
 CONSTRAINT [PK_Filesystem] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
/****** Object:  Table [dbo].[DevicePreferredTransferSyntax]    Script Date: 09/14/2007 12:55:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DevicePreferredTransferSyntax](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_DevicePreferredTransferSyntax_GUID]  DEFAULT (newid()),
	[DeviceGUID] [uniqueidentifier] NOT NULL,
	[ServerSopClassGUID] [uniqueidentifier] NOT NULL,
	[ServerTransferSyntaxGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DevicePreferredTransferSyntax] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]') AND name = N'IX_DevicePreferredTransferSyntax')
CREATE CLUSTERED INDEX [IX_DevicePreferredTransferSyntax] ON [dbo].[DevicePreferredTransferSyntax] 
(
	[DeviceGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Series]    Script Date: 09/14/2007 12:55:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Series](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Series_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[StudyGUID] [uniqueidentifier] NOT NULL,
	[SeriesInstanceUid] [varchar](64) NOT NULL,
	[Modality] [varchar](16) NOT NULL,
	[SeriesNumber] [varchar](12) NULL,
	[SeriesDescription] [nvarchar](64) NULL,
	[NumberOfSeriesRelatedInstances] [int] NOT NULL,
	[PerformedProcedureStepStartDate] [varchar](8) NULL,
	[PerformedProcedureStepStartTime] [varchar](16) NULL,
	[StatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_Series] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = N'IX_Series_StudyGUID_SeriesInstanceUid')
CREATE UNIQUE CLUSTERED INDEX [IX_Series_StudyGUID_SeriesInstanceUid] ON [dbo].[Series] 
(
	[SeriesInstanceUid] ASC,
	[StudyGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = N'IX_Series_Modality')
CREATE NONCLUSTERED INDEX [IX_Series_Modality] ON [dbo].[Series] 
(
	[Modality] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[WorkQueue]    Script Date: 09/14/2007 12:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueue](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueue_GUID]  DEFAULT (newid()),
	[StudyStorageGUID] [uniqueidentifier] NOT NULL,
	[TypeEnum] [smallint] NOT NULL,
	[StatusEnum] [smallint] NOT NULL,
	[ExpirationTime] [datetime] NULL,
	[ScheduledTime] [datetime] NOT NULL,
	[InsertTime] [datetime] NOT NULL CONSTRAINT [DF_WorkQueue_InsertTime]  DEFAULT (getdate()),
	[Data] [xml] NULL,
 CONSTRAINT [PK_WorkQueue] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [QUEUES]
) ON [QUEUES]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND name = N'IX_WorkQueue_ScheduledTime')
CREATE NONCLUSTERED INDEX [IX_WorkQueue_ScheduledTime] ON [dbo].[WorkQueue] 
(
	[ScheduledTime] ASC
)
INCLUDE ( [StatusEnum]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[StorageFilesystem]    Script Date: 09/14/2007 12:56:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StorageFilesystem](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StorageFilesystem_GUID]  DEFAULT (newid()),
	[StudyStorageGUID] [uniqueidentifier] NOT NULL,
	[FilesystemGUID] [uniqueidentifier] NOT NULL,
	[StudyFolder] [varchar](8) NOT NULL,
 CONSTRAINT [PK_StorageFilesystem] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND name = N'IX_StorageFilesystem_StudyStorageGUID')
CREATE CLUSTERED INDEX [IX_StorageFilesystem_StudyStorageGUID] ON [dbo].[StorageFilesystem] 
(
	[StudyStorageGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND name = N'IX_StorageFilesystem_FilesystemGUID')
CREATE NONCLUSTERED INDEX [IX_StorageFilesystem_FilesystemGUID] ON [dbo].[StorageFilesystem] 
(
	[FilesystemGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[WorkQueueUid]    Script Date: 09/14/2007 12:56:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueueUid](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueueInstance_GUID]  DEFAULT (newid()),
	[WorkQueueGUID] [uniqueidentifier] NOT NULL,
	[SeriesInstanceUid] [varchar](64) NULL,
	[SopInstanceUid] [varchar](64) NULL,
 CONSTRAINT [PK_WorkQueueUid] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [QUEUES]
) ON [QUEUES]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]') AND name = N'IX_WorkQueueUid')
CREATE CLUSTERED INDEX [IX_WorkQueueUid] ON [dbo].[WorkQueueUid] 
(
	[WorkQueueGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 65) ON [QUEUES]
GO
/****** Object:  Table [dbo].[Study]    Script Date: 09/14/2007 12:56:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Study](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Study_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[PatientGUID] [uniqueidentifier] NOT NULL,
	[StudyInstanceUid] [varchar](64) NOT NULL,
	[PatientName] [nvarchar](64) NULL,
	[PatientId] [nvarchar](64) NULL,
	[PatientsBirthDate] [varchar](8) NULL,
	[PatientsSex] [varchar](2) NULL,
	[StudyDate] [varchar](8) NULL,
	[StudyTime] [varchar](16) NULL,
	[AccessionNumber] [nvarchar](16) NULL,
	[StudyId] [nvarchar](16) NULL,
	[StudyDescription] [nvarchar](64) NULL,
	[ReferringPhysiciansName] [nvarchar](64) NULL,
	[NumberOfStudyRelatedSeries] [int] NOT NULL,
	[NumberOfStudyRelatedInstances] [int] NOT NULL,
	[StatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_Study] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_AccessionNumber')
CREATE NONCLUSTERED INDEX [IX_Study_AccessionNumber] ON [dbo].[Study] 
(
	[AccessionNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientGUID')
CREATE NONCLUSTERED INDEX [IX_Study_PatientGUID] ON [dbo].[Study] 
(
	[PatientGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientId')
CREATE NONCLUSTERED INDEX [IX_Study_PatientId] ON [dbo].[Study] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientName')
CREATE NONCLUSTERED INDEX [IX_Study_PatientName] ON [dbo].[Study] 
(
	[PatientName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyDate')
CREATE NONCLUSTERED INDEX [IX_Study_StudyDate] ON [dbo].[Study] 
(
	[StudyDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyDescription')
CREATE NONCLUSTERED INDEX [IX_Study_StudyDescription] ON [dbo].[Study] 
(
	[StudyDescription] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyInstanceUid')
CREATE NONCLUSTERED INDEX [IX_Study_StudyInstanceUid] ON [dbo].[Study] 
(
	[StudyInstanceUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[RequestAttributes]    Script Date: 09/14/2007 12:55:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestAttributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RequestAttributes](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_RequestAttribute_GUID]  DEFAULT (newid()),
	[SeriesGUID] [uniqueidentifier] NOT NULL,
	[RequestedProcedureId] [nvarchar](16) NULL,
	[ScheduledProcedureStepId] [nvarchar](16) NULL,
 CONSTRAINT [PK_RequestAttribute] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RequestAttributes]') AND name = N'IX_RequestAttribute_SeriesGUID')
CREATE CLUSTERED INDEX [IX_RequestAttribute_SeriesGUID] ON [dbo].[RequestAttributes] 
(
	[SeriesGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 09/14/2007 12:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Device](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Device_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[AeTitle] [varchar](16) NOT NULL,
	[IpAddress] [varchar](16) NULL,
	[Port] [int] NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Dhcp] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StudyStorage]    Script Date: 09/14/2007 12:56:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudyStorage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StudyStorage](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StudyStorage_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[StudyInstanceUid] [varchar](64) NOT NULL,
	[InsertTime] [datetime] NOT NULL CONSTRAINT [DF_StudyStorage_InsertTime]  DEFAULT (getdate()),
	[LastAccessedTime] [datetime] NOT NULL CONSTRAINT [DF_StudyStorage_LastAccessedTime]  DEFAULT (getdate()),
	[Lock] [bit] NOT NULL CONSTRAINT [DF_StudyStorage_Lock]  DEFAULT ((0)),
	[StatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_StudyStorage] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StudyStorage]') AND name = N'IX_StudyStorage_PartitionGUID_StudyInstanceUid')
CREATE UNIQUE NONCLUSTERED INDEX [IX_StudyStorage_PartitionGUID_StudyInstanceUid] ON [dbo].[StudyStorage] 
(
	[ServerPartitionGUID] ASC,
	[StudyInstanceUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 09/14/2007 12:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Patient](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Patient_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[PatientName] [nvarchar](64) NULL,
	[PatientId] [nvarchar](64) NULL,
	[IssuerOfPatientId] [nvarchar](64) NULL,
	[NumberOfPatientRelatedStudies] [int] NOT NULL,
	[NumberOfPatientRelatedSeries] [int] NOT NULL,
	[NumberOfPatientRelatedInstances] [int] NOT NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  ForeignKey [FK_Device_ServerPartition]    Script Date: 09/14/2007 12:55:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Device_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Device]'))
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_ServerPartition]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_Device]    Script Date: 09/14/2007 12:55:40 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_Device] FOREIGN KEY([DeviceGUID])
REFERENCES [dbo].[Device] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_Device]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_ServerSopClass]    Script Date: 09/14/2007 12:55:41 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_ServerSopClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerSopClass] FOREIGN KEY([ServerSopClassGUID])
REFERENCES [dbo].[ServerSopClass] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerSopClass]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_ServerTransferSyntax]    Script Date: 09/14/2007 12:55:41 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_ServerTransferSyntax]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerTransferSyntax] FOREIGN KEY([ServerTransferSyntaxGUID])
REFERENCES [dbo].[ServerTransferSyntax] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerTransferSyntax]
GO
/****** Object:  ForeignKey [FK_Filesystem_FilesystemTier]    Script Date: 09/14/2007 12:55:43 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Filesystem_FilesystemTier]') AND parent_object_id = OBJECT_ID(N'[dbo].[Filesystem]'))
ALTER TABLE [dbo].[Filesystem]  WITH CHECK ADD  CONSTRAINT [FK_Filesystem_FilesystemTier] FOREIGN KEY([FilesystemTierGUID])
REFERENCES [dbo].[FilesystemTier] ([GUID])
GO
ALTER TABLE [dbo].[Filesystem] CHECK CONSTRAINT [FK_Filesystem_FilesystemTier]
GO
/****** Object:  ForeignKey [FK_Patient_ServerPartition]    Script Date: 09/14/2007 12:55:49 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Patient]'))
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_ServerPartition]
GO
/****** Object:  ForeignKey [FK_RequestAttribute_Series]    Script Date: 09/14/2007 12:55:51 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RequestAttribute_Series]') AND parent_object_id = OBJECT_ID(N'[dbo].[RequestAttributes]'))
ALTER TABLE [dbo].[RequestAttributes]  WITH CHECK ADD  CONSTRAINT [FK_RequestAttribute_Series] FOREIGN KEY([SeriesGUID])
REFERENCES [dbo].[Series] ([GUID])
GO
ALTER TABLE [dbo].[RequestAttributes] CHECK CONSTRAINT [FK_RequestAttribute_Series]
GO
/****** Object:  ForeignKey [FK_Series_ServerPartition]    Script Date: 09/14/2007 12:55:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_ServerPartition]
GO
/****** Object:  ForeignKey [FK_Series_StatusEnum]    Script Date: 09/14/2007 12:55:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_StatusEnum] FOREIGN KEY([StatusEnum])
REFERENCES [dbo].[StatusEnum] ([StatusEnum])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_StatusEnum]
GO
/****** Object:  ForeignKey [FK_Series_Study]    Script Date: 09/14/2007 12:55:56 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_Study]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_Study] FOREIGN KEY([StudyGUID])
REFERENCES [dbo].[Study] ([GUID])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_Study]
GO
/****** Object:  ForeignKey [FK_StorageFilesystem_Filesystem]    Script Date: 09/14/2007 12:56:05 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StorageFilesystem_Filesystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]'))
ALTER TABLE [dbo].[StorageFilesystem]  WITH CHECK ADD  CONSTRAINT [FK_StorageFilesystem_Filesystem] FOREIGN KEY([FilesystemGUID])
REFERENCES [dbo].[Filesystem] ([GUID])
GO
ALTER TABLE [dbo].[StorageFilesystem] CHECK CONSTRAINT [FK_StorageFilesystem_Filesystem]
GO
/****** Object:  ForeignKey [FK_StorageFilesystem_StudyStorage]    Script Date: 09/14/2007 12:56:05 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StorageFilesystem_StudyStorage]') AND parent_object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]'))
ALTER TABLE [dbo].[StorageFilesystem]  WITH CHECK ADD  CONSTRAINT [FK_StorageFilesystem_StudyStorage] FOREIGN KEY([StudyStorageGUID])
REFERENCES [dbo].[StudyStorage] ([GUID])
GO
ALTER TABLE [dbo].[StorageFilesystem] CHECK CONSTRAINT [FK_StorageFilesystem_StudyStorage]
GO
/****** Object:  ForeignKey [FK_Study_Patient]    Script Date: 09/14/2007 12:56:12 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_Patient] FOREIGN KEY([PatientGUID])
REFERENCES [dbo].[Patient] ([GUID])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_Patient]
GO
/****** Object:  ForeignKey [FK_Study_ServerPartition]    Script Date: 09/14/2007 12:56:12 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_ServerPartition]
GO
/****** Object:  ForeignKey [FK_Study_StatusEnum]    Script Date: 09/14/2007 12:56:12 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_StatusEnum] FOREIGN KEY([StatusEnum])
REFERENCES [dbo].[StatusEnum] ([StatusEnum])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_StatusEnum]
GO
/****** Object:  ForeignKey [FK_StudyStorage_ServerPartition]    Script Date: 09/14/2007 12:56:15 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudyStorage_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudyStorage]'))
ALTER TABLE [dbo].[StudyStorage]  WITH CHECK ADD  CONSTRAINT [FK_StudyStorage_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[StudyStorage] CHECK CONSTRAINT [FK_StudyStorage_ServerPartition]
GO
/****** Object:  ForeignKey [FK_StudyStorage_StatusEnum]    Script Date: 09/14/2007 12:56:15 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudyStorage_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudyStorage]'))
ALTER TABLE [dbo].[StudyStorage]  WITH CHECK ADD  CONSTRAINT [FK_StudyStorage_StatusEnum] FOREIGN KEY([StatusEnum])
REFERENCES [dbo].[StatusEnum] ([StatusEnum])
GO
ALTER TABLE [dbo].[StudyStorage] CHECK CONSTRAINT [FK_StudyStorage_StatusEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueue_StatusEnum]    Script Date: 09/14/2007 12:56:20 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_StatusEnum] FOREIGN KEY([StatusEnum])
REFERENCES [dbo].[StatusEnum] ([StatusEnum])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_StatusEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueue_StudyStorage]    Script Date: 09/14/2007 12:56:20 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_StudyStorage]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_StudyStorage] FOREIGN KEY([StudyStorageGUID])
REFERENCES [dbo].[StudyStorage] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_StudyStorage]
GO
/****** Object:  ForeignKey [FK_WorkQueue_TypeEnum]    Script Date: 09/14/2007 12:56:20 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_TypeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_TypeEnum] FOREIGN KEY([TypeEnum])
REFERENCES [dbo].[TypeEnum] ([TypeEnum])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_TypeEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueueUid_WorkQueue]    Script Date: 09/14/2007 12:56:22 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueueUid_WorkQueue]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]'))
ALTER TABLE [dbo].[WorkQueueUid]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueUid_WorkQueue] FOREIGN KEY([WorkQueueGUID])
REFERENCES [dbo].[WorkQueue] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueueUid] CHECK CONSTRAINT [FK_WorkQueueUid_WorkQueue]
GO
