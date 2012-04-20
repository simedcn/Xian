#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Runtime.Serialization;
using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
    public interface IStudyRootStudyIdentifier : IStudyRootData, IStudyIdentifier
    {
    }

    /// <summary>
    /// Study Root Query identifier for a study.
    /// </summary>
    [DataContract(Namespace = QueryNamespace.Value)]
    public class StudyRootStudyIdentifier : StudyIdentifier, IStudyRootStudyIdentifier
    {
        #region Private Fields

        #endregion

        #region Public Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StudyRootStudyIdentifier()
        {
        }

        public StudyRootStudyIdentifier(IStudyRootStudyIdentifier other)
            : base(other)
        {
            CopyFrom(other);
        }

        public StudyRootStudyIdentifier(IStudyRootData other, IIdentifier identifier)
            : base(other, identifier)
        {
            CopyFrom(other);
        }

        public StudyRootStudyIdentifier(IStudyRootData other)
            : base(other)
        {
            CopyFrom(other);
        }

        public StudyRootStudyIdentifier(IPatientData patientData, IStudyIdentifier identifier)
            : base(identifier)
        {
            CopyFrom(patientData);
        }

        public StudyRootStudyIdentifier(IPatientData patientData, IStudyData studyData, IIdentifier identifier)
            : base(studyData, identifier)
        {
            CopyFrom(patientData);
        }

        /// <summary>
        /// Creates an instance of <see cref="StudyRootStudyIdentifier"/> from a <see cref="DicomAttributeCollection"/>.
        /// </summary>
        public StudyRootStudyIdentifier(DicomAttributeCollection attributes)
            : base(attributes)
        {
        }

        #endregion

        private void CopyFrom(IPatientData other)
        {
            if (other == null)
                return;

            PatientId = other.PatientId;
            PatientsName = other.PatientsName;
            PatientsBirthDate = other.PatientsBirthDate;
            PatientsBirthTime = other.PatientsBirthTime;
            PatientsSex = other.PatientsSex;

            PatientSpeciesDescription = other.PatientSpeciesDescription;
            PatientSpeciesCodeSequenceCodingSchemeDesignator = other.PatientSpeciesCodeSequenceCodingSchemeDesignator;
            PatientSpeciesCodeSequenceCodeValue = other.PatientSpeciesCodeSequenceCodeValue;
            PatientSpeciesCodeSequenceCodeMeaning = other.PatientSpeciesCodeSequenceCodeMeaning;
            PatientBreedDescription = other.PatientBreedDescription;
            PatientBreedCodeSequenceCodingSchemeDesignator = other.PatientBreedCodeSequenceCodingSchemeDesignator;
            PatientBreedCodeSequenceCodeValue = other.PatientBreedCodeSequenceCodeValue;
            PatientBreedCodeSequenceCodeMeaning = other.PatientBreedCodeSequenceCodeMeaning;
            ResponsiblePerson = other.ResponsiblePerson;
            ResponsiblePersonRole = other.ResponsiblePersonRole;
            ResponsibleOrganization = other.ResponsibleOrganization;
        }

        public override string ToString()
        {
            return String.Format("{0} | {1} | {2}", PatientsName, PatientId, base.ToString());
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the patient id of the identified study.
        /// </summary>
        [DicomField(DicomTags.PatientId, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
        [DataMember(IsRequired = false)]
        public string PatientId { get; set; }

        /// <summary>
        /// Gets or sets the patient's name for the identified study.
        /// </summary>
        [DicomField(DicomTags.PatientsName, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
        [DataMember(IsRequired = false)]
        public string PatientsName { get; set; }

        /// <summary>
        /// Gets or sets the patient's birth date for the identified study.
        /// </summary>
        [DicomField(DicomTags.PatientsBirthDate, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
        [DataMember(IsRequired = false)]
        public string PatientsBirthDate { get; set; }

        /// <summary>
        /// Gets or sets the patient's birth time for the identified study.
        /// </summary>
        [DicomField(DicomTags.PatientsBirthTime, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
        [DataMember(IsRequired = false)]
        public string PatientsBirthTime { get; set; }

        /// <summary>
        /// Gets or sets the patient's sex for the identified study.
        /// </summary>
        [DicomField(DicomTags.PatientsSex, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
        [DataMember(IsRequired = false)]
        public string PatientsSex { get; set; }

        #region Species

        [DicomField(DicomTags.PatientSpeciesDescription)]
        [DataMember(IsRequired = false)]
        public string PatientSpeciesDescription { get; set; }

        [DicomField(DicomTags.CodingSchemeDesignator, DicomTags.PatientSpeciesCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientSpeciesCodeSequenceCodingSchemeDesignator { get; set; }

        [DicomField(DicomTags.CodeValue, DicomTags.PatientSpeciesCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientSpeciesCodeSequenceCodeValue { get; set; }

        [DicomField(DicomTags.CodeMeaning, DicomTags.PatientSpeciesCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientSpeciesCodeSequenceCodeMeaning { get; set; }

        #endregion

        #region Breed

        [DicomField(DicomTags.PatientBreedDescription)]
        [DataMember(IsRequired = false)]
        public string PatientBreedDescription { get; set; }

        [DicomField(DicomTags.CodingSchemeDesignator, DicomTags.PatientBreedCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientBreedCodeSequenceCodingSchemeDesignator { get; set; }

        [DicomField(DicomTags.CodeValue, DicomTags.PatientBreedCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientBreedCodeSequenceCodeValue { get; set; }

        [DicomField(DicomTags.CodeMeaning, DicomTags.PatientBreedCodeSequence)]
        [DataMember(IsRequired = false)]
        public string PatientBreedCodeSequenceCodeMeaning { get; set; }

        #endregion

        #region Responsible Person/Organization

        [DicomField(DicomTags.ResponsiblePerson)]
        [DataMember(IsRequired = false)]
        public string ResponsiblePerson { get; set; }

        [DicomField(DicomTags.ResponsiblePersonRole)]
        [DataMember(IsRequired = false)]
        public string ResponsiblePersonRole { get; set; }

        [DicomField(DicomTags.ResponsibleOrganization)]
        [DataMember(IsRequired = false)]
        public string ResponsibleOrganization { get; set; }

        #endregion

        #endregion
    }
}