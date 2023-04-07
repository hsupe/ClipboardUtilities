namespace ClipboardUtilities.Lib
{
    public class Preprocess
    {
        public string Invoke(ActionDelegate actionDelegate, string input)
        {
            var cleaned = input.Trim();
            var result = actionDelegate(cleaned);
            return result.Trim();
        }
    }
}