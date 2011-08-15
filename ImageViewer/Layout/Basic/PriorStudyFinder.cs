#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.ServiceModel.Query;
using ClearCanvas.ImageViewer.Configuration;
using ClearCanvas.ImageViewer.Services.ServerTree;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Layout.Basic
{
	[ExceptionPolicyFor(typeof (LoadPriorStudiesException))]
	[ExtensionOf(typeof (ExceptionPolicyExtensionPoint))]
	public class PriorStudyLoaderExceptionPolicy : IExceptionPolicy
	{
		#region IExceptionPolicy Members

		public void Handle(Exception e, IExceptionHandlingContext exceptionHandlingContext)
		{
			if (e is LoadPriorStudiesException)
			{
				exceptionHandlingContext.Log(LogLevel.Error, e);

				Handle(e as LoadPriorStudiesException, exceptionHandlingContext);
			}
		}

		#endregion

	    private static bool _lastResultsComplete = true;

		private static void Handle(LoadPriorStudiesException exception, IExceptionHandlingContext context)
		{
			if (exception.FindFailed)
			{
				context.ShowMessageBox(SR.MessageSearchForPriorsFailed);
                return;
			}

            var summary = new StringBuilder();
            if (!exception.FindResultsComplete)
            {
                //Only show the message if the last query was successful/complete.
                if (_lastResultsComplete)
                    summary.Append(SR.MessagePriorsIncomplete);

                _lastResultsComplete = false;
            }
            else
            {
                _lastResultsComplete = true;
            }

            if (ShouldShowLoadErrorMessage(exception))
            {
                if (summary.Length > 0)
                    summary.AppendLine(); summary.AppendLine();

                summary.AppendLine(SR.MessageLoadPriorsErrorPrefix);
                summary.Append(exception.GetExceptionSummary());
            }

            if (summary.Length > 0)
                context.ShowMessageBox(summary.ToString());
		}

		private static bool ShouldShowLoadErrorMessage(LoadPriorStudiesException exception)
		{
            if (exception.FindResultsComplete)

			if (exception.IncompleteCount > 0)
				return true;

			if (exception.NotFoundCount > 0)
				return true;

			if (exception.UnknownFailureCount > 0)
				return true;

			return false;
		}
	}

	[ExtensionOf(typeof (PriorStudyFinderExtensionPoint))]
	public class PriorStudyFinder : ImageViewer.PriorStudyFinder
	{
		private volatile bool _cancel;

		public override PriorStudyFinderResult FindPriorStudies()
		{
			_cancel = false;
			var results = new Dictionary<string, StudyItem>();

			IPatientReconciliationStrategy reconciliationStrategy = new DefaultPatientReconciliationStrategy();
			reconciliationStrategy.SetStudyTree(Viewer.StudyTree);

			var patientIds = new Dictionary<string, string>();
			foreach (Patient patient in Viewer.StudyTree.Patients)
			{
				if (_cancel)
					break;

				IPatientData reconciled = reconciliationStrategy.ReconcileSearchCriteria(patient);
				patientIds[reconciled.PatientId] = reconciled.PatientId;
			}

		    bool resultsComplete = true;
		    bool anySuccessful = false;

		    foreach (var query in DefaultServers.GetQueryInterfaces(true))
		    {
                if (_cancel) break;

		        try
		        {
                    using (var bridge = new StudyRootQueryBridge(query))
                    {
                        foreach (string patientId in patientIds.Keys)
                        {
                            var identifier = new StudyRootStudyIdentifier { PatientId = patientId };

                            IList<StudyRootStudyIdentifier> studies = bridge.StudyQuery(identifier);
                            foreach (StudyRootStudyIdentifier study in studies)
                            {
                                if (_cancel)
                                    break;

                                //Eliminate false positives right away.
                                IPatientData reconciled = reconciliationStrategy.ReconcilePatientInformation(study);
                                if (reconciled == null)
                                    continue;

                                StudyItem studyItem = ConvertToStudyItem(study);
                                if (studyItem == null || results.ContainsKey(studyItem.StudyInstanceUid))
                                    continue;

                                if (!results.ContainsKey(studyItem.StudyInstanceUid))
                                    results[studyItem.StudyInstanceUid] = studyItem;
                            }
                        }
                    }

		            anySuccessful = true;
		        }
		        catch (Exception e)
		        {
		            resultsComplete = false;
                    Platform.Log(LogLevel.Error, e, "Failed to query server: {0}", query);
		        }
		    }

            if (!anySuccessful)
                throw new Exception("The search for prior studies has failed.");

            return new PriorStudyFinderResult(new StudyItemList(results.Values), resultsComplete);
        }

		public override void Cancel()
		{
			_cancel = true;
		}

		private static StudyItem ConvertToStudyItem(IStudyRootStudyIdentifier study)
		{
			string studyLoaderName;
			ApplicationEntity applicationEntity = null;

			IServerTreeNode node = FindServer(study.RetrieveAeTitle);
			if (node.IsLocalDataStore)
			{
				studyLoaderName = "DICOM_LOCAL";
			}
			else if (node.IsServer)
			{
				var server = (Server) node;
				studyLoaderName = server.IsStreaming ? "CC_STREAMING" : "DICOM_REMOTE";

				applicationEntity = new ApplicationEntity(server.Host, server.AETitle, server.Name, server.Port,
				                                          server.IsStreaming, server.HeaderServicePort, server.WadoServicePort);
			}
			else // (node == null)
			{
				Platform.Log(LogLevel.Warn,
				             String.Format("Unable to find server information '{0}' in order to load study '{1}'",
				                           study.RetrieveAeTitle, study.StudyInstanceUid));

				return null;
			}

			var item = new StudyItem(study, applicationEntity, studyLoaderName){ InstanceAvailability = study.InstanceAvailability };
			if (String.IsNullOrEmpty(item.InstanceAvailability))
				item.InstanceAvailability = "ONLINE";

			return item;
		}

		private static IServerTreeNode FindServer(string retrieveAETitle)
		{
			var serverTree = new ServerTree();
			if (retrieveAETitle == serverTree.RootNode.LocalDataStoreNode.GetClientAETitle())
				return serverTree.RootNode.LocalDataStoreNode;

			List<Server> remoteServers = DefaultServers.SelectFrom(serverTree);
			foreach (Server server in remoteServers)
			{
				if (server.AETitle == retrieveAETitle)
					return server;
			}

			return null;
		}
	}
}
