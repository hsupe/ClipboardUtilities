using System;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardUtilities.UI;

public class ActionManager
{
	private readonly ActionCatalog _catalog;

	public ActionManager(ActionCatalog catalog)
	{
		_catalog = catalog;
	}

	public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
	{
		contextMenuStrip.Items.Clear();

		var toolStripItems = _catalog.Actions().Select(item => ToolStripMenuItemWithHandler(item, ItemHandler))
			.Cast<ToolStripItem>().ToArray();
		contextMenuStrip.Items.AddRange(toolStripItems);
	}

	private void ItemHandler(object sender, EventArgs e)
	{
		Clipboard.Text = _catalog.InvokeAction(GetMethod(), Clipboard.Text);

		string GetMethod()
		{
			return ((ToolStripItem.ToolStripItemAccessibleObject)((ToolStripItem)sender).AccessibilityObject).Name;
		}
	}

	public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
	{
		var item = new ToolStripMenuItem(displayText);
		if (eventHandler != null) item.Click += eventHandler;

		return item;
	}
}