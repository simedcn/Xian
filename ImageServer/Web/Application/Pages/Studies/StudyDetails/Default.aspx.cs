#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Application.Controls;
using ClearCanvas.ImageServer.Web.Application.Helpers;
using ClearCanvas.ImageServer.Web.Application.Pages.Common;
using ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls;
using ClearCanvas.ImageServer.Web.Common.Data;
using ClearCanvas.ImageServer.Web.Common.Utilities;
using ClearCanvas.ImageServer.Web.Common.Exceptions;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails
{
    /// <summary>
    /// Study details page.
    /// </summary>
    public partial class Default : BasePage
    {
        #region constants
        private const string QUERY_KEY_STUDY_INSTANCE_UID = "siuid";
        private const string QUERY_KEY_SERVER_AE = "serverae";
        #endregion constants

        #region Private members
        private string _studyInstanceUid;
        private string _serverae;
        private ServerPartition _partition;
        private StudySummary _study;
        
        #endregion Private members

        #region Public properties

        public ServerPartition Partition
        {
            get { return _partition; }
            set { _partition = value; }
        }


        #endregion Public properties

        #region Protected Methods

        protected void SetupEventHandlers()
        {
            StudyDetailsPanel.EditStudyClicked += StudyDetailsPanel_EditStudyClicked;
            StudyDetailsPanel.DeleteStudyClicked += StudyDetailsPanel_DeleteStudyClicked;
            EditStudyDialog.StudyEdited += EditStudyDialog_StudyEdited;
            DeleteConfirmDialog.Confirmed += DeleteConfirmDialog_Confirmed;
            DeleteStudyConfirmDialog.StudyDeleted += DeleteStudyConfirmDialog_StudyDeleted;
        }

        void StudyDetailsPanel_DeleteStudyClicked(object sender, ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.StudyDetailsPanelDeleteStudyClickEventArgs e)
        {
            DeleteStudy();
        }

        void StudyDetailsPanel_EditStudyClicked(object sender, ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.StudyDetailsPanelEditStudyClickEventArgs e)
        {
            EditStudy();
        }

        void DeleteStudyConfirmDialog_StudyDeleting(object sender, ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.DeleteStudyConfirmDialogStudyDeletingEventArgs e)
        {
            
        }

        void DeleteStudyConfirmDialog_StudyDeleted(object sender, ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls.DeleteStudyConfirmDialogStudyDeletedEventArgs e)
        {
            Refresh();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _studyInstanceUid = Request.QueryString[QUERY_KEY_STUDY_INSTANCE_UID];
            _serverae = Request.QueryString[QUERY_KEY_SERVER_AE];

            if (!String.IsNullOrEmpty(_studyInstanceUid) && !String.IsNullOrEmpty(_serverae)) 
            {
                
                ServerPartitionDataAdapter adaptor = new ServerPartitionDataAdapter();
                ServerPartitionSelectCriteria criteria = new ServerPartitionSelectCriteria();
                criteria.AeTitle.EqualTo(Request.QueryString[QUERY_KEY_SERVER_AE]);
                IList<ServerPartition> partitions = adaptor.Get(criteria);
                if (partitions != null && partitions.Count>0)
                {
                    if (partitions.Count==1)
                    {
                        Partition = partitions[0];

                        //TODO: Do something if parition is inactive. Perhapse show an alert on the screen?

                        LoadStudy();   
                    }
                    else
                    {
                        Response.Write("Unexpected Error: Multiple Partitions exist with AE title : " + _serverae);
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            SetupEventHandlers();

            base.OnInit(e);
        }

        protected void LoadStudy()
        {
            if (String.IsNullOrEmpty(_studyInstanceUid))
                return;

            if (_partition == null)
                return;

            StudyAdaptor studyAdaptor = new StudyAdaptor();
            StudySelectCriteria criteria = new StudySelectCriteria();
            criteria.StudyInstanceUid.EqualTo(_studyInstanceUid);
            criteria.ServerPartitionKey.EqualTo(Partition.GetKey());
			Study study = studyAdaptor.GetFirst(criteria);

			if (study != null)
                _study = StudySummaryAssembler.CreateStudySummary(study);
            else
			{
			    StudyNotFoundException exception = new StudyNotFoundException(_studyInstanceUid, "The Study is null in Default.aspx -> LoadStudy()");		        
			    ExceptionHandler.ThrowException(exception);
			}

            StudyDetailsPanel.Study = _study;
            StudyDetailsPanel.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e); 
            
            if (_partition!=null && _study == null)
            {
                StudyNotFoundException exception = new StudyNotFoundException(_studyInstanceUid, "The Study is null in Default.aspx -> OnPreRender()");
                ExceptionHandler.ThrowException(exception);
            } 
            if (_partition == null)
            {
                PartitionNotFoundException exception = new PartitionNotFoundException(_serverae, "The Server Partition is null in Default.aspx -> OnPreRender()");
                ExceptionHandler.ThrowException(exception);
            }

            if (_study==null)
            {
                StudyNotFoundException exception = new StudyNotFoundException(_studyInstanceUid, "The Study is null in Default.aspx -> OnPreRender()");
                ExceptionHandler.ThrowException(exception);
            }
            else
            {
                Page.Title = String.Format("{0}:{1}", NameFormatter.Format(_study.PatientsName) , _study.PatientId);
            }

        }


        #endregion Protected Methods

        #region Public Methods

        
        public void EditStudy()
        {
            if (_study!=null)
            {
                string reason;
                if (!_study.CanScheduleEdit(out reason))
                {
                    MessageDialog.MessageType = MessageBox.MessageTypeEnum.ERROR;
                    MessageDialog.Message = reason;
                    MessageDialog.Show();
                }
                else
                {
                    EditStudyDialog.study = _study.TheStudy;
                    EditStudyDialog.Show(true);
                }
            }
            
        }

        public void DeleteStudy()
        {
            string reason;
            if (!_study.CanScheduleEdit(out reason))
            {
                MessageDialog.MessageType = MessageBox.MessageTypeEnum.ERROR;
                MessageDialog.Message = reason;
                MessageDialog.Show();
            }
            else
            {
                //DeleteConfirmDialog.MessageType = MessageBox.MessageTypeEnum.YESNO;
                //DeleteConfirmDialog.Message = App_GlobalResources.SR.SingleStudyDelete;
                //DeleteConfirmDialog.Data = _study.TheStudy;

                //DeleteConfirmDialog.Show();
                List<StudySummary> studyList = new List<StudySummary>();
                studyList.Add(_study);

                DeleteStudyConfirmDialog.DeletingStudies = CollectionUtils.Map<StudySummary, DeleteStudyInfo>(
                    studyList,
                    delegate(StudySummary study)
                    {
                        DeleteStudyInfo info = new DeleteStudyInfo();
                        info.StudyKey = study.TheStudy.GetKey();
                        info.AccessionNumber = study.AccessionNumber;
                        info.Modalities = study.ModalitiesInStudy;
                        info.PatientId = study.PatientId;
                        info.PatientsName = study.PatientsName;
                        info.StudyDate = study.StudyDate;
                        info.StudyDescription = study.StudyDescription;
                        info.StudyInstanceUid = study.StudyInstanceUid;
                        return info;
                    }
                );

                DeleteStudyConfirmDialog.Show();
                updatepanel.Update();
            }
        }


        void DeleteConfirmDialog_Confirmed(object data)
        {
            try
            {
                StudyController controller = new StudyController();
                Study study = DeleteConfirmDialog.Data as Study;
                controller.DeleteStudy(study.GetKey(), "hey");
            }
            finally
            {
                Refresh();
            }
        }


        void EditStudyDialog_StudyEdited()
        {
            Refresh();
        }

        #endregion



        public void Refresh()
        {
            LoadStudy();
        }
    }
}
