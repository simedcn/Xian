#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common.Serialization;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.Admin.EnumerationAdmin
{
    [DataContract]
    public class ListEnumerationValuesResponse : DataContractBase
    {
        public ListEnumerationValuesResponse()
        {

        }

		public ListEnumerationValuesResponse(List<EnumValueAdminInfo> values)
        {
            this.Values = values;
        }

        [DataMember]
		public List<EnumValueAdminInfo> Values;
    }
}
