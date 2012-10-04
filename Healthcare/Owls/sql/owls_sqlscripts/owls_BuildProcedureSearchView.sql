USE [Ris]
GO
/****** Object:  StoredProcedure [dbo].[owls_BuildProcedureSearchView]    Script Date: 05/03/2010 16:55:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[owls_BuildProcedureSearchView] 
	@startAfter uniqueidentifier = null output,
	@minEndTime	datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- if @startAfter value is null, find the smallest Procedure OID
	if(@startAfter is null)
	begin
		select @startAfter = OID_ from Procedure_
		where OID_ in
		(
			select top 1 OID_ from Procedure_
			order by OID_ asc
		)
	end

	-- declare a table to hold batch of OIDs
	declare @oids table
	(
		Index_ int identity,
		OID_ uniqueidentifier
	)

	-- get the next batch of OIDs into the table
	insert into @oids (OID_)
		select top 100 OID_		from Procedure_ rp		where rp.OID_ > @startAfter		and (rp.EndTime_ > @minEndTime or rp.EndTime_ is null)		order by rp.OID_ asc
	-- declare a table to hold the batch of view items corresponding to the batch of OIDs
	declare @tmp table	(		OID_ uniqueidentifier,
		Version_ int,
		PatientProfileOID_ uniqueidentifier,
		PatientProfileVersion_ int,
		PatientOID_ uniqueidentifier,
		PatientVersion_ int,
		PatientProfileMrnId_ nvarchar(50),
		PatientProfileMrnAssigningAuthority_ nvarchar(255),
		PatientProfileHealthcardId_ nvarchar(30),
		PatientProfileHealthcardAssigningAuthority_ nvarchar(255),
		PatientProfileFamilyName_ nvarchar(255),
		PatientProfileGivenName_ nvarchar(255),
		PatientProfileMiddleName_ nvarchar(255),
		VisitOID_ uniqueidentifier,
		VisitVersion_ int,
		VisitPatientClass_ nvarchar(255),
		VisitCurrentLocationOID_ uniqueidentifier,
		OrderOID_ uniqueidentifier,
		OrderVersion_ int,
		OrderAccessionNumber_ nvarchar(255),
		OrderPriority_ nvarchar(255),
		OrderSchedulingRequestTime_ datetime,
		OrderScheduledStartTime_ datetime,
		OrderStartTime_ datetime,
		OrderEndTime_ datetime,
		DiagnosticServiceOID_ uniqueidentifier,
		DiagnosticServiceId_ nvarchar(30),
		DiagnosticServiceName_ nvarchar(100),
		OrderOrderingPractitionerOID_ uniqueidentifier,
		ProcedureOID_ uniqueidentifier,
		ProcedureVersion_ int,
		ProcedurePerformingFacilityOID_ uniqueidentifier,
		ProcedurePerformingDepartmentOID_ uniqueidentifier,
		ProcedureTypeOID_ uniqueidentifier,
		ProcedureTypeId_ nvarchar(30),
		ProcedureTypeName_ nvarchar(100),
		ProcedurePortable_ bit,
		ProcedureLaterality_ nvarchar(255),
		ProcedureScheduledStartTime_ datetime,
		ProcedureStartTime_ datetime,
		ProcedureEndTime_ datetime,
		ProcedureStatus_ nvarchar(255),
		ProcedureDowntimeRecoveryMode_ bit,
		ProcedureCheckInOID_ uniqueidentifier,
		ProcedureCheckInVersion_ int,
		ProcedureCheckInTime_ datetime,
		ProcedureCheckOutTime_ datetime
	)	-- generate a batch of view items and insert into the temp table	insert into @tmp	select		newid(),		1,		pp.OID_,
		pp.Version_,
		pp.PatientOID_,
		p.Version_,
		pp.MrnId_,
		pp.MrnAssigningAuthority_,
		pp.HealthcardId_,
		pp.HealthcardAssigningAuthority_,
		pp.FamilyName_,
		pp.GivenName_,
		pp.MiddleName_,
		v.OID_,
		v.Version_,
		v.PatientClass_,
		v.CurrentLocationOID_,
		o.OID_,
		o.Version_,
		o.AccessionNumber_,
		o.Priority_,
		o.SchedulingRequestTime_,
		o.ScheduledStartTime_,
		o.StartTime_,
		o.EndTime_,
		ds.OID_,
		ds.Id_,
		ds.Name_,
		o.OrderingPractitionerOID_,
		rp.OID_,
		rp.Version_,
		rp.PerformingFacilityOID_,
		rp.PerformingDepartmentOID_,
		rpt.OID_,
		rpt.Id_,
		rpt.Name_,
		rp.Portable_,
		rp.Laterality_,
		rp.ScheduledStartTime_,
		rp.StartTime_,
		rp.EndTime_,
		rp.Status_,
		rp.DowntimeRecoveryMode_,
		pc.OID_,
		pc.Version_,
		pc.CheckInTime_,
		pc.CheckOutTime_
	from Ris.dbo.Procedure_ rp	left join Ris.dbo.ProcedureType_ rpt on rp.ProcedureTypeOID_=rpt.OID_	left join Ris.dbo.ProcedureCheckIn_ pc on rp.ProcedureCheckInOID_=pc.OID_	left join Ris.dbo.Order_ o on rp.OrderOID_=o.OID_	left join Ris.dbo.DiagnosticService_ ds on o.DiagnosticServiceOID_=ds.OID_	left join Ris.dbo.Visit_ v on o.VisitOID_=v.OID_	left join Ris.dbo.Patient_ p on o.PatientOID_ = p.OID_	left join Ris.dbo.PatientProfile_ pp on p.OID_=pp.PatientOID_	where rp.OID_ in 	(		select OID_ from @oids	)	-- delete any rows in the temp table where PatientProfile and Performing Facility don't match	delete from @tmp	where OID_ in 	(		select t.OID_		from @tmp t		inner join Facility_ f on f.OID_ = t.ProcedurePerformingFacilityOID_		where f.InformationAuthority_ <> t.PatientProfileMrnAssigningAuthority_ 	)	-- copy the batch from the temp table into the view table	insert into owls_ProcedureSearchViewItem_		select * from @tmp

	-- return the highest OID processed, so it can be used as a bookmark for next call
	set @startAfter = null	--clear it so that we return null if no rows were processed
	select @startAfter = OID_
	from @oids
	where Index_ = (select max(Index_) from @oids)
END
