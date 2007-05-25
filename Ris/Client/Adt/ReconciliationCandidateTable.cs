using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tables;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Client.Formatting;

namespace ClearCanvas.Ris.Client.Adt
{
    class ReconciliationCandidateTable : Table<ReconciliationCandidateTableEntry>
    {
        public ReconciliationCandidateTable()
        {
            this.Columns.Add(
               new TableColumn<ReconciliationCandidateTableEntry, bool>(SR.ColumnAbbreviationReconciliation,
                   delegate(ReconciliationCandidateTableEntry item) { return item.Checked; },
                   delegate(ReconciliationCandidateTableEntry item, bool value) { item.Checked = value; }, 0.5f));
            this.Columns.Add(
                new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnScore,
                    delegate(ReconciliationCandidateTableEntry item) { return item.ReconciliationCandidate.Score.ToString(); }, 1.0f));
            this.Columns.Add(
                new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnSite,
                    delegate(ReconciliationCandidateTableEntry item) { return item.ReconciliationCandidate.PatientProfile.Mrn.AssigningAuthority; }, 0.5f));
            this.Columns.Add(
               new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnMRN,
                   delegate(ReconciliationCandidateTableEntry item) { return item.ReconciliationCandidate.PatientProfile.Mrn.Id; }, 1.0f));
            this.Columns.Add(
              new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnName,
                  delegate(ReconciliationCandidateTableEntry item) { return PersonNameFormat.Format(item.ReconciliationCandidate.PatientProfile.Name); }, 2.0f));
            this.Columns.Add(
              new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnHealthcardNumber,
                  delegate(ReconciliationCandidateTableEntry item) { return HealthcardFormat.Format(item.ReconciliationCandidate.PatientProfile.Healthcard); }, 1.0f));
            this.Columns.Add(
              new TableColumn<ReconciliationCandidateTableEntry, DateTime>(SR.ColumnDateOfBirth,
                  delegate(ReconciliationCandidateTableEntry item) { return item.ReconciliationCandidate.PatientProfile.DateOfBirth; }, 1.0f));
            this.Columns.Add(
              new TableColumn<ReconciliationCandidateTableEntry, string>(SR.ColumnSex,
                  delegate(ReconciliationCandidateTableEntry item) { return item.ReconciliationCandidate.PatientProfile.Sex.Value; }, 0.5f));
        }
    }
}
