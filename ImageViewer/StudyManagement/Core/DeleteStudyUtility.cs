﻿#region License

// Copyright (c) 2012, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageViewer.Common.WorkItem;
using ClearCanvas.ImageViewer.StudyManagement.Core.Storage;

namespace ClearCanvas.ImageViewer.StudyManagement.Core
{
    /// <summary>
    /// Utility class for deleting a study.
    /// </summary>
    public class DeleteStudyUtility
    {
        private StudyLocation _location;

        public int NumberOfStudyRelatedInstances { get; set; }

        public void Initialize(StudyLocation location)
        {
            _location = location;

            using (var context = new DataAccessContext(DataAccessContext.WorkItemMutex))
            {
                var studyBroker = context.GetStudyBroker();
                var study = studyBroker.GetStudy(_location.Study.StudyInstanceUid);
                if (study != null)
                {
                    _location.Study = study;
                    if (study.NumberOfStudyRelatedInstances.HasValue)
                        NumberOfStudyRelatedInstances = study.NumberOfStudyRelatedInstances.Value;
                }
            }
        }        

        public bool Process()
        {
            // Decided not to use the command processor here, since we're just removing everything and want to be as forgiving as possible.
            try
            {
                DirectoryUtility.DeleteIfExists(_location.StudyFolder);
            }
            catch (Exception e)
            {
                Platform.Log(LogLevel.Error, e, "Unable to delete study folder: {0}", _location.StudyFolder);
            }

            try
            {
                using (var context = new DataAccessContext(DataAccessContext.WorkItemMutex))
                {
                    var studyBroker = context.GetStudyBroker();
                    var study = studyBroker.GetStudy(_location.Study.StudyInstanceUid);
                    if (study != null)
                    {
                        studyBroker.Delete(study);
                    }

                    context.Commit();
                }

                Platform.Log(LogLevel.Info, "Deleted study for: {0}:{1}", _location.Study.PatientsName, _location.Study.PatientId);
                return true;
            }

            catch (Exception e)
            {
                Platform.Log(LogLevel.Error, e,
                             "Unexpected exception when {0} deleting Study related database entries for study: {0}",
                             _location.Study.StudyInstanceUid);
       
                return false;
            }
        }
    }
}
