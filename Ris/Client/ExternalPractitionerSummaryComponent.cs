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
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.Admin.ExternalPractitionerAdmin;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client
{
    [MenuAction("launch", "global-menus/Admin/ExternalPractitioner", "Launch")]
    //[ActionPermission("launch", ClearCanvas.Ris.Application.Common.AuthorityTokens.ExternalPractitionerAdmin)]

    [ExtensionOf(typeof(DesktopToolExtensionPoint))]
    public class ExternalPractitionerSummaryTool : Tool<IDesktopToolContext>
    {
        private IWorkspace _workspace;

        public void Launch()
        {
            if (_workspace == null)
            {
                try
                {
                    ExternalPractitionerSummaryComponent component = new ExternalPractitionerSummaryComponent();

                    _workspace = ApplicationComponent.LaunchAsWorkspace(
                        this.Context.DesktopWindow,
                        component,
                        SR.TitleExternalPractitioner);
                    _workspace.Closed += delegate { _workspace = null; };

                }
                catch (Exception e)
                {
                    // failed to launch component
                    ExceptionHandler.Report(e, this.Context.DesktopWindow);
                }
            }
            else
            {
                _workspace.Activate();
            }
        }
    }

    /// <summary>
    /// Extension point for views onto <see cref="ExternalPractitionerSummaryComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class ExternalPractitionerSummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// ExternalPractitionerSummaryComponent class
    /// </summary>
    [AssociateView(typeof(ExternalPractitionerSummaryComponentViewExtensionPoint))]
    public class ExternalPractitionerSummaryComponent : ApplicationComponent
    {
        private ExternalPractitionerSummary _selectedPractitioner;
        private Table<ExternalPractitionerSummary> _practitioners;

        private SimpleActionModel _staffActionHandler;
        private readonly string _addPractitionerKey = "AddPractitioner";
        private readonly string _updatePractitionerKey = "UpdatePractitioner";

        private PagingController<ExternalPractitionerSummary> _pagingController;

        private ListExternalPractitionersRequest _listRequest;
        private string _firstName;
        private string _lastName;

        private bool _dialogMode;


        /// <summary>
        /// Constructor
        /// </summary>
        public ExternalPractitionerSummaryComponent()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dialogMode">Indicates whether the component will be shown in a dialog box or not</param>
        public ExternalPractitionerSummaryComponent(bool dialogMode)
        {
            _dialogMode = dialogMode;
        }

        public override void Start()
        {
            _practitioners = new Table<ExternalPractitionerSummary>();
            _practitioners.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnFamilyName,
               delegate(ExternalPractitionerSummary staff) { return staff.Name.FamilyName; },
               1.0f));

            _practitioners.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnGivenName,
                delegate(ExternalPractitionerSummary staff) { return staff.Name.GivenName; },
                1.0f));

            _practitioners.Columns.Add(new TableColumn<ExternalPractitionerSummary, string>(SR.ColumnLicenseNumber,
                delegate(ExternalPractitionerSummary staff) { return LicenseNumberFormat.Format(staff.LicenseNumber); },
                1.0f));

            _staffActionHandler = new SimpleActionModel(new ResourceResolver(this.GetType().Assembly));
            _staffActionHandler.AddAction(_addPractitionerKey, SR.TitleAddExternalPractitioner, "Icons.AddToolSmall.png", SR.TitleAddExternalPractitioner, AddPractitioner, ClearCanvas.Ris.Application.Common.AuthorityTokens.ExternalPractitionerAdmin);
            _staffActionHandler.AddAction(_updatePractitionerKey, SR.TitleUpdateExternalPractitioner, "Icons.EditToolSmall.png", SR.TitleUpdateExternalPractitioner, UpdateSelectedPractitioner, ClearCanvas.Ris.Application.Common.AuthorityTokens.ExternalPractitionerAdmin);

            InitialisePaging(_staffActionHandler);

            _listRequest = new ListExternalPractitionersRequest();

            // if the last name or first name properties are valued, generate an initial search
            if (!string.IsNullOrEmpty(_lastName) || !string.IsNullOrEmpty(_firstName))
            {
                // do not handle exceptions here - allow to propagate to caller
                DoSearch();
            }

            base.Start();
        }

        private void InitialisePaging(ActionModelNode actionModelNode)
        {
            _pagingController = new PagingController<ExternalPractitionerSummary>(
                delegate(int firstRow, int maxRows)
                {
                    ListExternalPractitionersResponse listResponse = null;

                    Platform.GetService<IExternalPractitionerAdminService>(
                        delegate(IExternalPractitionerAdminService service)
                        {
                            _listRequest.Page.FirstRow = firstRow;
                            _listRequest.Page.MaxRows = maxRows;

                            listResponse = service.ListExternalPractitioners(_listRequest);
                        });

                    return listResponse.Practitioners;
                }
            );

            if (actionModelNode != null)
            {
                actionModelNode.Merge(new PagingActionModel<ExternalPractitionerSummary>(_pagingController, _practitioners, Host.DesktopWindow));
            }
        }

        public override void Stop()
        {
            base.Stop();
        }

        #region Event Handler


        #endregion

        #region Presentation Model

        public bool ShowAcceptCancelButtons
        {
            get { return _dialogMode; }
            set { _dialogMode = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public ITable Practitioners
        {
            get { return _practitioners; }
        }

        public ActionModelNode PractitionersListActionModel
        {
            get { return _staffActionHandler; }
        }

        public ISelection SelectedPractitioner
        {
            get { return _selectedPractitioner == null ? Selection.Empty : new Selection(_selectedPractitioner); }
            set
            {
                _selectedPractitioner = (ExternalPractitionerSummary)value.Item;
                PractitionerSelectionChanged();
            }
        }

        public void AddPractitioner()
        {
            try
            {
                ExternalPractitionerEditorComponent editor = new ExternalPractitionerEditorComponent();
                ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                    this.Host.DesktopWindow, editor, SR.TitleAddExternalPractitioner);
                if (exitCode == ApplicationComponentExitCode.Accepted)
                {
                    _practitioners.Items.Add(editor.ExternalPractitionerSummary);
                }
            }
            catch (Exception e)
            {
                // failed to launch editor
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public void UpdateSelectedPractitioner()
        {
            try
            {
                // can occur if user double clicks while holding control
                if (_selectedPractitioner == null) return;

                ExternalPractitionerEditorComponent editor = new ExternalPractitionerEditorComponent(_selectedPractitioner.PractitionerRef);
                ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(
                    this.Host.DesktopWindow, editor, SR.TitleUpdateExternalPractitioner);
                if (exitCode == ApplicationComponentExitCode.Accepted)
                {
                    _practitioners.Items.Replace(
                        delegate(ExternalPractitionerSummary s) { return s.PractitionerRef.Equals(editor.ExternalPractitionerSummary.PractitionerRef, true); },
                        editor.ExternalPractitionerSummary);
                }
            }
            catch (Exception e)
            {
                // failed to launch editor
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public void DoubleClickSelectedPractitioner()
        {
            // double-click behaviour is different depending on whether we're running as a dialog box or not
            if (_dialogMode)
                Accept();
            else
                UpdateSelectedPractitioner();
        }


        public void Search()
        {
            try
            {
                DoSearch();
            }
            catch (Exception e)
            {
                // search failed
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public bool AcceptEnabled
        {
            get { return _selectedPractitioner != null; }
        }

        public void Accept()
        {
            this.ExitCode = ApplicationComponentExitCode.Accepted;
            this.Host.Exit();
        }

        public void Cancel()
        {
            this.ExitCode = ApplicationComponentExitCode.None;
            this.Host.Exit();
        }

        #endregion

        private void DoSearch()
        {
            _listRequest.FirstName = _firstName;
            _listRequest.LastName = _lastName;

            _practitioners.Items.Clear();
            _practitioners.Items.AddRange(_pagingController.GetFirst());
        }

        private void PractitionerSelectionChanged()
        {
            if (_selectedPractitioner != null)
                _staffActionHandler[_updatePractitionerKey].Enabled = true;
            else
                _staffActionHandler[_updatePractitionerKey].Enabled = false;
        }
    }
}
