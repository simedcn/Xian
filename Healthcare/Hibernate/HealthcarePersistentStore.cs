#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.IO;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Enterprise.Hibernate;


namespace ClearCanvas.Healthcare.Hibernate
{
	[ExtensionOf(typeof(PersistentStoreExtensionPoint))]
	public class HealthcarePersistentStore : PersistentStore
	{
	}
}
