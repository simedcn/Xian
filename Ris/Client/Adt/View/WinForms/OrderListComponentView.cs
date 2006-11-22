using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.View.WinForms;

namespace ClearCanvas.Ris.Client.Adt.View.WinForms
{
    /// <summary>
    /// Provides a Windows Forms view onto <see cref="OrderListComponent"/>
    /// </summary>
    [ExtensionOf(typeof(OrderListComponentViewExtensionPoint))]
    public class OrderListComponentView : WinFormsView, IApplicationComponentView
    {
        private OrderListComponent _component;
        private OrderListComponentControl _control;


        #region IApplicationComponentView Members

        public void SetComponent(IApplicationComponent component)
        {
            _component = (OrderListComponent)component;
        }

        #endregion

        public override object GuiElement
        {
            get
            {
                if (_control == null)
                {
                    _control = new OrderListComponentControl(_component);
                }
                return _control;
            }
        }
    }
}
