#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Helpers;
using ClearCanvas.ImageServer.Core.Edit;
using ClearCanvas.ImageServer.Core.Process;
using ClearCanvas.ImageServer.Core.Reconcile;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Rules;

namespace ClearCanvas.ImageServer.Core
{

	/// <summary>
	/// Encapsulates the context of the 'StudyProcess' operation.
	/// </summary>
	public class StudyProcessorContext
	{
		#region Private Members

	    private readonly object _syncLock = new object();
        private readonly StudyStorageLocation _storageLocation;
		private Study _study;
		private ServerRulesEngine _sopProcessedRulesEngine;
		private ServerRulesEngine _sopCompressionRulesEngine;
		private List<BaseImageLevelUpdateCommand> _updateCommands;
	    #endregion

		#region Constructors

		public StudyProcessorContext(StudyStorageLocation storageLocation)
		{
		    Platform.CheckForNullReference(storageLocation, "storageLocation");
		    _storageLocation = storageLocation;
		}

		#endregion

		#region Public Properties

        /// <summary>
        /// Gets
        /// </summary>
        internal ServerPartition Partition
		{
            get { return _storageLocation.ServerPartition; }
		}

		internal Study Study
		{
            get
            {
                if (_study==null)
                {
                    lock (_syncLock)
                    {
                        _study = _storageLocation.LoadStudy(ExecutionContext.Current.PersistenceContext);    
                    }
                    
                }
                return _study;
            }

		}

	    public ServerRulesEngine SopProcessedRulesEngine
		{
			get
			{
				lock (_syncLock)
				{
					if (_sopProcessedRulesEngine == null)
					{
						_sopProcessedRulesEngine =
							new ServerRulesEngine(ServerRuleApplyTimeEnum.SopProcessed, Partition.GetKey());
						_sopProcessedRulesEngine.AddOmittedType(ServerRuleTypeEnum.SopCompress);
						_sopProcessedRulesEngine.Load();
					}
				}
				return _sopProcessedRulesEngine;
			}
			set
			{
                lock (_syncLock)
                {
                    _sopProcessedRulesEngine = value;
                }
			}
		}

		public ServerRulesEngine SopCompressionRulesEngine
		{
			get
			{
				lock (_syncLock)
				{
					if (_sopCompressionRulesEngine == null)
					{
						_sopCompressionRulesEngine =
							new ServerRulesEngine(ServerRuleApplyTimeEnum.SopProcessed, Partition.GetKey());
						_sopCompressionRulesEngine.AddIncludeType(ServerRuleTypeEnum.SopCompress);
						_sopCompressionRulesEngine.Load();
					}
				}
				return _sopCompressionRulesEngine;
			}
			set
			{
				lock (_syncLock)
				{
					_sopCompressionRulesEngine = value;
				}
			}
		}

	    public StudyStorageLocation StorageLocation
	    {
	        get { return _storageLocation; }
	    }

		public List<BaseImageLevelUpdateCommand> UpdateCommands
		{
			get
			{
				lock (_syncLock)
				{
					if (_updateCommands == null)
					{
						_updateCommands = new List<BaseImageLevelUpdateCommand>();
					}
					return _updateCommands;
				}
			}
		}
	    #endregion
	}

