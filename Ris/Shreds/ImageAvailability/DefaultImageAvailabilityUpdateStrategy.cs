using System;
using System.Collections.Generic;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Workflow;

namespace ClearCanvas.Ris.Shreds.ImageAvailability
{
	/// <summary>
	/// The default update strategy does not depend on the presence of MPPS from the modality.  The strategy queries DICOM 
	/// server for a list of studies with a given accession number.  The NumberOfStudyRelatedInstances for all studies are
	/// added together.  The NumberOfSeriesRelatedInstances from all the DicomSeries in an order are also added as a different 
	/// sum.  The ImageAvailability of all procedures in the order are updated based on the comparison of these two sums.
	/// </summary>
	public class DefaultImageAvailabilityUpdateStrategy : IImageAvailabilityUpdateStrategy
	{
		private const string ProcedureOIDKey = "ProcedureOID";
        private readonly ImageAvailabilityShredSettings _settings;

        public DefaultImageAvailabilityUpdateStrategy()
        {
            _settings = new ImageAvailabilityShredSettings();
        }

		#region IImageAvailabilityUpdateStrategy Members

		public string ScheduledWorkQueueItemType
		{
			get { return "Image Availability"; }
		}

		public WorkQueueItem ScheduleWorkQueueItem(Procedure p, IPersistenceContext context)
		{
			WorkQueueItem item = new WorkQueueItem(this.ScheduledWorkQueueItemType);
            item.ExpirationTime = DateTime.Now.AddHours(_settings.ExpirationTimeInHours);
			item.ExtendedProperties.Add(ProcedureOIDKey, p.GetRef().Serialize());
			context.Lock(item, DirtyState.New);

			return item;
		}

		public void Update(WorkQueueItem item, IPersistenceContext context)
		{
			EntityRef procedureRef = new EntityRef(item.ExtendedProperties[ProcedureOIDKey]);
			Procedure procedure = context.Load<Procedure>(procedureRef, EntityLoadFlags.Proxy);

			// Find the number of instances recorded in the DicomSeries
			bool hasIncompleteDicomSeries;
			int numberOfInstancesFromDocumentation = QueryDocumentation(procedure.Order, out hasIncompleteDicomSeries);

			if (hasIncompleteDicomSeries)
			{
                procedure.ImageAvailability = Healthcare.ImageAvailability.N;
			}
			else
			{
				bool studiesNotFound;
				int numberOfInstancesFromDicomServer;

				numberOfInstancesFromDicomServer = QueryDicomServer(procedure.Order,
                    _settings.DicomCallingAETitle,
                    _settings.DicomServerAETitle,
                    _settings.DicomServerHost,
                    _settings.DicomServerPort,
					out studiesNotFound);

				// Compare recorded result with the result from Dicom Query 
				if (studiesNotFound || numberOfInstancesFromDicomServer == 0)
                    procedure.ImageAvailability = Healthcare.ImageAvailability.Z;
				else if (numberOfInstancesFromDicomServer < numberOfInstancesFromDocumentation)
                    procedure.ImageAvailability = Healthcare.ImageAvailability.P;
				else if (numberOfInstancesFromDicomServer == numberOfInstancesFromDocumentation)
                    procedure.ImageAvailability = Healthcare.ImageAvailability.C;
				else
                    procedure.ImageAvailability = Healthcare.ImageAvailability.N;
			}

			// update WorkQueueItem Status and the next ScheduledTime
			switch (procedure.ImageAvailability)
			{
				// ImageAvailability.X should never get pass into this method
				// case Healthcare.ImageAvailability.X:
				//     break;
                case Healthcare.ImageAvailability.N:
                    item.Reschedule(DateTime.Now.AddMinutes(_settings.NextScheduledTimeForUnknownAvailabilityInMinutes));
					break;
                case Healthcare.ImageAvailability.Z:
                    item.Reschedule(DateTime.Now.AddMinutes(_settings.NextScheduledTimeForZeroAvailabilityInMinutes));
					break;
                case Healthcare.ImageAvailability.P:
                    item.Reschedule(DateTime.Now.AddMinutes(_settings.NextScheduledTimeForPartialAvailabilityInMinutes));
					break;
                case Healthcare.ImageAvailability.C:
					item.Complete();
					break;
				default:
					break;
			}
		}

