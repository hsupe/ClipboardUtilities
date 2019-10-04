using System.Windows.Forms;

namespace ClipboardUtilities.UI
{
	public class MyClipboard
    {
        public static string Text
        {
            get => new ClipboardUtilities.Lib.ClipboardUtilities().RemoveEmptyLines(Clipboard.GetText(TextDataFormat.Text));
            set => Clipboard.SetText(value, TextDataFormat.Text);
        }
    }
}
