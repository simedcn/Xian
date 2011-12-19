#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Text;
using System.Windows.Forms;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.Desktop.View.WinForms
{
    internal partial class ExceptionDialogControl : CustomUserControl
    {
    	private readonly Exception _exception;

		internal ExceptionDialogControl(Exception exception, string message, ExceptionDialogActions buttonActions, ClickHandlerDelegate ok, ClickHandlerDelegate quit)
		{
        	_exception = exception;

			InitializeComponent();

			_description.Text = message;

			AcceptButton = _okButton;
			CancelButton = _okButton;

			EventHandler okClick = delegate
			                       	{
			                       		Result = ExceptionDialogAction.Ok;
			                       		ok();
			                       	};

			EventHandler quitClick = delegate
			                         	{
			                         		Result = ExceptionDialogAction.Quit;
			                         		quit();
			                         	};

			if (buttonActions == ExceptionDialogActions.Ok)
			{
				if (ok == null)
					throw new ArgumentException("Ok method must be supplied", "ok");

				_okButton.Click += okClick;
				_quitButton.Dispose();
			}
			else if (buttonActions == ExceptionDialogActions.Quit)
			{
				if (quit == null)
					throw new ArgumentException("Quit method must be supplied", "quit");

				_quitButton.Click += quitClick;
				AcceptButton = _quitButton;
				CancelButton = _quitButton;
				_okButton.Dispose();
			}
			else
			{
				if (ok == null)
					throw new ArgumentException("Ok method must be supplied", "ok");
				if (quit == null)
					throw new ArgumentException("Quit method must be supplied", "quit");

				_okButton.Click += okClick;
				_okButton.Text = SR.MenuContinue;
				_quitButton.Click += quitClick;
			}

			if (_exception != null)
			{
				// Update Exceptions detail tree
				_detailTree.BeginUpdate();
				BuildTreeFromException(null, _exception);
				_detailTree.ExpandAll();
				_detailTree.EndUpdate();
			}
			else
			{
				_detailButton.Dispose();
			}

        	// Hide the details when dialog first startup
            HideDetails();
        }

		public ExceptionDialogAction Result { get; private set; }

        #region Event functions

        private void _detailButton_Click(object sender, EventArgs e)
        {
            if (_detailTree.Visible)
                HideDetails();
            else
                ShowDetails();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Copy exception details to clipboard
			string clipboardMessage = SR.ExceptionHandlerMessagePrefix + _description.Text + Environment.NewLine + Environment.NewLine;
            clipboardMessage += BuildMessageFromException(_exception);
            Clipboard.SetText(clipboardMessage);
        }

        #endregion

        #region Helper functions

        private void HideDetails()
        {
            _detailTree.Hide();
        	_detailButton.Text = SR.MenuShowDetails;

            // Shrink the user control
			Height -= _detailTree.Height;
        }

        private void ShowDetails()
        {
            _detailTree.Show();
			_detailButton.Text = SR.MenuHideDetails;

            // Expand the user control
			Height += _detailTree.Height;
        }

        private void BuildTreeFromException(TreeNode thisNode, Exception e)
        {
            // Special case for the root node
            if (thisNode == null)
                thisNode = _detailTree.Nodes.Add(e.Message);
            else
                thisNode.Nodes.Add(e.Message);

            if (e.StackTrace != null)
            {
                // Add a new node for each level of StackTrace
                string lineBreak = Environment.NewLine;
                int prevIndex = 0;
                int startIndex = e.StackTrace.IndexOf(lineBreak, prevIndex);
                while (startIndex != -1)
                {
                    thisNode.Nodes.Add(e.StackTrace.Substring(prevIndex, startIndex - prevIndex));
                    prevIndex = startIndex + lineBreak.Length;
                    startIndex = e.StackTrace.IndexOf(lineBreak, prevIndex);
                }
                thisNode.Nodes.Add(e.StackTrace.Substring(prevIndex));
            }

            // Recursively add inner exception to the tree
            if (e.InnerException != null)
            {
                TreeNode childNode = thisNode.Nodes.Add("InnerException");
                BuildTreeFromException(childNode, e.InnerException);
            }
        }

        private static string BuildMessageFromException(Exception e)
        {
        	var sb = new StringBuilder();

			sb.AppendLine(!string.IsNullOrEmpty(e.Source) ? string.Format(SR.FormatExceptionDetails, e.Source, e.Message) : e.Message);
        	sb.AppendLine(e.StackTrace);

            // Recursively add inner exception to the message
            if (e.InnerException != null)
            {
            	sb.AppendLine();
				sb.AppendLine(SR.ExceptionHandlerInnerExceptionText);
            	sb.AppendLine(BuildMessageFromException(e.InnerException));
            }

            return sb.ToString();
        }

        #endregion
    }
}