		#endregion

		private static int QueryDocumentation(Order order, out bool hasIncompleteDicomSeries)
		{
			List<DicomSeries> dicomSeries = new List<DicomSeries>();
			bool isMissingDicomSeries = false;

			// Find all the DicomSeries for this order
			CollectionUtils.ForEach(order.Procedures,
				delegate(Procedure procedure)
					{
						CollectionUtils.ForEach(procedure.ModalityProcedureSteps,
							delegate(ModalityProcedureStep mps)
							{
								List<PerformedStep> mppsList = CollectionUtils.Select(mps.PerformedSteps,
									delegate(PerformedStep ps) { return ps.Is<ModalityPerformedProcedureStep>(); });

								if (mppsList.Count == 0)
								{
									isMissingDicomSeries = true;
								}
								else
								{
									CollectionUtils.ForEach(mps.PerformedSteps,
										delegate(PerformedStep ps)
										{
											if (ps.Is<ModalityPerformedProcedureStep>())
											{
												ModalityPerformedProcedureStep mpps = ps.As<ModalityPerformedProcedureStep>();
												if (mpps.DicomSeries == null || mpps.DicomSeries.Count == 0)
													isMissingDicomSeries = true;
												else
													dicomSeries.AddRange(mpps.DicomSeries);
											}
										});
								}
							});
					});

			// Sum the number of instances for all DicomSeries
			hasIncompleteDicomSeries = isMissingDicomSeries;
			int numberOfInstancesFromDocumentation = CollectionUtils.Reduce<DicomSeries, int>(
				dicomSeries, 0,
				delegate(DicomSeries series, int totalInstances)
					{
						return totalInstances + series.NumberOfSeriesRelatedInstances;
					});

			return numberOfInstancesFromDocumentation;
		}

		private static int QueryDicomServer(Order order, 
			string shredAETitle,
			string dicomServerAETitle,
			string dicomServerHost,
			int dicomServerPort,
			out bool studiesNotFound)
		{
			DicomAttributeCollection requestCollection = new DicomAttributeCollection();
			requestCollection[DicomTags.QueryRetrieveLevel].SetStringValue("STUDY");
			requestCollection[DicomTags.StudyInstanceUid].SetStringValue("");
			requestCollection[DicomTags.AccessionNumber].SetStringValue(order.AccessionNumber);
			requestCollection[DicomTags.NumberOfStudyRelatedInstances].SetStringValue("");

			int numberOfInstancesFromDicomServer = 0;
			using (StudyRootFindScu scu = new StudyRootFindScu())
			{
				IList<DicomAttributeCollection> results = scu.Find(
					shredAETitle,
					dicomServerAETitle,
					dicomServerHost,
					dicomServerPort,
					requestCollection);

				// Wait for a response
				scu.Join(new TimeSpan(0, 0, 0, 0, 1000));

				if (scu.Status == ScuOperationStatus.Canceled)
				{
					String message = String.Format(SR.MessageFormatDicomRemoteServerCancelledFind,
					                               scu.FailureDescription ?? "no failure description provided");
					throw new DicomException(message);
				}
				if (scu.Status == ScuOperationStatus.ConnectFailed)
				{
					String message = String.Format(SR.MessageFormatDicomConnectionFailed,
					                               scu.FailureDescription ?? "no failure description provided");
					throw new DicomException(message);
				}
				if (scu.Status == ScuOperationStatus.Failed)
				{
					String message = String.Format(SR.MessageFormatDicomQueryOperationFailed,
					                               scu.FailureDescription ?? "no failure description provided");
					throw new DicomException(message);
				}
				if (scu.Status == ScuOperationStatus.TimeoutExpired)
				{
					String message = String.Format(SR.MessageFormatDicomConnectTimeoutExpired,
					                               scu.FailureDescription ?? "no failure description provided");
					throw new DicomException(message);
				}

				foreach (DicomAttributeCollection result in results)
				{
					numberOfInstancesFromDicomServer += (int) result[DicomTags.NumberOfStudyRelatedInstances].GetUInt32(0, 0);
				}

				studiesNotFound = results.Count == 0;
			}

			return numberOfInstancesFromDicomServer;
		}
	}
}
