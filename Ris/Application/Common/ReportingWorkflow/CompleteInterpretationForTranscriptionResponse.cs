using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.ReportingWorkflow
{
    [DataContract]
    public class CompleteInterpretationForTranscriptionResponse : DataContractBase
    {
        public CompleteInterpretationForTranscriptionResponse(EntityRef stepRef)
        {
            this.ReportingStepRef = stepRef;
        }

        [DataMember]
        public EntityRef ReportingStepRef;
    }
}