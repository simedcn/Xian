#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Common.Audit;
using ClearCanvas.Dicom.Audit;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common.ServiceModel;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;

namespace ClearCanvas.ImageServer.Common
{
	/// <summary>
	/// A collection of useful ImageServer utility functions.
	/// </summary>
    static public class ServerPlatform
    {
        #region Private Fields
        private static string _dbVersion;
        private static string _tempDir;
        private static readonly object _syncLock = new object();
    	private static DicomAuditSource _auditSource;
    	private static AuditLog _log;
    	private static string _hostId;
    	private static string _serverInstanceId;
    	private static string _processorId;

        private static bool? _manifestVerified;
    	#endregion

        /// <summary>
        /// Generates an alert message with an expiration time.
        /// </summary>
        /// <param name="category">An alert category</param>
        /// <param name="level">Alert level</param>
        /// <param name="source">Name of the source where the alert is raised</param>
        /// <param name="alertCode">Alert type code</param>
        /// <param name="contextData">The user-defined application context data</param>
        /// <param name="expirationTime">Expiration time for the alert</param>
        /// <param name="message">The alert message or formatted message.</param>
        /// <param name="args">Paramaters used in the alert message, when specified.</param>
        public static void Alert(AlertCategory category, AlertLevel level, String source, 
                                    int alertCode, object contextData, TimeSpan expirationTime, 
                                    String message, params object[] args)
        {
            Platform.CheckForNullReference(source, "source");
            Platform.CheckForNullReference(message, "message");
            IAlertService service = Platform.GetService<IAlertService>();
            if (service != null)
            {
                AlertSource src = new AlertSource(source) {Host = ServerInstanceId};
            	Alert alert = new Alert
                              	{
                              		Category = category,
                              		Level = level,
                              		Code = alertCode,
                              		ExpirationTime = Platform.Time.Add(expirationTime),
                              		Source = src,
                              		Message = String.Format(message, args),
                              		ContextData = contextData
                              	};

            	service.GenerateAlert(alert);
            }
        }

		/// <summary>
		/// A well known AuditSource for ImageServer audit logging.
		/// </summary>
    	public static DicomAuditSource AuditSource
    	{
    		get
    		{
    			lock (_syncLock)
    			{
    				if (_auditSource == null)
    				{
    					_auditSource = new DicomAuditSource("ImageServer");
    				}
    				return _auditSource;
    			}
    		}
    	}

		/// <summary>
		/// Returns the duration of the user session based on the application settings
		/// </summary>	
	    public static TimeSpan WebSessionTimeout
	    {
            get
            {
                int timeout;
                if (Int32.TryParse(ConfigurationManager.AppSettings["SessionTimeout"], out timeout))
                {
                    return TimeSpan.FromMinutes(timeout);
                }
                else
                {
                    return TimeSpan.FromMinutes(60);
                }
            }
	    }

		/// <summary>
		/// Log an Audit message.
		/// </summary>
		/// <param name="helper"></param>
		public static void LogAuditMessage(DicomAuditHelper helper)
		{
			lock (_syncLock)
			{
				if (_log == null)
					_log = new AuditLog("ImageServer","DICOM");

                string serializeText = null;
                try
                {
                    serializeText = helper.Serialize(false);
                    _log.WriteEntry(helper.Operation, serializeText);
                }
                catch(Exception ex)
                {
                    Platform.Log(LogLevel.Error, ex, "Error occurred when writing audit log");

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Audit Log failed to save:");
                    sb.AppendLine(String.Format("Operation: {0}", helper.Operation));
                    sb.AppendLine(String.Format("Details: {0}", serializeText));
                    Platform.Log(LogLevel.Info, sb.ToString());
                }
			}
		}

		/// <summary>
		/// The Reconcile folder.    DO NOT CHANGE!
		/// </summary>
		public const string ReconcileStorageFolder = "Reconcile";

		/// <summary>
		/// The default DICOM file extension.  DO NOT CHANGE!
		/// </summary>
		public const string DicomFileExtension = ".dcm";

