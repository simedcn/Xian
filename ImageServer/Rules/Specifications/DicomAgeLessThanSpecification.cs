#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Common.Specifications;
using ClearCanvas.Dicom;
using SR=ClearCanvas.Common.SR;

namespace ClearCanvas.ImageServer.Rules.Specifications
{
    [ExtensionOf(typeof(XmlSpecificationCompilerOperatorExtensionPoint))]
    public class DicomAgeLessThanSpecificationOperator : IXmlSpecificationCompilerOperator
    {
        private string GetAttributeOrNull(XmlElement node, string attr)
        {
            string val = node.GetAttribute(attr);
            return string.IsNullOrEmpty(val) ? null : val;
        }

        #region IXmlSpecificationCompilerOperator Members

        public string OperatorTag
        {
            get { return "dicom-age-less-than"; }
        }

        public Specification Compile(XmlElement xmlNode, IXmlSpecificationCompilerContext context)
        {
            string units = xmlNode.GetAttribute("units");

            if (units == null)
                units = "years";

            string refValue = GetAttributeOrNull(xmlNode, "refValue");
            if (refValue == null)
                throw new XmlSpecificationCompilerException("Xml attribute 'refValue' is required.");


            return new DicomAgeLessThanSpecification(units, refValue);
        }

        #endregion
    }

    public class DicomAgeLessThanSpecification : PrimitiveSpecification
    {
        private readonly string _units;
        private readonly string _refValue;

        public DicomAgeLessThanSpecification(string units, string refValue)
        {
            _refValue = refValue;
            _units = units.ToLower();
        }

        protected override TestResult InnerTest(object exp, object root)
        {
            // assume that null doesn't match
            if (exp == null || root == null)
                return DefaultTestResult(false);

            if (exp is string)
            {
                if ((exp as string).Length == 0)
                    return DefaultTestResult(false);

                DateTime comparisonTime = Platform.Time;
                double time;
                if (false == double.TryParse(_refValue, out time))
                    throw new SpecificationException(SR.ExceptionCastExpressionString);

                if (_units.Equals("weeks"))
                    comparisonTime = comparisonTime.AddDays(time * -7f);
                else if (_units.Equals("days"))
                    comparisonTime = comparisonTime.AddDays(-1*time);
                else
                    comparisonTime = comparisonTime.AddYears((int)(-1*time));

                DateTime? testTime = DateTimeParser.Parse(exp as string);

                return DefaultTestResult(comparisonTime < testTime);
            }
            else
            {
                throw new SpecificationException(SR.ExceptionCastExpressionString);
            }
        }
    }
}
