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
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.ImageViewer.Common.DicomServer;
using ClearCanvas.ImageViewer.Common.ServerDirectory;

namespace ClearCanvas.ImageViewer.Configuration.ServerTree
{
	[ButtonAction("activate", "servertree-toolbar/ToolbarVerify", "VerifyServer")]
	[MenuAction("activate", "servertree-contextmenu/MenuVerify", "VerifyServer")]
	[EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[Tooltip("activate", "TooltipVerify")]
	[IconSet("activate", "Icons.VerifyServerToolSmall.png", "Icons.VerifyServerToolMedium.png", "Icons.VerifyServerToolLarge.png")]
	[ExtensionOf(typeof(ServerTreeToolExtensionPoint))]
	public class VerifyServerTool : ServerTreeTool
	{
		private bool NoServersSelected()
		{
			return this.Context.SelectedServers == null || this.Context.SelectedServers.Count == 0;
		}

		private void VerifyServer()
		{
			BlockingOperation.Run(this.InternalVerifyServer);
		}

		private void InternalVerifyServer()
		{
			if (this.NoServersSelected())
			{
				//should never get here because the verify button should be disabled.
				this.Context.DesktopWindow.ShowMessageBox(SR.MessageNoServersSelected, MessageBoxActions.Ok);
				return;
			}

		    try
		    {
		        var localServer = ServerDirectory.GetLocalServer();

                var msgText = new StringBuilder();
                msgText.AppendFormat(SR.MessageCEchoVerificationPrefix + "\r\n\r\n");
                foreach (var server in this.Context.SelectedServers)
                {
                    using (var scu = new VerificationScu())
                    {
                        VerificationResult result = scu.Verify(localServer.AETitle, server.AETitle, server.ScpParameters.HostName, server.ScpParameters.Port);
                        if (result == VerificationResult.Success)
                            msgText.AppendFormat(SR.MessageCEchoVerificationSingleServerResultSuccess + "\r\n", server.Name);
                        else
                            msgText.AppendFormat(SR.MessageCEchoVerificationSingleServerResultFail + "\r\n", server.Name);

                        // must wait for the SCU thread to release the connection properly before disposal, otherwise we might end up aborting the connection instead
                        scu.Join(new TimeSpan(0, 0, 2));
                    }
                }

                msgText.AppendFormat("\r\n");
                this.Context.DesktopWindow.ShowMessageBox(msgText.ToString(), MessageBoxActions.Ok);
		    }
		    catch (Exception e)
		    {
                ExceptionHandler.Report(e, base.Context.DesktopWindow);
		    }
		}

		protected override void OnSelectedServerChanged(object sender, EventArgs e)
		{
			this.Enabled = !this.Context.SelectedServers.IsLocalServer && !this.NoServersSelected();                 
		}
	}
}