		/// <summary>
		/// The default Duplicate DICOM file extension.  
		/// </summary>
		/// <remarks>
		/// Note, due to historical reasons, this value does not have a "." in the
		/// duplicate value.  The extensions is input in the WorkQueueUid table without the period.
		/// </remarks>
		public const string DuplicateFileExtension = "dup";

        /// <summary>
        /// Gets the path to the temporary folder.
        /// </summary>
        public static String TempDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(_tempDir) || !Directory.Exists(_tempDir))
                {
                    lock(_syncLock)
                    {
                        // if specified in the config, use it
                        if (!String.IsNullOrEmpty(ImageServerCommonConfiguration.TemporaryPath))
                        {
                            _tempDir = ImageServerCommonConfiguration.TemporaryPath;
                        }
                        else
                        {
                            // Use the OS temp folder instead, assuming it's not too long.
                            // Avoid creating a temp folder under the installation directory because it could
                            // lead to PathTooLongException.
                            _tempDir = Path.Combine(Path.GetTempPath(), "ImageServer");
                        }

                        // make sure it exists
                        if (!Directory.Exists(_tempDir))
                        {
                            Directory.CreateDirectory(_tempDir);
                        }
                    }
                }

                return _tempDir;
            }
        }

        /// <summary>
        /// Returns the version info in the database.
        /// </summary>
        public static String VersionString
        {
            get
            {
                lock (_syncLock)
                {
                    if (String.IsNullOrEmpty(_dbVersion))
                    {
                        try
                        {
                            IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();
                            using (IReadContext ctx = store.OpenReadContext())
                            {
                                IDatabaseVersionEntityBroker broker = ctx.GetBroker<IDatabaseVersionEntityBroker>();
                                DatabaseVersion version = broker.FindOne(new DatabaseVersionSelectCriteria());
								_dbVersion = version != null ? version.GetVersionString() : String.Empty;
                            }
                        }
                        catch (Exception ex)
                        {
                            Platform.Log(LogLevel.Error, ex);
                        }
                    }
                }

                return _dbVersion;
            }
        }

		/// <summary>
		/// Flag telling if instance level logging is enabled.
		/// </summary>
    	public static LogLevel InstanceLogLevel
    	{
			get { return Settings.Default.InstanceLogging ? LogLevel.Info : LogLevel.Debug; }
    	}

    	/// <summary>
    	/// Returns a string that can be used to identify the host machine where the server is running
    	/// </summary>
    	public static string HostId
    	{
    		get
    		{
    			if (String.IsNullOrEmpty(_hostId))
    			{
    				String strHostName = Dns.GetHostName();
    				if (String.IsNullOrEmpty(strHostName) == false)
    					_hostId = strHostName;
    				else
    				{
    					// Find host by name
    					IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

    					// Enumerate IP addresses, pick an IPv4 address first
    					foreach (IPAddress ipaddress in iphostentry.AddressList)
    					{
    						if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    						{
    							_hostId = ipaddress.ToString();
    							break;
    						}
    					}
    				}
    			}

    			return _hostId;
    		}
    	}

		/// <summary>
		/// Server Instance Id
		/// </summary>
    	public static string ServerInstanceId
    	{
    		get
    		{
    			if (String.IsNullOrEmpty(_serverInstanceId))
    			{
    				_serverInstanceId = String.Format("Host={0}/Pid={1}", HostId, Process.GetCurrentProcess().Id);
    			}

    			return _serverInstanceId;
    		}
    	}

    	/// <summary>
    	/// A string representing the ID of the work queue processor.
    	/// </summary>
    	/// <remarks>
    	/// <para>
    	/// This ID is used to reset the work queue items.
    	/// </para>
    	/// <para>
    	/// For the time being, the machine ID is tied to the IP address. Assumimg the server
    	/// will be installed on a machine with DHCP disabled or if the DNS server always assign
    	/// the same IP for the machine, this will work fine.
    	/// </para>
    	/// <para>
    	/// Because of this implemenation, all instances of WorkQueueProcessor will have the same ID.
    	/// </para>
    	/// </remarks>
    	public static string ProcessorId
    	{
    		get
    		{
    			if (_processorId == null)
    			{
    				try
    				{
    					String strHostName = Dns.GetHostName();

    					// Find host by name
    					IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

    					// Enumerate IP addresses, pick an IPv4 address first
    					foreach (IPAddress ipaddress in iphostentry.AddressList)
    					{
    						if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    						{
    							_processorId = ipaddress.ToString();
    							break;
    						}
    					}
    					if (_processorId == null)
    					{
    						foreach (IPAddress ipaddress in iphostentry.AddressList)
    						{
    							_processorId = ipaddress.ToString();
    							break;
    						}
    					}
    				}
    				catch (Exception e)
    				{
    					Platform.Log(LogLevel.Error, e, "Cannot resolve hostname into IP address");
    				}
    			}

    			if (_processorId == null)
    			{
    				Platform.Log(LogLevel.Warn, "Could not determine hostname or IP address of the local machine. Work Queue Processor ID is set to Unknown");
    				_processorId = "Unknown";

    			}

    			return _processorId;
    		}
    	}


	    public static bool IsManifestVerified
	    {
	        get
	        {
                lock (_syncLock)
                {
                    if (_manifestVerified==null)
                    {
                        lock (_syncLock)
                        {
                            try
                            {
                                Platform.GetService(delegate(IProductVerificationService service)
                                                        {
                                                            var result = service.Verify(new ProductVerificationRequest());
                                                            _manifestVerified = result.IsManifestValid;
                                                        }
                                    );
                            }
                            catch(Exception ex)
                            {
                                Platform.Log(LogLevel.Error, ex, "Unable to verify shred host service manifest");
                            }
                        }
                    }
                }

                return _manifestVerified.HasValue ? _manifestVerified.Value : false;
	        }
	    }

	    public static StudyHistory CreateStudyHistoryRecord(IUpdateContext updateContext,
			StudyStorageLocation primaryStudyLocation, StudyStorageLocation secondaryStudyLocation,
			StudyHistoryTypeEnum type, object entryInfo, object changeLog)
		{
			StudyHistoryUpdateColumns columns = new StudyHistoryUpdateColumns
			                                    	{
			                                    		InsertTime = Platform.Time,
			                                    		StudyHistoryTypeEnum = type,
			                                    		StudyStorageKey = primaryStudyLocation.GetKey(),
			                                    		DestStudyStorageKey =
			                                    			secondaryStudyLocation != null
			                                    				? secondaryStudyLocation.GetKey()
			                                    				: primaryStudyLocation.GetKey(),
			                                    		StudyData = XmlUtils.SerializeAsXmlDoc(entryInfo) ?? new XmlDocument(),
			                                    		ChangeDescription = XmlUtils.SerializeAsXmlDoc(changeLog) ?? new XmlDocument()
			                                    	};

			IStudyHistoryEntityBroker broker = updateContext.GetBroker<IStudyHistoryEntityBroker>();
			return broker.Insert(columns);
		}

        /// <summary>
        /// Returns a boolean indicating whether the entry is still "active"
        /// </summary>
        /// <remarks>
        /// </remarks>
        static public bool IsActiveWorkQueue(WorkQueue item)
        {
        	// The following code assumes InactiveWorkQueueMinTime is set appropirately
        	
            if (item.WorkQueueStatusEnum.Equals(WorkQueueStatusEnum.Failed))
                return false;

            if (item.WorkQueueStatusEnum.Equals(WorkQueueStatusEnum.Pending) || 
                item.WorkQueueStatusEnum.Equals(WorkQueueStatusEnum.Idle))
            {
                // Assuming that if the entry is picked up and rescheduled recently (the ScheduledTime would have been updated), 
                // the item is inactive if its ScheduledTime still indicated it was scheduled long time ago.
                // Note: this logic still works if the entry has never been processed (new). It will be
                // considered as "inactive" if it was scheduled long time ago and had never been updated.

                DateTime time = item.LastUpdatedTime!=DateTime.MinValue? item.LastUpdatedTime:item.ScheduledTime;
                if (time < Platform.Time - Settings.Default.InactiveWorkQueueMinTime)
                    return false;
            }
            else if (item.WorkQueueStatusEnum.Equals(WorkQueueStatusEnum.InProgress))
            {
                if (String.IsNullOrEmpty(item.ProcessorID))
                {
                    // This is a special case, the item is not assigned but is set to InProgress. 
                    // It's definitely stuck cause it won't be picked up by any servers.
                    return false; 
                }
            	// TODO: Need more elaborate logic to detect if it's stuck when the status is InProgress.
            	// Ideally, we can assume item is stuck if it has not been updated for a while. 
            	// Howerver, some operations were designed to process everything in a single run 
            	// instead of batches.One example is the StudyProcess, research studies may take days to process 
            	// and the item stays in "InProgress" for the entire period without any update 
            	// (eventhough the WorkQueueUid records are removed)
            	// For now, we assume it's stucked if it is not updated for long time.
            	if (item.ScheduledTime < Platform.Time - Settings.Default.InactiveWorkQueueMinTime)
            		return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method to return the path to the duplicate image (in the Reconcile folder)
        /// </summary>
        /// <param name="studyStorage"></param>
        /// <param name="sop"></param>
        /// <returns></returns>
        public static String GetDuplicateUidPath(StudyStorageLocation studyStorage, WorkQueueUid sop)
        {
            string dupPath = GetDuplicateGroupPath(studyStorage, sop);
            dupPath = string.IsNullOrEmpty(sop.RelativePath)
                        ? Path.Combine(dupPath,
                                       Path.Combine(studyStorage.StudyInstanceUid, sop.SopInstanceUid + "." + sop.Extension))
                        : Path.Combine(dupPath, sop.RelativePath);

            #region BACKWARD_COMPATIBILTY_CODE

            if (string.IsNullOrEmpty(sop.RelativePath) && !File.Exists(dupPath))
            {
                string basePath = Path.Combine(studyStorage.GetStudyPath(), sop.SeriesInstanceUid);
                basePath = Path.Combine(basePath, sop.SopInstanceUid);
                if (sop.Extension != null)
                    dupPath = basePath + "." + sop.Extension;
                else
                    dupPath = basePath + ".dcm";
            }

            #endregion

            return dupPath;
        }

        /// <summary>
        /// Helper method to return the path to the folder containing the duplicate images (in the Reconcile folder)
        /// </summary>
        /// <param name="studyStorage"></param>
        /// <param name="sop"></param>
        /// <returns></returns>
        public static String GetDuplicateGroupPath(StudyStorageLocation studyStorage, WorkQueueUid sop)
        {
            String groupFolderPath = Path.Combine(studyStorage.FilesystemPath, studyStorage.PartitionFolder);
            groupFolderPath = Path.Combine(groupFolderPath, ServerPlatform.ReconcileStorageFolder);
            groupFolderPath = Path.Combine(groupFolderPath, sop.GroupID);

            return groupFolderPath;
        }

        /// <summary>
        /// Helper method to return the path to the folder containing the duplicate images (in the Reconcile folder)
        /// </summary>
        /// <param name="storageLocation"></param>
        /// <param name="queueItem"></param>
        /// <returns></returns>
        public static string GetDuplicateGroupPath(StudyStorageLocation storageLocation, WorkQueue queueItem)
	    {
            string path = Path.Combine(storageLocation.FilesystemPath, storageLocation.PartitionFolder);
            path = Path.Combine(path, ServerPlatform.ReconcileStorageFolder);
            path = Path.Combine(path, queueItem.GroupID);
            return path;
	    }
    }
}
