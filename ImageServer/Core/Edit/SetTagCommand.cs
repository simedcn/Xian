﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using System.Xml.Serialization;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Common.Helpers;
using ClearCanvas.ImageServer.Core.Edit;

namespace ClearCanvas.ImageServer.Core.Edit
{
	/// <summary>
	/// Represents a command that can be executed on an <see cref="DicomFile"/>
	/// </summary>
	/// <remarks>
	/// This class is serializable.
	/// </remarks>
	[XmlRoot("SetTag")]
	public class SetTagCommand : BaseImageLevelUpdateCommand, IUpdateImageTagCommand
	{
		#region Private Fields
		private ImageLevelUpdateEntry _updateEntry = new ImageLevelUpdateEntry();
		#endregion

		#region Constructors
		/// <summary>
		/// **** For serialization purpose. ****
		/// </summary>
		public SetTagCommand()
			: base("SetTag")
		{
			Description = "Update Dicom Tag";
		}

		/// <summary>
		/// Creates an instance of <see cref="SetTagCommand"/> that can be used to update the specified dicom tag with the specified value
		/// </summary>
		/// <remarks>
		/// <see cref="BaseImageLevelUpdateCommand.File"/> must be set prior to <see cref="BaseImageLevelUpdateCommand.OnExecute"></see>
		/// </remarks>
		public SetTagCommand(uint tag, string value)
			: this()
		{
			UpdateEntry.TagPath = new DicomTagPath();
			UpdateEntry.TagPath.Tag = DicomTagDictionary.GetDicomTag(tag);
			UpdateEntry.Value = value;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the <see cref="ImageLevelUpdateEntry"/> for this command.
		/// </summary>
		[XmlIgnore]
		public ImageLevelUpdateEntry UpdateEntry
		{
			get { return _updateEntry; }
			set { _updateEntry = value; }
		}

		/// <summary>
		/// Gets the name of the Dicom tag affected by this command.
		/// **** For XML serialization purpose. ****
		/// </summary>
		[XmlAttribute(AttributeName = "TagName")]
		public string TagName
		{
			get { return _updateEntry.TagPath.Tag.Name; }
			set
			{
				// NO-OP 
			}
		}

		/// <summary>
		/// Gets or sets the Dicom tag value to be used by this command when updating the dicom file.
		/// </summary>
		[XmlAttribute(AttributeName = "Value")]
		public string Value
		{
			get
			{
				if (_updateEntry == null)
					return null;

				return _updateEntry.Value != null ? _updateEntry.Value.ToString() : null;
			}
			set
			{
				_updateEntry.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets 
		/// </summary>
		[XmlAttribute(AttributeName = "TagPath")]
		public string TagPath
		{
			get { return _updateEntry.TagPath.HexString(); }
			set
			{
				DicomTagPathConverter converter = new DicomTagPathConverter();
				_updateEntry.TagPath = (DicomTagPath)converter.ConvertFromString(value);
			}
		}

		#endregion

		#region IImageLevelCommand Members

		public override bool Apply(DicomFile file)
		{
			if (_updateEntry != null)
			{
				DicomAttribute attr = FindAttribute(file.DataSet, UpdateEntry);
				if (attr != null)
					attr.SetStringValue(UpdateEntry.GetStringValue());
			}

			return true;
		}

		public override string ToString()
		{
			return String.Format("Set {0}={1}", UpdateEntry.TagPath.Tag, UpdateEntry.Value);
		}
		#endregion

		#region IImageLevelCommand Members

		#endregion



		protected DicomAttribute FindAttribute(DicomAttributeCollection collection, ImageLevelUpdateEntry entry)
		{
			if (collection == null)
				return null;

			if (entry.TagPath.Parents != null)
			{
				foreach (DicomTag tag in entry.TagPath.Parents)
				{
					DicomAttribute sq = collection[tag] as DicomAttributeSQ;
					if (sq == null)
					{
						throw new Exception(String.Format("Invalid tag value: {0}({1}) is not a SQ VR", tag, tag.Name));
					}
					if (sq.IsEmpty)
					{
						DicomSequenceItem item = new DicomSequenceItem();
						sq.AddSequenceItem(item);
					}

					DicomSequenceItem[] items = sq.Values as DicomSequenceItem[];
					Platform.CheckForNullReference(items, "items");
					collection = items[0];
				}
			}

			return collection[entry.TagPath.Tag];
		}
	}
}