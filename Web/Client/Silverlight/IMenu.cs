#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections;

namespace ClearCanvas.Web.Client.Silverlight
{
    public interface IMenuItem : IMenu
    {
        IMenuItem ParentMenuItem { get; }

        bool IsTopLevel { get; }

        bool IsChecked { get; set; }
        ImageSource Icon { get; set; }

        bool IsHighlighted { get; set; }

        bool IsExpanded { get; set; }

        event EventHandler Click;
    }

    public interface IMenu
    {
        bool IsRoot { get; }
        MenuBase RootMenu { get; }
        IMenu ParentMenu { get; set; }

        bool HasItems { get; }
        IList Items { get; }

        IMenuItemCoordinator ItemCoordinator { get; }
        FrameworkElement ItemsContainer { get; }
    }
}
