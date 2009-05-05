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

using ClearCanvas.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Services.WorkQueue.ReconcileStudy.MergeStudy;

namespace ClearCanvas.ImageServer.Services.WorkQueue.ReconcileStudy.CreateStudy
{
    /// <summary>
    /// A processor implementing <see cref="IReconcileProcessor"/> to handle "CreateStudy" operation
    /// </summary>
    class ReconcileCreateStudyProcessor : ServerCommandProcessor, IReconcileProcessor
    {
        #region Private Members
        private ReconcileStudyProcessorContext _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of <see cref="ReconcileCreateStudyProcessor"/>
        /// </summary>
        public ReconcileCreateStudyProcessor()
            : base("Create Study")
        {

        }

        #endregion

        #region IReconcileProcessor Members


        public string Name
        {
            get { return "Create Study Processor"; }
        }

        public void Initialize(ReconcileStudyProcessorContext context)
        {
            Platform.CheckForNullReference(context, "context");
            _context = context;
            CreateStudyCommandXmlParser parser = new CreateStudyCommandXmlParser();
            ReconcileCreateStudyDescriptor desc = parser.Parse(_context.History.ChangeDescription);

            if (_context.History.DestStudyStorageKey == null)
            {
                CreateStudyCommand.CommandParameters paramaters = new CreateStudyCommand.CommandParameters();
                paramaters.Commands = desc.Commands;
                CreateStudyCommand command = new CreateStudyCommand(context, paramaters);
                AddCommand(command);
            }
            else
            {
                ReconcileMergeStudyCommandParameters parameters = new ReconcileMergeStudyCommandParameters();
                parameters.Commands = desc.Commands;
                parameters.UpdateDestination = false;
                MergeStudyCommand command = new MergeStudyCommand(_context, parameters);
                AddCommand(command);
            }
        }

        #endregion      
    }
    
}
