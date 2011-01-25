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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.Utilities.Manifest;

namespace ClearCanvas.Desktop.Help
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			this.SuspendLayout();

			InitializeComponent();

			_version.Text = String.Format(AboutSettings.Default.VersionTextFormat, ProductInformation.GetVersion(true, true, true));
			_copyright.Text = ProductInformation.Copyright;
			_license.Text = ProductInformation.License;

            _manifest.Visible = !ManifestVerification.Valid;
           
			if (AboutSettings.Default.UseSettings)
			{
				try
				{
					Assembly assembly = Assembly.Load(AboutSettings.Default.BackgroundImageAssemblyName);
					if (assembly != null)
					{
						string streamName = AboutSettings.Default.BackgroundImageAssemblyName + "." + AboutSettings.Default.BackgroundImageResourceName;
						Stream stream = assembly.GetManifestResourceStream(streamName);
						if (stream != null)
						{
							this.BackgroundImage = new Bitmap(stream);
							ClientSize = this.BackgroundImage.Size;
						}
					}
				}
				catch (Exception ex)
				{
					Platform.Log(LogLevel.Warn, ex, "Failed to resolve about dialog resources.");
				}

				this._copyright.Location = AboutSettings.Default.CopyrightLocation;
				this._copyright.Size = AboutSettings.Default.CopyrightSize;
				this._copyright.AutoSize = AboutSettings.Default.CopyrightAutoSize;
				this._copyright.ForeColor = AboutSettings.Default.CopyrightForeColor;
				this._copyright.Font = AboutSettings.Default.CopyrightFontBold ? new Font(this._copyright.Font, FontStyle.Bold) : this._copyright.Font;
				this._copyright.TextAlign = AboutSettings.Default.CopyrightTextAlign;

				this._version.Location = AboutSettings.Default.VersionLocation;
				this._version.Size = AboutSettings.Default.VersionSize;
				this._version.AutoSize = AboutSettings.Default.VersionAutoSize;
				this._version.ForeColor = AboutSettings.Default.VersionForeColor;
				this._version.Font = AboutSettings.Default.VersionFontBold ? new Font(this._version.Font, FontStyle.Bold) : this._version.Font;
				this._version.TextAlign = AboutSettings.Default.VersionTextAlign;

				this._license.Visible = AboutSettings.Default.LicenseVisible;
				this._license.Location = AboutSettings.Default.LicenseLocation;
				this._license.Size = AboutSettings.Default.LicenseSize;
				this._license.AutoSize = AboutSettings.Default.LicenseAutoSize;
				this._license.ForeColor = AboutSettings.Default.LicenseForeColor;
				this._license.Font = AboutSettings.Default.LicenseFontBold ? new Font(this._license.Font, FontStyle.Bold) : this._license.Font;
				this._license.TextAlign = AboutSettings.Default.LicenseTextAlign;

                this._manifest.Location = AboutSettings.Default.ManifestLocation;
                this._manifest.Size = AboutSettings.Default.ManifestSize;
                this._manifest.AutoSize = AboutSettings.Default.ManifestAutoSize;
                this._manifest.ForeColor = AboutSettings.Default.ManifestForeColor;
                this._manifest.Font = AboutSettings.Default.ManifestFontBold ? new Font(this._manifest.Font, FontStyle.Bold) : this._license.Font;
                this._manifest.TextAlign = AboutSettings.Default.ManifestTextAlign;

				this._closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
				this._closeButton.LinkColor = AboutSettings.Default.CloseButtonLinkColor;
			}

			this.ResumeLayout();

			this._closeButton.Click += new EventHandler(OnCloseClicked);
		}

		private void OnCloseClicked(object sender, EventArgs e)
		{
			Close();
		}
	}
}