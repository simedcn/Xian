#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

#if UNIT_TESTS

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ClearCanvas.Common;
using ClearCanvas.Workflow;

namespace ClearCanvas.Healthcare.Tests
{
    [TestFixture]
    public class PublicationStepTests
    {
        #region Property Tests

        [Test]
        public void Test_Name()
        {
            PublicationStep procedureStep = new PublicationStep();

            Assert.AreEqual("Publication", procedureStep.Name);
        }

        [Test]
        public void Test_FailureCount()
        {
            PublicationStep procedureStep = new PublicationStep();

            Assert.AreEqual(0, procedureStep.FailureCount);
        }

        [Test]
        public void Test_LastFailureTime()
        {
            PublicationStep procedureStep = new PublicationStep();

            Assert.IsNull(procedureStep.LastFailureTime);
        }

        #endregion
        
        #region Constructor Test

        [Test]
        public void Test_Constructor_PreviousStep()
        {
            InterpretationStep previousStep = new InterpretationStep(new Procedure());
            PublicationStep newStep = new PublicationStep(previousStep);

            Assert.AreEqual(0, newStep.FailureCount);
            Assert.IsNull(newStep.LastFailureTime);
        }

        #endregion

        #region Method Tests

        [Test]
        public void Test_Fail()
        {
            PublicationStep procedureStep = new PublicationStep();
            Assert.AreEqual(0, procedureStep.FailureCount);
            Assert.IsNull(procedureStep.LastFailureTime);

            procedureStep.Fail();

            Assert.AreEqual(1, procedureStep.FailureCount);
            Assert.IsTrue(RoughlyEqual(procedureStep.LastFailureTime, Platform.Time));

            procedureStep.Fail();

            Assert.AreEqual(2, procedureStep.FailureCount);
            Assert.IsTrue(RoughlyEqual(procedureStep.LastFailureTime, Platform.Time));
        }

        [Test]
        public void Test_Complete()
        {
            Procedure procedure = new Procedure();
            Report report = new Report(procedure);
            ReportPart reportPart = new ReportPart(report, 0);

            // This modality procedure step is created such that when the procedure tries to update its status
            // the detection of no ModalityProcedureSteps makes any procedure trying to update its status set itself
            // to discontinued, which for this test, is undesirable, this situation will probably never happen
            // in practice.
            ModalityProcedureStep modalityStep = new ModalityProcedureStep(procedure, "New modality.", new Modality());

            InterpretationStep previousStep = new InterpretationStep(procedure);
            previousStep.ReportPart = reportPart;
            PublicationStep procedureStep = new PublicationStep(previousStep);
            
            procedureStep.Start(new Staff());

            Assert.AreEqual(ActivityStatus.IP, procedureStep.State);
            Assert.AreEqual(0, procedureStep.ReportPart.Index);
            Assert.AreEqual(ReportPartStatus.D, procedureStep.ReportPart.Status);
            Assert.IsTrue(procedureStep.AllProcedures.TrueForAll(
                delegate(Procedure p)
                {
                    return p.Status == ProcedureStatus.IP;
                }));

            procedureStep.Complete();

            Assert.AreEqual(ActivityStatus.CM, procedureStep.State);
            Assert.AreEqual(ReportPartStatus.F, procedureStep.ReportPart.Status);
            Assert.IsTrue(procedureStep.AllProcedures.TrueForAll(
                delegate(Procedure p)
                {
                    return p.Status == ProcedureStatus.CM;
                }));
            Assert.IsTrue(procedureStep.AllProcedures.TrueForAll(
                delegate(Procedure p)
                {
                    return p.EndTime == procedureStep.EndTime;
                }));
        }

        [Test]
        public void Test_Reassign()
        {
            InterpretationStep previousStep = new InterpretationStep(new Procedure());
            PublicationStep procedureStep = new PublicationStep(previousStep);

            PublicationStep newStep = (PublicationStep)procedureStep.Reassign(new Staff());

            Assert.AreEqual(procedureStep, newStep);
            Assert.IsInstanceOfType(typeof(PublicationStep), newStep);
        }

        #endregion

        private static bool RoughlyEqual(DateTime? x, DateTime? y)
        {
            if (!x.HasValue && !y.HasValue)
                return true;

            if (!x.HasValue || !y.HasValue)
                return false;

            DateTime xx = x.Value;
            DateTime yy = y.Value;

            // for these purposes, if the times are within 1 second, that is good enough
            return Math.Abs((xx - yy).TotalSeconds) < 1;
        }
    }
}

#endif