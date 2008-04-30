using System;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Clipboard
{
	[MenuAction("delete", "clipboard-contextmenu/MenuDeleteClipboardItem", "Delete")]
	[ButtonAction("delete", "clipboard-toolbar/ToolbarDeleteClipboardItem", "Delete")]
	[Tooltip("delete", "TooltipDeleteClipboardItem")]
	[IconSet("delete", IconScheme.Colour, "Icons.DeleteClipboardItemToolSmall.png", "Icons.DeleteClipboardItemToolSmall.png", "Icons.DeleteClipboardItemToolSmall.png")]
	[EnabledStateObserver("delete", "Enabled", "EnabledChanged")]

	[ExtensionOf(typeof(ClipboardToolExtensionPoint))]
	public class DeleteClipboardItemTool : ClipboardTool
	{
		public DeleteClipboardItemTool()
		{
		}

		public override void Initialize()
		{
			this.Enabled = this.Context.SelectedClipboardItems.Count > 0;
			base.Initialize();
		}

		public void Delete()
		{
			bool anyLocked = false;

			foreach (IClipboardItem item in this.Context.SelectedClipboardItems)
			{
				if (item.Locked)
				{
					anyLocked = true;
				}
				else
				{
					if (item.Item is IDisposable)
						((IDisposable)item.Item).Dispose();

					this.Context.ClipboardItems.Remove(item);
				}
			}

			if (anyLocked)
				this.Context.DesktopWindow.ShowMessageBox(SR.MessageUnableToClearClipboardItems, MessageBoxActions.Ok);
		}
	}
}
