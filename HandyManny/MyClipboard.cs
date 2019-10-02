using System.Windows.Forms;

namespace HandyManny
{
	public class MyClipboard
    {
        public static string Text
        {
            get => new ClipboardUtilities.Lib.ClipboardUtilities().RemoveEmptyLines(Clipboard.GetText(TextDataFormat.Text));
            set => Clipboard.SetText(value, TextDataFormat.Text);
        }

        public static string GetWithOutRemovingEmptyLines()
        {
            return Clipboard.GetText(TextDataFormat.Text);
        }
    }
}
