#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer
{
	//NOTE: keep this internal for now, as I'm not too sure of their usefulness right now.
	internal interface IImageSetFactory
	{
		void SetStudyTree(StudyTree studyTree);

		IImageSet CreateImageSet(Study study);
	}

	internal class ImageSetFactory : IImageSetFactory
	{
		private StudyTree _studyTree;
		private readonly IDisplaySetFactory _displaySetFactory;

	    public ImageSetFactory()
			: this(new BasicDisplaySetFactory())
		{
		}

        public ImageSetFactory(IDisplaySetFactory displaySetFactory)
		{
			_displaySetFactory = displaySetFactory;
		}

		#region IImageSetFactory Members

		void IImageSetFactory.SetStudyTree(StudyTree studyTree)
		{
			_studyTree = studyTree;
			_displaySetFactory.SetStudyTree(studyTree);
		}

		IImageSet IImageSetFactory.CreateImageSet(Study study)
		{
			Platform.CheckForNullReference(study, "study");
			Platform.CheckMemberIsSet(_studyTree, "_studyTree");

			return CreateImageSet(study);
		}

		#endregion

		protected virtual IImageSet CreateImageSet(Study study)
		{
			ImageSet imageSet = null;
			List<IDisplaySet> displaySets = CreateDisplaySets(study);

			if (displaySets.Count > 0)
			{
				imageSet = new ImageSet(CreateImageSetDescriptor(study));
				
				foreach (IDisplaySet displaySet in displaySets)
					imageSet.DisplaySets.Add(displaySet);
			}

			return imageSet;
		}

		protected virtual List<IDisplaySet> CreateDisplaySets(Study study)
		{
		    return _displaySetFactory.CreateDisplaySets(study);
		}

        protected virtual DicomImageSetDescriptor CreateImageSetDescriptor(Study study)
		{
			return new DicomImageSetDescriptor(study.GetIdentifier());
		}
	}
}