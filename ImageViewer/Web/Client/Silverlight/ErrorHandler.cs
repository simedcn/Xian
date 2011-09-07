﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.ImageViewer.Web.Client.Silverlight.Helpers;
using ClearCanvas.ImageViewer.Web.Client.Silverlight.Resources;
using ClearCanvas.Web.Client.Silverlight.Utilities;

namespace ClearCanvas.ImageViewer.Web.Client.Silverlight
{
    public static class ErrorHandler
    {
        public static event EventHandler OnCriticalError;

        public static void HandleCriticalError(string message, params object[] args)
        {
            UIThread.Execute(() =>
            {
                if (OnCriticalError!=null)
                    OnCriticalError(null, EventArgs.Empty);

                var window= PopupHelper.PopupMessage(DialogTitles.Error, string.Format(message, args));
            });
        }

        public static void HandleException(Exception ex)
        {
            UIThread.Execute(() =>
            {
                if (OnCriticalError != null)
                    OnCriticalError(null, EventArgs.Empty);

                var window = PopupHelper.PopupMessage(DialogTitles.Error, ex.Message);
            });
        }
    }
}
