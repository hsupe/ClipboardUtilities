using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;


namespace ClipboardUtilities.Lib
{
	public interface IClipboardUtilities
	{
		string Trim(string input);
		string Sort(string input);
		string Reverse(string input);
		string RemoveDuplicates(string input);
		string ConvertDecimalTo8BytesLowercaseHex(string input);
		string ConvertHexToDecimal(string input);
		string ToSqlInList(string input, bool includeValuesInQuotes = false);
		string ToSqlInListQuoted(string input);
		string IpAddressToHexNumber(string input);
		string HexToIpAddress(string input);
		string ApplyPattern(string input);
		string ExtractPattern(string input);
		string LogDateToSplunkDate(string logDate);
	}

	public class ClipboardUtilities : IClipboardUtilities
	{
		public string Trim(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => x.Trim())
				.ToMultiLineString();
		}

		public string Sort(string input)
		{
			return input.SplitInputIntoLines()
				.OrderBy(x => x)
				.ToMultiLineString();
		}

		public string Reverse(string input)
		{
			return input.SplitInputIntoLines()
				.Reverse()
				.ToMultiLineString();
		}

		public string RemoveDuplicates(string input)
		{
			return input.SplitInputIntoLines()
				.Distinct()
				.ToMultiLineString();
		}

		public string ConvertDecimalTo8BytesLowercaseHex(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => $"{long.Parse(x):x8}")
				.ToMultiLineString();
		}

		public string ConvertHexToDecimal(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => Convert.ToUInt64(x, 16))
				.ToMultiLineString();
		}

		public string ToSqlInList(string input, bool includeValuesInQuotes = false)
		{
			string pattern = includeValuesInQuotes ? "'$$VAL$$'," : "$$VAL$$,";
			input = pattern + Environment.NewLine + input;
			return ApplyPattern(input)
				.TrimEnd(',')
				.SurroundWith("(", ")")
				.ToSingleLine();
		}

		public string ToSqlInListQuoted(string input)
		{
			return ToSqlInList(input, true);
		}

		public string IpAddressToHexNumber(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => new IpAddress().ToHexNumberAsString(x))
				.ToMultiLineString();
		}

		public string HexToIpAddress(string input)
		{
			return input.SplitInputIntoLines()
				.Select(x => new IpAddress().ToString(x))
				.ToMultiLineString();
		}

		public string ApplyPattern(string input)
		{
			IEnumerable<string> lines = input.SplitInputIntoLines();
			string pattern = lines.First();
			return lines.Skip(1)
				.Select(x => pattern.Replace("$$VAL$$", x))
				.ToMultiLineString();
		}

		public string ExtractPattern(string input)
		{
			IEnumerable<string> lines = input.SplitInputIntoLines();
			string pattern = lines.First();
			return lines.Skip(1)
				.Select(x => 
							{
								Match m = Regex.Match(x, pattern);
								return m.Success ? m.Value : string.Empty;
							}
					)
				.ToMultiLineString();
		}

		public string LogDateToSplunkDate(string logDate)
		{
			string[] array = logDate.ToPlainTextSingleSpaceSingleLine().Split(new[] { '-', ':', '.', ' ' });
			string Y = array[0];
			string M = array[1];
			string D = array[2];
			string H = array[3];
			string Min = array[4];
			string S = array[5];

			return $"\"{M}/{D}/{Y}:{H}:{Min}:{S}\"";
		}
	}

	internal static class ExtensionMethods
	{
		public static string[] SplitInputIntoLines(this string input)
		{
			return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		}
		public static string ToPlainTextSingleSpaceSingleLine(this string input)
		{
			return Regex.Replace(input, @"\s+", " ");
		}

		public static string ToSingleLine(this string input)
		{
			return Regex.Replace(input, Environment.NewLine, " ");
		}

		public static string ToMultiLineString<T>(this IEnumerable<T> lines)
		{
			return JoinIntoString(lines, Environment.NewLine);
		}

		public static string JoinIntoString<T>(this IEnumerable<T> lines, string separator)
		{
			return string.Join(separator, lines);
		}

		public static string SurroundWith(this string input, string begin, string end)
		{
			return JoinIntoString(
				new List<string>() { begin, input, end },
				" ");
		}
	}
}