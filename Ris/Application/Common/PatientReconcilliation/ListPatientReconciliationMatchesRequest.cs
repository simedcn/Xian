using System;
using System.Runtime.Serialization;

using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.PatientReconcilliation
{
    [DataContract]
    public class ListPatientReconciliationMatchesRequest : DataContractBase
    {
        public ListPatientReconciliationMatchesRequest(EntityRef patientProfileRef)
        {
            this.PatientProfileRef = patientProfileRef;
        }

        [DataMember(IsRequired=true)]
        public EntityRef PatientProfileRef;
    }
}
