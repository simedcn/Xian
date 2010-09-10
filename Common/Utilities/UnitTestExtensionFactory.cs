﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
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

#if UNIT_TESTS

// ReSharper disable SuggestBaseTypeForParameter

using System;
using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Common.Utilities
{
	//TODO (CR Sept 2010): move to Tests namespace?

	/// <summary>
	/// An <see cref="IExtensionFactory"/> that returns only extensions that have been explicitly mapped.
	/// </summary>
	/// <remarks>
	/// This <see cref="IExtensionFactory"/> is useful in unit test scenarios where precise control over
	/// creation of extensions is required. Simply create an instance of this class and map individual
	/// extension types to <see cref="IExtensionPoint"/> types.
	/// </remarks>
	public class UnitTestExtensionFactory : IExtensionFactory, IDictionary<Type, Type>
	{
		private readonly Dictionary<Type, List<Type>> _extensionMap = new Dictionary<Type, List<Type>>();

		/// <summary>
		/// Instantiates an empty <see cref="UnitTestExtensionFactory"/>.
		/// </summary>
		public UnitTestExtensionFactory() {}

		/// <summary>
		/// Instantiates an <see cref="UnitTestExtensionFactory"/> with the provided <paramref name="extensionMap">extensions map</paramref>.
		/// </summary>
		public UnitTestExtensionFactory(IDictionary<Type, IEnumerable<Type>> extensionMap)
		{
			foreach (var entry in extensionMap)
				foreach (var type in entry.Value)
					Define(entry.Key, type);
		}

		/// <summary>
		/// Instantiates an <see cref="UnitTestExtensionFactory"/> with the provided <paramref name="extensionMap">extensions map</paramref>.
		/// </summary>
		public UnitTestExtensionFactory(IDictionary<Type, Type> extensionMap)
		{
			foreach (var entry in extensionMap)
				Define(entry.Key, entry.Value);
		}

		/// <summary>
		/// Defines a type as an extension of the specified <see cref="IExtensionPoint"/> type.
		/// </summary>
		/// <param name="extensionPoint">The type of the <see cref="IExtensionPoint"/>.</param>
		/// <param name="extension">The type of the extension.</param>
		public void Define(Type extensionPoint, Type extension)
		{
			if (!typeof (IExtensionPoint).IsAssignableFrom(extensionPoint))
				throw new ArgumentException("Extension point class must implement IExtensionPoint", "extensionPoint");

			if (!_extensionMap.ContainsKey(extensionPoint))
				_extensionMap.Add(extensionPoint, new List<Type>());
			_extensionMap[extensionPoint].Add(extension);
		}

		/// <summary>
		/// Undefines all extensions for the specified <see cref="IExtensionPoint"/> type.
		/// </summary>
		/// <param name="extensionPoint">The type of the <see cref="IExtensionPoint"/>.</param>
		/// <returns>True if any extensions were undefined; False otherwise.</returns>
		public bool UndefineAll(Type extensionPoint)
		{
			if (!typeof (IExtensionPoint).IsAssignableFrom(extensionPoint))
				throw new ArgumentException("Extension point class must implement IExtensionPoint", "extensionPoint");
			return _extensionMap.Remove(extensionPoint);
		}

		/// <summary>
		/// Checks if there are any extensions defined for the specified <see cref="IExtensionPoint"/> type.
		/// </summary>
		/// <param name="extensionPoint">The type of the <see cref="IExtensionPoint"/>.</param>
		/// <returns>True if extensions are undefined; False otherwise.</returns>
		public bool HasExtensions(Type extensionPoint)
		{
			if (!typeof (IExtensionPoint).IsAssignableFrom(extensionPoint))
				throw new ArgumentException("Extension point class must implement IExtensionPoint", "extensionPoint");
			return _extensionMap.ContainsKey(extensionPoint);
		}

		/// <summary>
		/// Gets a list of <see cref="IExtensionPoint"/> types for which extensions have been defined.
		/// </summary>
		public ICollection<Type> ExtensionPoints
		{
			get { return _extensionMap.Keys; }
		}

		#region IExtensionFactory Members

		/// <summary>
		/// Creates one of each type of object that extends the input <paramref name="extensionPoint" />, 
		/// matching the input <paramref name="filter" />; creates a single extension if <paramref name="justOne"/> is true.
		/// </summary>
		/// <param name="extensionPoint">The <see cref="ExtensionPoint"/> to create extensions for.</param>
		/// <param name="filter">The filter used to match each extension that is discovered.</param>
		/// <param name="justOne">Indicates whether or not to return only the first matching extension that is found.</param>
		/// <returns></returns>
		public virtual object[] CreateExtensions(ExtensionPoint extensionPoint, ExtensionFilter filter, bool justOne)
		{
			var extensionInfos = ListExtensions(extensionPoint, filter);
			if (justOne && extensionInfos.Length > 1)
				extensionInfos = new[] {extensionInfos[0]};
			return CollectionUtils.Map<ExtensionInfo, object>(extensionInfos, extensionInfo => Activator.CreateInstance(extensionInfo.ExtensionClass)).ToArray();
		}

		/// <summary>
		/// Gets metadata describing all extensions of the input <paramref name="extensionPoint"/>, 
		/// matching the given <paramref name="filter"/>.
		/// </summary>
		/// <param name="extensionPoint">The <see cref="ExtensionPoint"/> whose extension metadata is to be retrieved.</param>
		/// <param name="filter">An <see cref="ExtensionFilter"/> used to filter out extensions with particular characteristics.</param>
		/// <returns></returns>
		public virtual ExtensionInfo[] ListExtensions(ExtensionPoint extensionPoint, ExtensionFilter filter)
		{
			if (extensionPoint == null)
				throw new ArgumentNullException("extensionPoint");

			var extensionPointType = extensionPoint.GetType();
			if (!_extensionMap.ContainsKey(extensionPointType))
				return new ExtensionInfo[0];

			var extensions = new List<ExtensionInfo>();
			foreach (var extensionType  in _extensionMap[extensionPointType])
			{
				var extensionInfo = new ExtensionInfo(extensionType, extensionPointType, extensionType.Name, extensionType.AssemblyQualifiedName, true);
				if (filter == null || filter.Test(extensionInfo))
					extensions.Add(extensionInfo);
			}
			return extensions.ToArray();
		}

		#endregion

		#region IDictionary<Type, Type> Members

		/// <summary>
		/// See <see cref="Define"/>.
		/// </summary>
		/// <remarks>
		/// This is separately declared here to support collection initializer syntax.
		/// </remarks>
		public void Add(Type key, Type value)
		{
			Define(key, value);
		}

		bool IDictionary<Type, Type>.ContainsKey(Type key)
		{
			return HasExtensions(key);
		}

		ICollection<Type> IDictionary<Type, Type>.Keys
		{
			get { return ExtensionPoints; }
		}

		bool IDictionary<Type, Type>.Remove(Type key)
		{
			return UndefineAll(key);
		}

		bool IDictionary<Type, Type>.TryGetValue(Type key, out Type value)
		{
			List<Type> list;
			bool result = _extensionMap.TryGetValue(key, out list);
			if (result)
			{
				value = CollectionUtils.FirstElement(list);
				return value != null;
			}
			value = null;
			return false;
		}

		ICollection<Type> IDictionary<Type, Type>.Values
		{
			get
			{
				var list = new List<Type>();
				foreach (var value in _extensionMap.Values)
				{
					list.AddRange(value);
				}
				return list.AsReadOnly();
			}
		}

		Type IDictionary<Type, Type>.this[Type key]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		#endregion

		#region ICollection<KeyValuePair<Type,Type>> Members

		void ICollection<KeyValuePair<Type, Type>>.Add(KeyValuePair<Type, Type> item)
		{
			Define(item.Key, item.Value);
		}

		public void Clear()
		{
			_extensionMap.Clear();
		}

		bool ICollection<KeyValuePair<Type, Type>>.Contains(KeyValuePair<Type, Type> item)
		{
			return _extensionMap.ContainsKey(item.Key);
		}

		void ICollection<KeyValuePair<Type, Type>>.CopyTo(KeyValuePair<Type, Type>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		int ICollection<KeyValuePair<Type, Type>>.Count
		{
			get { return ((IDictionary<Type, Type>) this).Values.Count; }
		}

		bool ICollection<KeyValuePair<Type, Type>>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<Type, Type>>.Remove(KeyValuePair<Type, Type> item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<KeyValuePair<Type,Type>> Members

		IEnumerator<KeyValuePair<Type, Type>> IEnumerable<KeyValuePair<Type, Type>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

// ReSharper restore SuggestBaseTypeForParameter
#endif