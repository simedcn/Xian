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
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scp;
using ClearCanvas.Dicom.Utilities;
using ClearCanvas.ImageViewer.Common;
using ClearCanvas.ImageViewer.Common.LocalDataStore;
using ClearCanvas.ImageViewer.Common.WorkItem;
using ClearCanvas.ImageViewer.Dicom.Core;
using ClearCanvas.ImageViewer.StudyManagement.Storage;

namespace ClearCanvas.ImageViewer.Shreds.DicomServer
{
	[ExtensionOf(typeof(DicomScpExtensionPoint<IDicomServerContext>))]
	public class ImageStorageScpExtension : StoreScpExtension
	{
		public ImageStorageScpExtension()
			: base(GetSupportedSops())
		{}

		private static IEnumerable<SupportedSop> GetSupportedSops()
		{
			foreach (SopClass sopClass in GetSopClasses(DicomServerSettings.Instance.ImageStorageSopClasses))
			{
				SupportedSop supportedSop = new SupportedSop();
				supportedSop.SopClass = sopClass;

				supportedSop.AddSyntax(TransferSyntax.ExplicitVrLittleEndian);
				supportedSop.AddSyntax(TransferSyntax.ImplicitVrLittleEndian);

				foreach (TransferSyntax transferSyntax in GetTransferSyntaxes(DicomServerSettings.Instance.StorageTransferSyntaxes))
				{
					if (transferSyntax.DicomUid.UID != TransferSyntax.ExplicitVrLittleEndianUid &&
						transferSyntax.DicomUid.UID != TransferSyntax.ImplicitVrLittleEndianUid)
					{
						supportedSop.AddSyntax(transferSyntax);
					}
				}

				yield return supportedSop;
			}
		}
	}

	[ExtensionOf(typeof(DicomScpExtensionPoint<IDicomServerContext>))]
	public class NonImageStorageScpExtension : StoreScpExtension
	{
		public NonImageStorageScpExtension()
			: base(GetSupportedSops())
		{ }

		private static IEnumerable<SupportedSop> GetSupportedSops()
		{
			foreach (SopClass sopClass in GetSopClasses(DicomServerSettings.Instance.NonImageStorageSopClasses))
			{
				SupportedSop supportedSop = new SupportedSop();
				supportedSop.SopClass = sopClass;
				supportedSop.AddSyntax(TransferSyntax.ExplicitVrLittleEndian);
				supportedSop.AddSyntax(TransferSyntax.ImplicitVrLittleEndian);
				yield return supportedSop;
			}
		}
	}

	public abstract class StoreScpExtension : ScpExtension, IDicomScp<IDicomServerContext>
	{
		protected StoreScpExtension(IEnumerable<SupportedSop> supportedSops)
			: base(supportedSops)
		{
		}

		protected static IEnumerable<SopClass> GetSopClasses(SopClassConfigurationElementCollection config)
		{
			foreach (SopClassConfigurationElement element in config)
			{
				if (!String.IsNullOrEmpty(element.Uid))
				{
					SopClass sopClass = SopClass.GetSopClass(element.Uid);
					if (sopClass != null)
						yield return sopClass;
				}
			}
		}

		protected static IEnumerable<TransferSyntax> GetTransferSyntaxes(TransferSyntaxConfigurationElementCollection config)
		{
			foreach (TransferSyntaxConfigurationElement element in config)
			{
				if (!String.IsNullOrEmpty(element.Uid))
				{
					TransferSyntax syntax = TransferSyntax.GetTransferSyntax(element.Uid);
					if (syntax != null)
					{
						//at least for now, restrict to available codecs for compressed syntaxes.
						if (!syntax.Encapsulated || ClearCanvas.Dicom.Codec.DicomCodecRegistry.GetCodec(syntax) != null)
							yield return syntax;
					}
				}
			}
		}

