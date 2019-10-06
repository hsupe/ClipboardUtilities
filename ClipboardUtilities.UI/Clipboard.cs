using System.Text.RegularExpressions;
using System.Windows.Forms;
using SystemClipboard = System.Windows.Forms.Clipboard;

namespace ClipboardUtilities.UI
{
	public class Clipboard
	{
		public static string Text
		{
			get => RemoveEmptyLines(SystemClipboard.GetText(TextDataFormat.Text));
			set => SystemClipboard.SetText(value, TextDataFormat.Text);
		}
		public static string RemoveEmptyLines(string input) => Regex.Replace(input, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
	}
}