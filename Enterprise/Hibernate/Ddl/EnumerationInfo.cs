#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClearCanvas.Enterprise.Hibernate.Ddl
{
	/// <summary>
	/// Describes a member of an enumeration.
	/// </summary>
	[DataContract]
	public class EnumerationMemberInfo : ElementInfo
	{
		private string _code;
		private string _value;
		private string _description;
		private float _displayOrder;
		private bool _deactivated;

		public EnumerationMemberInfo()
		{
		}


		internal EnumerationMemberInfo(string code, string value, string description, float displayOrder, bool deactivated)
		{
			_code = code;
			_value = value;
			_description = description;
			_displayOrder = displayOrder;
			_deactivated = deactivated;
		}

		[DataMember]
		public string Code
		{
			get { return _code; }
			private set { _code = value; }
		}

		[DataMember]
		public string Value
		{
			get { return _value; }
			private set { _value = value; }
		}

		[DataMember]
		public string Description
		{
			get { return _description; }
			private set { _description = value; }
		}

		[DataMember]
		public float DisplayOrder
		{
			get { return _displayOrder; }
			private set { _displayOrder = value; }
		}

		[DataMember]
		public bool Deactivated
		{
			get { return _deactivated; }
			private set { _deactivated = value; }
		}

		public override string Identity
		{
			get { return _code; }
		}
	}

	/// <summary>
	/// Describes an enumeration that is defined as part of a relational model.
	/// </summary>
	[DataContract]
	public class EnumerationInfo : ElementInfo
	{
		private string _enumerationClass;
		private List<EnumerationMemberInfo> _members;
		private bool _isHard;
		private string _table;

		public EnumerationInfo()
		{

		}

		internal EnumerationInfo(string enumerationClass, string table, bool isHard, List<EnumerationMemberInfo> members)
		{
			_enumerationClass = enumerationClass;
			_members = members;
			_isHard = isHard;
			_table = table;
		}

		/// <summary>
		/// Gets the .NET class name of the enumeration.
		/// </summary>
		[DataMember]
		public string EnumerationClass
		{
			get { return _enumerationClass; }
			private set { _enumerationClass = value; }
		}

		/// <summary>
		/// Gets the name of the table that holds the enumeration values.
		/// </summary>
		[DataMember]
		public string Table
		{
			get { return _table; }
			private set { _table = value; }
		}

		/// <summary>
		/// Gets the set of member values of the enumeration.
		/// </summary>
		[DataMember]
		public List<EnumerationMemberInfo> Members
		{
			get { return _members; }
			private set { _members = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this enumeration is 'hard' vs. 'soft'.
		/// </summary>
		[DataMember]
		public bool IsHard
		{
			get { return _isHard; }
			private set { _isHard = value; }
		}

		/// <summary>
		/// Gets the unique identity of the element.
		/// </summary>
		/// <remarks>
		/// The identity string must uniquely identify the element within a given set of elements, but need not be globally unique.
		/// </remarks>
		public override string Identity
		{
			get { return _enumerationClass; }
		}
	}
}
