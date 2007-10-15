#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Security.Permissions;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Healthcare.Workflow.Reporting;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.ReportingWorkflow;
using ClearCanvas.Ris.Application.Common.Admin;
using ClearCanvas.Ris.Application.Services.Admin;

namespace ClearCanvas.Ris.Application.Services.ReportingWorkflow
{
    [ServiceImplementsContract(typeof(IReportingWorkflowService))]
    [ExtensionOf(typeof(ApplicationServiceExtensionPoint))]
    public class ReportingWorkflowService : WorkflowServiceBase, IReportingWorkflowService
    {
        public ReportingWorkflowService()
        {
            _worklistExtPoint = new ReportingWorklistExtensionPoint();
        }

        #region IReportingWorkflowService Members

        [ReadOperation]
        public SearchResponse Search(SearchRequest request)
        {
            ReportingWorkflowAssembler assembler = new ReportingWorkflowAssembler();
            
            IList<WorklistItem> result = PersistenceContext.GetBroker<IReportingWorklistBroker>().Search(
                request.SearchData.MrnID,
                request.SearchData.HealthcardID,
                request.SearchData.FamilyName,
                request.SearchData.GivenName,
                request.SearchData.AccessionNumber,
                request.SearchData.ShowActiveOnly);

            return new SearchResponse(CollectionUtils.Map<WorklistItem, ReportingWorklistItem, List<ReportingWorklistItem>>(result,
                delegate(WorklistItem item)
                {
                    return assembler.CreateReportingWorklistItem(item, this.PersistenceContext);
                }));
        }

        [ReadOperation]
        public ListWorklistsResponse ListWorklists(ListWorklistsRequest request)
        {
            WorklistAssembler assembler = new WorklistAssembler();
            return new ListWorklistsResponse(
                CollectionUtils.Map<Worklist, WorklistSummary, List<WorklistSummary>>(
                    this.PersistenceContext.GetBroker<IWorklistBroker>().FindAllReportingWorklists(this.CurrentUser),
                    delegate(Worklist worklist)
                    {
                        return assembler.GetWorklistSummary(worklist);
                    }));
        }


        [ReadOperation]
        public GetWorklistResponse GetWorklist(GetWorklistRequest request)
        {
            ReportingWorkflowAssembler assembler = new ReportingWorkflowAssembler();

            IList items = request.WorklistRef == null
                  ? GetWorklist(request.WorklistClassName)
                  : GetWorklist(request.WorklistRef);

            return new GetWorklistResponse(
                CollectionUtils.Map<WorklistItem, ReportingWorklistItem, List<ReportingWorklistItem>>(
                    items,
                    delegate(WorklistItem item)
                    {
                        return assembler.CreateReportingWorklistItem(item, this.PersistenceContext);
                    }));
        }

        [ReadOperation]
        public GetWorklistCountResponse GetWorklistCount(GetWorklistCountRequest request)
        {
            int count = request.WorklistRef == null
                            ? GetWorklistCount(request.WorklistClassName)
                            : GetWorklistCount(request.WorklistRef);

            return new GetWorklistCountResponse(count);
        }

        [ReadOperation]
        public LoadWorklistPreviewResponse LoadWorklistPreview(LoadWorklistPreviewRequest request)
        {
            ReportingWorkflowAssembler assembler = new ReportingWorkflowAssembler();
            return new LoadWorklistPreviewResponse(assembler.CreateReportingWorklistPreview(request.WorklistItem, this.PersistenceContext));
        }

        [ReadOperation]
        public GetOperationEnablementResponse GetOperationEnablement(GetOperationEnablementRequest request)
        {
            return new GetOperationEnablementResponse(GetOperationEnablement(new WorklistItemKey(request.ProcedureStepRef)));
        }

