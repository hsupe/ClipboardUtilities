using System;
using System.Windows.Forms;
namespace HandyManny
{
	//TODO Rename ClipboardUtilities and HandyManny
	public class ActionManager
	{
		private readonly ClipboardUtilities.Lib.ClipboardUtilities _clipboardUtilities;

		public ActionManager() => _clipboardUtilities = new ClipboardUtilities.Lib.ClipboardUtilities();

		public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
		{
			contextMenuStrip.Items.Clear();
			contextMenuStrip.Items.AddRange(
				// TODO Can this be reduced to an array and loop
				new ToolStripItem[] {
					ToolStripMenuItemWithHandler("IpAddress To Hex Number",  IpAddressToHex),
                    ToolStripMenuItemWithHandler("Hex To IpAddress",  HexToIpAddress),
					new ToolStripSeparator(),

                    ToolStripMenuItemWithHandler("Format Xml", FormatXml),
					ToolStripMenuItemWithHandler("Define C# Byte Array", DefineCSharpByteArray),
					new ToolStripSeparator(),
                 
                    ToolStripMenuItemWithHandler("Decimal to 8 Bytes Lowercase Hex", DecimalTo8BytesLowercaseHex),
                    ToolStripMenuItemWithHandler("Hex to Decimal", HexToDecimal),
                    new ToolStripSeparator(),
					
					ToolStripMenuItemWithHandler("Trim White-spaces", TrimWhitespaces),
                    ToolStripMenuItemWithHandler("Remove Empty Lines", RemoveEmptyLines),
                    ToolStripMenuItemWithHandler("Remove Duplicates", RemoveDuplicates),
                    new ToolStripSeparator(),

					ToolStripMenuItemWithHandler("Sort Ascending", Sort),
                    ToolStripMenuItemWithHandler("Reverse the Order", ReverseTheOrder),
                    new ToolStripSeparator(),

					ToolStripMenuItemWithHandler("To Sql Select...As", ToSqlSelectAs),
					ToolStripMenuItemWithHandler("Assign values to variables", AssignValuesToVariables),
                    ToolStripMenuItemWithHandler("To Sql IN List", ToSqlInListUnquoted),
                    ToolStripMenuItemWithHandler("To Sql IN List - Quoted", ToSqlInListQuoted),
					new ToolStripSeparator(),

                    ToolStripMenuItemWithHandler("Apply Pattern", ApplyPattern),
                    ToolStripMenuItemWithHandler("Extract Pattern", ExtractPattern),
					new ToolStripSeparator(),
                    
                    ToolStripMenuItemWithHandler("To Plain Text - Keep Lines",  ToPlainTextLineKeepLines),
					ToolStripMenuItemWithHandler("To Plain Text - Make Single Space Single Line", ToPlainTextSingleSpaceSingleLine),
                    new ToolStripSeparator(),

                    ToolStripMenuItemWithHandler("To Splunk OR", ToSplunkOr),
					ToolStripMenuItemWithHandler("Log Date to Splunk Date", LogDateToSplunkDate),
				});
		}

		//TODO can these methods be converted to single method(object sender, EventArgs e) with a switch to identify the delegate and call it at the end.
		private void IpAddressToHex(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.IpAddressToHexNumber(MyClipboard.Text);

		private void HexToIpAddress(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.HexToIpAddress(MyClipboard.Text);

		private void ToPlainTextLineKeepLines(object sender, EventArgs e) => MyClipboard.Text = MyClipboard.Text;

		private void ToPlainTextSingleSpaceSingleLine(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ToSingleToLine(MyClipboard.Text);

		private void ToSqlSelectAs(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ToSqlSelectAs(MyClipboard.Text);

		private void AssignValuesToVariables(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.AssignValuesToVariables(MyClipboard.Text);

		private void ToSqlInListUnquoted(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ToSqlInList(MyClipboard.Text);

		private void ToSqlInListQuoted(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ToSqlInListQuoted(MyClipboard.Text);

		private void DecimalTo8BytesLowercaseHex(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ConvertDecimalTo8BytesLowercaseHex(MyClipboard.Text);

		private void HexToDecimal(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ConvertHexToDecimal(MyClipboard.Text);

		private void RemoveDuplicates(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.RemoveDuplicates(MyClipboard.Text);

		private void Sort(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.Sort(MyClipboard.Text);

		private void ReverseTheOrder(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.Reverse(MyClipboard.Text);

		private void ApplyPattern(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ApplyPattern(MyClipboard.Text);

		private void ExtractPattern(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ExtractPattern(MyClipboard.Text);

		private void FormatXml(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.FormatXml(MyClipboard.Text);

		private void LogDateToSplunkDate(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.LogDateToSplunkDate(MyClipboard.Text);

		private void ToSplunkOr(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.ToSplunkOr(MyClipboard.Text);

		private void TrimWhitespaces(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.Trim(MyClipboard.Text);

		private void RemoveEmptyLines(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.RemoveEmptyLines(MyClipboard.Text);

		private void DefineCSharpByteArray(object sender, EventArgs e) => MyClipboard.Text = _clipboardUtilities.DefineCSharpByteArray(MyClipboard.Text);

		public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
		{
			var item = new ToolStripMenuItem(displayText);
			if (eventHandler != null) { item.Click += eventHandler; }
			return item;
		}
	}
}