		public override bool OnReceiveRequest(ClearCanvas.Dicom.Network.DicomServer server, 
			ServerAssociationParameters association, byte presentationID, DicomMessage message)
		{
			string studyInstanceUid = null;
			string seriesInstanceUid = null;
			DicomUid sopInstanceUid;

			bool ok = message.DataSet[DicomTags.SopInstanceUid].TryGetUid(0, out sopInstanceUid);
			if (ok) ok = message.DataSet[DicomTags.SeriesInstanceUid].TryGetString(0, out seriesInstanceUid);
			if (ok) ok = message.DataSet[DicomTags.StudyInstanceUid].TryGetString(0, out studyInstanceUid);

			if (!ok)
			{
				Platform.Log(LogLevel.Error, "Unable to retrieve UIDs from request message, sending failure status.");

				server.SendCStoreResponse(presentationID, message.MessageId, sopInstanceUid.UID,
					DicomStatuses.ProcessingFailure);

				return true;
			}

		    var context = new DicomReceiveImportContext(association.CalledAE);
            context.StudyWorkItems.ItemAdded +=delegate(object sender, DictionaryEventArgs<string, WorkItem> e)
                                                   {
                                                       try
                                                       {
                                                           PublishManager<IWorkItemActivityCallback>.Publish("WorkItemChanged", WorkItemHelper.FromWorkItem(e.Item));
                                                       }
                                                       catch (Exception x)
                                                       {
                                                           Platform.Log(LogLevel.Warn, x, "Unexpected error attempting to publish WorkItem status");
                                                       }                                                                                                                                      
                                                   };

		    var importer = new SopInstanceImporter(context);

		    var result = importer.Import(message,BadFileBehaviourEnum.Ignore, FileImportBehaviourEnum.Save);
            if (result.Successful)
            {
                if (!String.IsNullOrEmpty(result.AccessionNumber))
                    Platform.Log(LogLevel.Info, "Received SOP Instance {0} from {1} to {2} (A#:{3} StudyUid:{4})",
                                 result.SopInstanceUid, association.CallingAE, association.CalledAE, result.AccessionNumber,
                                 result.StudyInstanceUid);
                else
                    Platform.Log(LogLevel.Info, "Received SOP Instance {0} from {1} to {2} (StudyUid:{3})",
                                 result.SopInstanceUid, association.CallingAE, association.CalledAE,
                                 result.StudyInstanceUid);

                //OnFileReceived(association.CallingAE,string.Empty);
            }
            else
            {
                Platform.Log(LogLevel.Warn, "Failure importing sop: {0}", result.ErrorMessage);
                //OnReceiveError(message, result.ErrorMessage, association.CallingAE);
            }

		    server.SendCStoreResponse(presentationID, message.MessageId, message.AffectedSopInstanceUid, result.DicomStatus);
               
			return true;
		}

		private static void OnFileReceived(string fromAE, string filename)
		{
		    var info = new StoreScpReceivedFileInformation
		                   {
		                       AETitle = fromAE,
                               FileName = filename
		                   };
		    LocalDataStoreEventPublisher.Instance.FileReceived(info);
		}

		private static void OnReceiveError(DicomMessage message, string error, string fromAE)
		{
		    var info = new ReceiveErrorInformation
		                   {
		                       FromAETitle = fromAE,
		                       ErrorMessage = error,
		                       StudyInformation =
		                           new StudyInformation
		                               {
		                                   PatientId = message.DataSet[DicomTags.PatientId].GetString(0, ""),
		                                   PatientsName = message.DataSet[DicomTags.PatientsName].GetString(0, ""),
		                                   StudyDate = DateParser.Parse(message.DataSet[DicomTags.StudyDate].GetString(0, "")),
		                                   StudyDescription = message.DataSet[DicomTags.StudyDescription].GetString(0, ""),
		                                   StudyInstanceUid = message.DataSet[DicomTags.StudyInstanceUid].GetString(0, "")
		                               }
		                   };


		    LocalDataStoreEventPublisher.Instance.ReceiveError(info);
		}
	}
}