        [UpdateOperation]
        [OperationEnablement("CanClaimInterpretation")]
        public ClaimInterpretationResponse ClaimInterpretation(ClaimInterpretationRequest request)
        {
            InterpretationStep interpretation = PersistenceContext.Load<InterpretationStep>(request.InterpretationStepRef, EntityLoadFlags.CheckVersion);

            Operations.ClaimInterpretation op = new Operations.ClaimInterpretation();
            op.Execute(interpretation, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            return new ClaimInterpretationResponse(interpretation.GetRef());
        }

        [UpdateOperation]
        [OperationEnablement("CanStartInterpretation")]
        public StartInterpretationResponse StartInterpretation(StartInterpretationRequest request)
        {
            InterpretationStep interpretation = PersistenceContext.Load<InterpretationStep>(request.InterpretationStepRef, EntityLoadFlags.CheckVersion);

            Operations.StartInterpretation op = new Operations.StartInterpretation();
            op.Execute(interpretation, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            return new StartInterpretationResponse(interpretation.GetRef());
        }

        [UpdateOperation]
        [OperationEnablement("CanCompleteInterpretationForTranscription")]
        public CompleteInterpretationForTranscriptionResponse CompleteInterpretationForTranscription(CompleteInterpretationForTranscriptionRequest request)
        {
            InterpretationStep interpretation = PersistenceContext.Load<InterpretationStep>(request.InterpretationStepRef, EntityLoadFlags.CheckVersion);
            Staff supervisor = request.SupervisorRef == null ? null : PersistenceContext.Load<Staff>(request.SupervisorRef, EntityLoadFlags.Proxy);

            if (String.IsNullOrEmpty(request.ReportContent) == false)
            {
                Operations.SaveReport saveReportOp = new Operations.SaveReport();
                saveReportOp.Execute(interpretation, request.ReportContent, supervisor, this.PersistenceContext);
            }

            Operations.CompleteInterpretationForTranscription op = new Operations.CompleteInterpretationForTranscription();
            TranscriptionStep transcription = op.Execute(interpretation, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            CompleteInterpretationForTranscriptionResponse response = new CompleteInterpretationForTranscriptionResponse();
            response.InterpretationStepRef = interpretation.GetRef();
            response.TranscriptionStepRef = transcription.GetRef();
            return response;
        }

        [UpdateOperation]
        [OperationEnablement("CanCompleteInterpretationForVerification")]
        public CompleteInterpretationForVerificationResponse CompleteInterpretationForVerification(CompleteInterpretationForVerificationRequest request)
        {
            InterpretationStep interpretation = PersistenceContext.Load<InterpretationStep>(request.InterpretationStepRef, EntityLoadFlags.CheckVersion);
            Staff supervisor = request.SupervisorRef == null ? null : PersistenceContext.Load<Staff>(request.SupervisorRef, EntityLoadFlags.Proxy);
            if (Thread.CurrentPrincipal.IsInRole(AuthorityTokens.VerifyReport) == false && supervisor == null)
                throw new RequestValidationException(SR.ExceptionResidentReportMissingSupervisor);

            if (String.IsNullOrEmpty(request.ReportContent) == false)
            {
                Operations.SaveReport saveReportOp = new Operations.SaveReport();
                saveReportOp.Execute(interpretation, request.ReportContent, supervisor, this.PersistenceContext);
            }

            Operations.CompleteInterpretationForVerification op = new Operations.CompleteInterpretationForVerification();
            VerificationStep verification = op.Execute(interpretation, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            CompleteInterpretationForVerificationResponse response = new CompleteInterpretationForVerificationResponse();
            response.InterpretationStepRef = interpretation.GetRef();
            response.VerificationStepRef = verification.GetRef();
            return response;
        }

        [UpdateOperation]
        [OperationEnablement("CanCompleteInterpretationAndVerify")]
        [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.VerifyReport)]
        public CompleteInterpretationAndVerifyResponse CompleteInterpretationAndVerify(CompleteInterpretationAndVerifyRequest request)
        {
            InterpretationStep interpretation = PersistenceContext.Load<InterpretationStep>(request.InterpretationStepRef, EntityLoadFlags.CheckVersion);
            Staff supervisor = request.SupervisorRef == null ? null : PersistenceContext.Load<Staff>(request.SupervisorRef, EntityLoadFlags.Proxy);

            if (String.IsNullOrEmpty(request.ReportContent) == false)
            {
                Operations.SaveReport saveReportOp = new Operations.SaveReport();
                saveReportOp.Execute(interpretation, request.ReportContent, supervisor, this.PersistenceContext);
            }

            if (interpretation.ReportPart == null || String.IsNullOrEmpty(interpretation.ReportPart.Content))
                throw new RequestValidationException(SR.ExceptionVerifyWithNoReport);

            Operations.CompleteInterpretationAndVerify op = new Operations.CompleteInterpretationAndVerify();
            VerificationStep verification = op.Execute(interpretation, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            CompleteInterpretationAndVerifyResponse response = new CompleteInterpretationAndVerifyResponse();
            response.InterpretationStepRef = interpretation.GetRef();
            response.VerificationStepRef = verification.GetRef();
            return response;
        }

        [UpdateOperation]
        [OperationEnablement("CanCancelReportingStep")]
        public CancelReportingStepResponse CancelReportingStep(CancelReportingStepRequest request)
        {
            ReportingProcedureStep step = PersistenceContext.Load<ReportingProcedureStep>(request.ReportingStepRef, EntityLoadFlags.CheckVersion);

            Operations.CancelReportingStep op = new Operations.CancelReportingStep();
            InterpretationStep interpretation = op.Execute(step, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            CancelReportingStepResponse response = new CancelReportingStepResponse();
            response.ReportingStepRef = step.GetRef();
            response.InterpretationStepRef = interpretation == null ? null : interpretation.GetRef();
            return response;
        }

        [UpdateOperation]
        [OperationEnablement("CanStartVerification")]
        [PrincipalPermission(SecurityAction.Demand, Role = AuthorityTokens.VerifyReport)]
        public StartVerificationResponse StartVerification(StartVerificationRequest request)
        {
            VerificationStep verification = PersistenceContext.Load<VerificationStep>(request.VerificationStepRef, EntityLoadFlags.CheckVersion);

            Operations.StartVerification op = new Operations.StartVerification();
            op.Execute(verification, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            return new StartVerificationResponse(verification.GetRef());
        }

        [UpdateOperation]
        [OperationEnablement("CanCompleteVerification")]
        [PrincipalPermission(SecurityAction.Demand, Role=AuthorityTokens.VerifyReport)]
        public CompleteVerificationResponse CompleteVerification(CompleteVerificationRequest request)
        {
            VerificationStep verification = PersistenceContext.Load<VerificationStep>(request.VerificationStepRef, EntityLoadFlags.CheckVersion);

            if (String.IsNullOrEmpty(request.ReportContent) == false)
            {
                Operations.SaveReport saveReportOp = new Operations.SaveReport();
                saveReportOp.Execute(verification, request.ReportContent, null, this.PersistenceContext);
            }

            if (verification.ReportPart == null || String.IsNullOrEmpty(verification.ReportPart.Content))
                throw new RequestValidationException(SR.ExceptionVerifyWithNoReport);

            Operations.CompleteVerification op = new Operations.CompleteVerification();
            op.Execute(verification, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            return new CompleteVerificationResponse(verification.GetRef());
        }

        [UpdateOperation]
        [OperationEnablement("CanCreateAddendum")]
        public CreateAddendumResponse CreateAddendum(CreateAddendumRequest request)
        {
            VerificationStep verification = PersistenceContext.Load<VerificationStep>(request.VerificationStepRef, EntityLoadFlags.CheckVersion);

            Operations.CreateAddendum op = new Operations.CreateAddendum();
            InterpretationStep interpretation = op.Execute(verification, this.CurrentUserStaff, new PersistentWorkflow(this.PersistenceContext));

            PersistenceContext.SynchState();
            CreateAddendumResponse response = new CreateAddendumResponse();
            response.VerificationStepRef = verification.GetRef();
            response.InterpretationStepRef = interpretation.GetRef();
            return response;
        }

        [ReadOperation]
        public LoadReportForEditResponse LoadReportForEdit(LoadReportForEditRequest request)
        {
            ReportingProcedureStep step = PersistenceContext.Load<ReportingProcedureStep>(request.ReportingStepRef, EntityLoadFlags.CheckVersion);
            ReportingWorkflowAssembler assembler = new ReportingWorkflowAssembler();

            LoadReportForEditResponse response = new LoadReportForEditResponse();
            if (step.ReportPart == null)
            {
                response.Report = assembler.CreateReportSummary(step.RequestedProcedure, null, this.PersistenceContext);
                response.ReportPartIndex = 0;
            }
            else
            {
                response.Report = assembler.CreateReportSummary(step.RequestedProcedure, step.ReportPart.Report, this.PersistenceContext);
                response.ReportPartIndex = step.ReportPart.Index;
            }

            return response;
        }

        [UpdateOperation]
        [OperationEnablement("CanSaveReport")]
        public SaveReportResponse SaveReport(SaveReportRequest request)
        {
            ReportingProcedureStep step = PersistenceContext.Load<ReportingProcedureStep>(request.ReportingStepRef, EntityLoadFlags.CheckVersion);
            Staff supervisor = request.SupervisorRef == null ? null : PersistenceContext.Load<Staff>(request.SupervisorRef, EntityLoadFlags.Proxy);

            Operations.SaveReport op = new Operations.SaveReport();
            op.Execute(step, request.ReportContent, supervisor, this.PersistenceContext);

            PersistenceContext.SynchState();
            return new SaveReportResponse(step.GetRef());
        }

        [ReadOperation]
        public GetPriorReportResponse GetPriorReport(GetPriorReportRequest request)
        {
            ReportingWorkflowAssembler assembler = new ReportingWorkflowAssembler();

            ReportingProcedureStep step = PersistenceContext.Load<ReportingProcedureStep>(request.ReportingStepRef, EntityLoadFlags.Proxy);

            IList<Report> listReports = PersistenceContext.GetBroker<IReportingWorklistBroker>().GetPriorReport(step.RequestedProcedure.Order.Patient);
            List<ReportSummary> listSummary = CollectionUtils.Map<Report, ReportSummary, List<ReportSummary>>(listReports,
                delegate(Report report)
                {
                    return assembler.CreateReportSummary(report.Procedure, report, this.PersistenceContext);
                });

            return new GetPriorReportResponse(listSummary);
        }

        [ReadOperation]
        public GetRadiologistListResponse GetRadiologistList(GetRadiologistListRequest request)
        {
            StaffAssembler assembler = new StaffAssembler();

            StaffSearchCriteria criteria = new StaffSearchCriteria();
            criteria.Type.EqualTo(StaffType.PRAD);

            if (String.IsNullOrEmpty(request.SupervisorID) == false)
                criteria.Id.EqualTo(request.SupervisorID);

            IList<Staff> listRadiologists = PersistenceContext.GetBroker<IStaffBroker>().Find(criteria);

            return new GetRadiologistListResponse(
                CollectionUtils.Map<Staff, StaffSummary, List<StaffSummary>>(listRadiologists,
                delegate(Staff staff)
                    {
                        return assembler.CreateStaffSummary(staff, this.PersistenceContext);
                    }));
        }

        #endregion

        #region OperationEnablement Helpers

        public bool CanClaimInterpretation(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.ClaimInterpretation(), itemKey);
        }

        public bool CanStartInterpretation(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.StartInterpretation(), itemKey);
        }

        public bool CanCompleteInterpretationForTranscription(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.CompleteInterpretationForTranscription(), itemKey);
        }

        public bool CanCompleteInterpretationForVerification(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.CompleteInterpretationForVerification(), itemKey);
        }

        public bool CanCompleteInterpretationAndVerify(IWorklistItemKey itemKey)
        {
            if (!Thread.CurrentPrincipal.IsInRole(AuthorityTokens.VerifyReport))
                return false;

            return CanExecuteOperation(new Operations.CompleteInterpretationAndVerify(), itemKey);
        }

        public bool CanCancelReportingStep(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.CancelReportingStep(), itemKey);
        }

        public bool CanStartVerification(IWorklistItemKey itemKey)
        {
            if (!Thread.CurrentPrincipal.IsInRole(AuthorityTokens.VerifyReport))
                return false;

            return CanExecuteOperation(new Operations.StartVerification(), itemKey);
        }

        public bool CanCompleteVerification(IWorklistItemKey itemKey)
        {
            if (!Thread.CurrentPrincipal.IsInRole(AuthorityTokens.VerifyReport))
                return false;

            return CanExecuteOperation(new Operations.CompleteVerification(), itemKey);
        }

        public bool CanCreateAddendum(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.CreateAddendum(), itemKey);
        }

        public bool CanSaveReport(IWorklistItemKey itemKey)
        {
            return CanExecuteOperation(new Operations.SaveReport(), itemKey);
        }

        private bool CanExecuteOperation(Operations.ReportingOperation op, IWorklistItemKey itemKey)
        {
            if (itemKey is WorklistItemKey)
            {
                ReportingProcedureStep step = PersistenceContext.Load<ReportingProcedureStep>(((WorklistItemKey)itemKey).ReportingProcedureStep);
                return op.CanExecute(step, this.CurrentUserStaff);
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