    /// <summary>
    /// Represents an event that occurs when a sop instance is being inserted into the study by the <see cref="SopInstanceProcessor"/>
    /// </summary>
    public class SopInsertingEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="ServerCommandProcessor"/> used by the <see cref="SopInstanceProcessor"/> to insert the sop instance into the study.
        /// </summary>
        public ServerCommandProcessor Processor { get; set; }
    }

	    

	/// <summary>
	/// Processor for Sop Instances being inserted into the database.
	/// </summary>
	public class SopInstanceProcessor
    {
        
        #region Private Members

        private readonly StudyProcessorContext _context;
		private readonly InstanceStatistics _instanceStats = new InstanceStatistics();
		private string _modality;
	    private readonly PatientNameRules _patientNameRules;

	    #endregion

		#region Constructors

		public SopInstanceProcessor(StudyProcessorContext context)
		{
            Platform.CheckForNullReference(context, "context");
		    _context = context;
		    _patientNameRules = new PatientNameRules(context.Study);
		}

		#endregion

		#region Public Properties

        /// <summary>
        /// Gets or sets a value to indicate whether to apply the Patient's Name rules when processing a Dicom object.
        /// </summary>
        public bool EnforceNameRules { get; set; }

		public string Modality
		{
			get { return _modality; }
		}

		public InstanceStatistics InstanceStats
		{
			get { return _instanceStats; }
		}

		#endregion

        #region Events
        /// <summary>
        /// Occurs when the SOP instance is about to be added to the study.
        /// </summary>
        public event EventHandler<SopInsertingEventArgs> OnInsertingSop;
        #endregion


		#region Public Methods



		/// <summary>
		/// Process a specific DICOM file related to a <see cref="WorkQueue"/> request.
		/// </summary>
		/// <param name="stream">The <see cref="StudyXml"/> file to update with information from the file.</param>
		/// <param name="group"></param>
		/// <param name="file"></param>
		/// <param name="compare"></param>
		/// <param name="uid">An optional WorkQueueUid associated with the entry, that will be deleted upon success.</param>
		/// <param name="deleteFile">An option file to delete as part of the process</param>
        public  ProcessingResult ProcessFile(string group, DicomFile file, StudyXml stream, bool compare, WorkQueueUid uid, string deleteFile)
		{
		    Platform.CheckForNullReference(file, "file");


            _instanceStats.ProcessTime.Start();
            ProcessingResult result = new ProcessingResult {Status = ProcessingStatus.Failed};

		    using (ServerCommandProcessor processor = new ServerCommandProcessor("Process File"))
            {
                SopProcessingContext processingContext = new SopProcessingContext(processor, _context.StorageLocation, group);

                if (EnforceNameRules)
                {
                    _patientNameRules.Apply(file);
                }

                if (compare && ShouldReconcile(_context.StorageLocation, file))
                {
                    ScheduleReconcile(processingContext, file, uid);
                    result.Status = ProcessingStatus.Reconciled;
                }
                else
                {
                    InsertInstance(file, stream, uid, deleteFile);
                    result.Status = ProcessingStatus.Success;
                }
            }            

			_instanceStats.ProcessTime.End();

			if (_context.SopProcessedRulesEngine.Statistics.LoadTime.IsSet)
				_instanceStats.SopRulesLoadTime.Add(_context.SopProcessedRulesEngine.Statistics.LoadTime);

			if (_context.SopProcessedRulesEngine.Statistics.ExecutionTime.IsSet)
				_instanceStats.SopEngineExecutionTime.Add(_context.SopProcessedRulesEngine.Statistics.ExecutionTime);

			_context.SopProcessedRulesEngine.Statistics.Reset();

            //TODO: Should throw exception if result is failed?
		    return result;
		}

        

	    #endregion

		#region Private Methods

       

		/// <summary>
		/// Returns a value indicating whether the Dicom image must be reconciled.
		/// </summary>
		/// <param name="storageLocation"></param>
		/// <param name="message">The Dicom message</param>
		/// <returns></returns>
		private bool ShouldReconcile(StudyStorageLocation storageLocation, DicomMessageBase message)
		{
			Platform.CheckForNullReference(_context, "_context");
			Platform.CheckForNullReference(message, "message");

			if (_context.Study == null)
			{
				// the study doesn't exist in the database
				return false;
			}

		    StudyComparer comparer = new StudyComparer();
            DifferenceCollection list = comparer.Compare(message, storageLocation.Study, storageLocation.ServerPartition.GetComparisonOptions());

		    if (list != null && list.Count > 0)
		    {
		        LogDifferences(message, list);
		        return true;
		    }
		    return false;
		}

		private static void LogDifferences(DicomMessageBase message, DifferenceCollection list)
		{
			string sopInstanceUid = message.DataSet[DicomTags.SopInstanceUid].GetString(0, String.Empty);
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Found {0} issue(s) in SOP {1}\n", list.Count, sopInstanceUid);
			sb.Append(list.ToString());
			Platform.Log(LogLevel.Warn, sb.ToString());
		}

		/// <summary>
		/// Schedules a reconciliation for the specified <see cref="DicomFile"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="file"></param>
		/// <param name="uid"></param>
		private static void ScheduleReconcile(SopProcessingContext context, DicomFile file, WorkQueueUid uid)
		{
			ImageReconciler reconciler = new ImageReconciler(context);
			reconciler.ScheduleReconcile(file, StudyIntegrityReasonEnum.InconsistentData, uid);
		}

       
		private void InsertInstance(DicomFile file, StudyXml stream, WorkQueueUid uid, string deleteFile)
		{
			using (ServerCommandProcessor processor = new ServerCommandProcessor("Processing WorkQueue DICOM file"))
			{
			    EventsHelper.Fire(OnInsertingSop, this, new SopInsertingEventArgs {Processor = processor });

				InsertInstanceCommand insertInstanceCommand = null;
				InsertStudyXmlCommand insertStudyXmlCommand = null;

				String patientsName = file.DataSet[DicomTags.PatientsName].GetString(0, String.Empty);
				_modality = file.DataSet[DicomTags.Modality].GetString(0, String.Empty);

				if (_context.UpdateCommands.Count > 0)
				{
					foreach (BaseImageLevelUpdateCommand command in _context.UpdateCommands)
					{
						command.File = file;
						processor.AddCommand(command);
					}
				}

				try
				{
					// Create a context for applying actions from the rules engine
					ServerActionContext context =
						new ServerActionContext(file, _context.StorageLocation.FilesystemKey, _context.Partition, _context.StorageLocation.Key);
					context.CommandProcessor = processor;

					_context.SopCompressionRulesEngine.Execute(context);
                    String seriesUid = file.DataSet[DicomTags.SeriesInstanceUid].GetString(0, String.Empty);
                    String sopUid = file.DataSet[DicomTags.SopInstanceUid].GetString(0, String.Empty);
                    String finalDest = _context.StorageLocation.GetSopInstancePath(seriesUid, sopUid);

					if (_context.UpdateCommands.Count > 0)
					{
						processor.AddCommand(new SaveDicomFileCommand(_context.StorageLocation, file, file.Filename != finalDest));
					}
					else if (file.Filename != finalDest || processor.CommandCount > 0)
                    {
						// Have to be careful here about failure on exists vs. not failing on exists
						// because of the different use cases of the importer.
                        // save the file in the study folder, or if its been compressed
						processor.AddCommand(new SaveDicomFileCommand(finalDest, file, file.Filename != finalDest));
                    }

					// Update the StudyStream object
					insertStudyXmlCommand = new InsertStudyXmlCommand(file, stream, _context.StorageLocation);
					processor.AddCommand(insertStudyXmlCommand);

					// Have the rules applied during the command processor, and add the objects.
					processor.AddCommand(new ApplySopRulesCommand(context,_context.SopProcessedRulesEngine));

					// If specified, delete the file
					if (deleteFile != null)
						processor.AddCommand(new FileDeleteCommand(deleteFile, true));

					// Insert into the database, but only if its not a duplicate so the counts don't get off
					insertInstanceCommand = new InsertInstanceCommand(file, _context.StorageLocation);
					processor.AddCommand(insertInstanceCommand);
					
					// Do a check if the StudyStatus value should be changed in the StorageLocation.  This
					// should only occur if the object has been compressed in the previous steps.
					processor.AddCommand(new UpdateStudyStatusCommand(_context.StorageLocation, file));

					if (uid!=null)
						processor.AddCommand(new DeleteWorkQueueUidCommand(uid));

					// Do the actual processing
					if (!processor.Execute())
					{
						Platform.Log(LogLevel.Error, "Failure processing command {0} for SOP: {1}", processor.Description, file.MediaStorageSopInstanceUid);
						Platform.Log(LogLevel.Error, "File that failed processing: {0}", file.Filename);
						throw new ApplicationException("Unexpected failure (" + processor.FailureReason + ") executing command for SOP: " + file.MediaStorageSopInstanceUid, processor.FailureException);
					}
				    Platform.Log(ServerPlatform.InstanceLogLevel, "Processed SOP: {0} for Patient {1}", file.MediaStorageSopInstanceUid, patientsName);
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e, "Unexpected exception when {0}.  Rolling back operation.",
					             processor.Description);
					processor.Rollback();
					throw new ApplicationException("Unexpected exception when processing file.", e);
				}
				finally
				{
					if (insertInstanceCommand != null && insertInstanceCommand.Statistics.IsSet)
						_instanceStats.InsertDBTime.Add(insertInstanceCommand.Statistics);
					if (insertStudyXmlCommand != null && insertStudyXmlCommand.Statistics.IsSet)
						_instanceStats.InsertStreamTime.Add(insertStudyXmlCommand.Statistics);
				}
			}
		}
		#endregion
	}

    public class ProcessingResult
    {
        public ProcessingStatus Status { get; set; }
    }

    public enum ProcessingStatus
    {
        Success,
        Reconciled,
        Failed
    }
}