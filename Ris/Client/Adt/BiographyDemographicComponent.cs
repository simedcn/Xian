using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.PatientBiography;
using ClearCanvas.Ris.Client;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client.Adt
{
    /// <summary>
    /// Extension point for views onto <see cref="BiographyDemographicComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class BiographyDemographicComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// BiographyDemographicComponent class
    /// </summary>
    [AssociateView(typeof(BiographyDemographicComponentViewExtensionPoint))]
    public class BiographyDemographicComponent : ApplicationComponent
    {
        private EntityRef _profileRef;
        private PatientProfileDetail _patientProfile;

        private PatientProfileSummary _selectedProfile;
        private List<PatientProfileSummary> _profileChoices;

        private AddressDetail _selectedAddress;
        private TelephoneDetail _selectedPhone;
        private EmailAddressDetail _selectedEmail;
        private ContactPersonDetail _selectedContact;

        private AddressTable _addressTable;
        private TelephoneNumberTable _phoneTable;
        private EmailAddressTable _emailTable;
        private ContactPersonTable _contactTable;

        private List<EnumValueInfo> _addressTypeChoices;
        private List<EnumValueInfo> _phoneTypeChoices;
        private List<EnumValueInfo> _contactPersonTypeChoices;
        private List<EnumValueInfo> _contactPersonRelationshipChoices;

        /// <summary>
        /// Constructor
        /// </summary>
        public BiographyDemographicComponent(EntityRef profileRef, PatientProfileDetail patientProfile)
        {
            _profileRef = profileRef;
            _patientProfile = patientProfile;

            _profileChoices = new List<PatientProfileSummary>();

            _addressTable = new AddressTable();
            _phoneTable = new TelephoneNumberTable();
            _emailTable = new EmailAddressTable();
            _contactTable = new ContactPersonTable();

            _addressTypeChoices = new List<EnumValueInfo>();
            _phoneTypeChoices = new List<EnumValueInfo>();
            _contactPersonTypeChoices = new List<EnumValueInfo>();
            _contactPersonRelationshipChoices = new List<EnumValueInfo>();
        }

        public override void Start()
        {
            base.Start();

            try
            {
                Platform.GetService<IPatientBiographyService>(
                    delegate(IPatientBiographyService service)
                    {
                        ListAllProfilesForPatientResponse response = service.ListAllProfilesForPatient(new ListAllProfilesForPatientRequest(_profileRef));
                        _profileChoices = response.Profiles;

                        LoadPatientProfileFormDataResponse formDataResponse = service.LoadPatientProfileFormData(new LoadPatientProfileFormDataRequest());
                        _addressTypeChoices = formDataResponse.AddressTypeChoices;
                        _phoneTypeChoices = formDataResponse.PhoneTypeChoices;
                        _contactPersonTypeChoices = formDataResponse.ContactPersonTypeChoices;
                        _contactPersonRelationshipChoices = formDataResponse.ContactPersonRelationshipChoices;
                    });

                UpdateTables();
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }

        private void UpdateTables()
        {
            if (_patientProfile == null)
                return;

            _addressTable.Items.Clear();
            _phoneTable.Items.Clear();
            _emailTable.Items.Clear();
            _contactTable.Items.Clear();

            _addressTable.Items.AddRange(_patientProfile.Addresses);
            _phoneTable.Items.AddRange(_patientProfile.TelephoneNumbers);
            _emailTable.Items.AddRange(_patientProfile.EmailAddresses);
            _contactTable.Items.AddRange(_patientProfile.ContactPersons);
        }

        private string ProfileStringConverter(PersonNameDetail name, MrnDetail mrn)
        {
            return String.Format("{0} - {1}", MrnFormat.Format(mrn), PersonNameFormat.Format(name));
        }

        private void OnSelectedProfileChanged()
        {
            if (_selectedProfile == null)
                return;

            try
            {
                Platform.GetService<IPatientBiographyService>(
                    delegate(IPatientBiographyService service)
                    {
                        LoadPatientProfileResponse response = service.LoadPatientProfile(new LoadPatientProfileRequest(_selectedProfile.ProfileRef));
                        _patientProfile = response.PatientDetail;
                    });

                UpdateTables();

                NotifyAllPropertiesChanged();       
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Host.DesktopWindow);
            }
        }

        #region Presentation Model

        public List<string> ProfileChoices
        {
            get
            {
                List<string> profileStrings = new List<string>();
                if (_profileChoices.Count > 0)
                {
                    profileStrings.AddRange(
                        CollectionUtils.Map<PatientProfileSummary, string>(
                            _profileChoices, delegate(PatientProfileSummary pp) { return ProfileStringConverter(pp.Name, pp.Mrn); }));
                }

                return profileStrings;
            }
        }

        public string SelectedProfile
        {
            get { return _selectedProfile == null ? "" : ProfileStringConverter(_selectedProfile.Name, _selectedProfile.Mrn); }
            set
            {
                _selectedProfile = (value == "") ? null :
                    CollectionUtils.SelectFirst<PatientProfileSummary>(_profileChoices,
                        delegate(PatientProfileSummary pp) { return ProfileStringConverter(pp.Name, pp.Mrn) == value; });

                OnSelectedProfileChanged();
            }
        }

        public ITable Addresses
        {
            get { return _addressTable; }
        }

        public ISelection SelectedAddress
        {
            get { return new Selection(_selectedAddress); }
            set { _selectedAddress = (AddressDetail)value.Item; }
        }

        public ITable PhoneNumbers
        {
            get { return _phoneTable; }
        }

        public ISelection SelectedPhone
        {
            get { return new Selection(_selectedPhone); }
            set { _selectedPhone = (TelephoneDetail)value.Item; }
        }

        public ITable EmailAddresses
        {
            get { return _emailTable; }
        }

        public ISelection SelectedEmail
        {
            get { return new Selection(_selectedEmail); }
            set { _selectedEmail = (EmailAddressDetail)value.Item; }
        }

        public ITable ContactPersons
        {
            get { return _contactTable; }
        }

        public ISelection SelectedContact
        {
            get { return new Selection(_selectedContact); }
            set { _selectedContact = (ContactPersonDetail)value.Item; }
        }

        public string FamilyName
        {
            get { return _patientProfile.Name.FamilyName; }
        }

        public string GivenName
        {
            get { return _patientProfile.Name.GivenName; }
        }

        public string MiddleName
        {
            get { return _patientProfile.Name.MiddleName; }
        }

        public string Prefix
        {
            get { return _patientProfile.Name.Prefix; }
        }

        public string Suffix
        {
            get { return _patientProfile.Name.Suffix; }
        }

        public string Degree
        {
            get { return _patientProfile.Name.Degree; }
        }

        public string Sex
        {
            get { return _patientProfile.Sex.Value; }
        }

        public string DateOfBirth
        {
            get { return Format.Date(_patientProfile.DateOfBirth); }
        }

        public string TimeOfDeath
        {
            get { return Format.DateTime(_patientProfile.TimeOfDeath); }
        }

        public string Religion
        {
            get { return _patientProfile.Religion.Value; }
        }

        public string PrimaryLanguage
        {
            get { return _patientProfile.PrimaryLanguage.Value; }
        }

        public string Mrn
        {
            get { return _patientProfile.Mrn.Id; }
        }

        public string MrnSite
        {
            get { return _patientProfile.Mrn.AssigningAuthority; }
        }

        public string Healthcard
        {
            get { return _patientProfile.Healthcard.Id; }
        }

        public string HealthcardProvince
        {
            get { return _patientProfile.Healthcard.AssigningAuthority; }
        }

        public string HealthcardVersionCode
        {
            get { return _patientProfile.Healthcard.VersionCode; }
        }

        public string HealthcardExpiry
        {
            get { return Format.Date(_patientProfile.Healthcard.ExpiryDate); }
        }

        public void ShowSelectedAddress()
        {
            if (_selectedAddress == null)
                return;

            AddressEditorComponent editor = new AddressEditorComponent(_selectedAddress, _addressTypeChoices);
            ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleAddresses);
        }

        public void ShowSelectedPhone()
        {
            if (_selectedPhone == null)
                return;

            PhoneNumberEditorComponent editor = new PhoneNumberEditorComponent(_selectedPhone, _phoneTypeChoices);
            ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitlePhoneNumbers);
        }

        public void ShowSelectedEmail()
        {
            if (_selectedEmail == null)
                return;

            EmailAddressEditorComponent editor = new EmailAddressEditorComponent(_selectedEmail);
            ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleEmailAddresses);
        }

        public void ShowSelectedContact()
        {
            if (_selectedContact == null)
                return;

            ContactPersonEditorComponent editor = new ContactPersonEditorComponent(_selectedContact, _contactPersonTypeChoices, _contactPersonRelationshipChoices);
            ApplicationComponent.LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleContactPersons);
        }

        #endregion
    }
}
