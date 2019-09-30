using System;
using System.Collections.Generic;
using System.Linq;

namespace ClipboardUtilities.Lib
{
	public interface IClipboardUtilities
	{
		string Trim(string input);
		string Sort(string input);
	}

	internal static class ExtensionMethods
	{
		public static string[] SplitInputIntoLines(this string input)
		{
			return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		}

		public static string JoinIntoString<T>(this IEnumerable<T> lines)
		{
			return string.Join(Environment.NewLine, lines);
		}
	}

	public class ClipboardUtilities : IClipboardUtilities
	{
		public string Trim(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => x.Trim())
				.JoinIntoString();
		}

		public string Sort(string input)
		{
			return input.SplitInputIntoLines()
				.OrderBy(x => x)
				.JoinIntoString();
		}
	}
}