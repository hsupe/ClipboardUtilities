using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ClipboardUtilities.Lib
{
	public class StringUtilities : IStringUtilities
	{
		public string Trim(string input)
		{
			//TODO Refactor this pattern, it is repeating several times.
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

		private string ToSqlInList(string input, bool includeValuesInQuotes)
		{
			var pattern = includeValuesInQuotes ? "'$$VAL$$'," : "$$VAL$$,";
			input = pattern + Environment.NewLine + input;
			return ApplyPattern(input)
				.RemoveTrailing(",")
				.SurroundWith("(", ")")
				.ToSingleLine();
		}

		public string ToSqlInList(string input)
		{
			return ToSqlInList(input, false);
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
			var pattern = lines.First();
			return lines.Skip(1)
				.Select(x => pattern.Replace("$$VAL$$", x))
				.ToMultiLineString();
		}

		public string ExtractPattern(string input)
		{
			IEnumerable<string> lines = input.SplitInputIntoLines();
			var pattern = lines.First();
			return lines.Skip(1)
				.Select(x =>
					{
						var m = Regex.Match(x, pattern);
						return m.Success ? m.Value : string.Empty;
					}
				)
				.ToMultiLineString();
		}

		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public string LogDateToSplunkDate(string logDate)
		{
			var array = logDate.ToPlainTextSingleSpaceSingleLine().Split('-', ':', '.', ' ');
			var Y = array[0];
			var M = array[1];
			var D = array[2];
			var H = array[3];
			var Min = array[4];
			var S = array[5];
			return $"\"{M}/{D}/{Y}:{H}:{Min}:{S}\"";
		}

		public string FormatXml(string input)
		{
			try
			{
				input = input.RemoveLeading("xml=");
				return XDocument.Parse(input).ToString();
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}

		public string DefineCSharpByteArray(string input)
		{
			try
			{
				var array = StringToByte(input);

				if (array.Length == 0)
					return string.Empty;

				var sb = new StringBuilder();

				var count = 0;
				foreach (var b in array)
				{
					sb.Append($"0x{b:X2}, ");
					count++;
					if (count % 20 == 0)
						sb.AppendLine();
				}

				var arrayDefinition = sb.ToString()
					.TrimEnd(',', ' ', '\r', '\n');

				return string.Format("byte[] arrayName = {{{0}{1}{0}}};", Environment.NewLine, arrayDefinition);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

		public string RemoveEmptyLines(string input)
		{
			return Regex.Replace(input, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
		}

		public string ToSplunkOr(string input)
		{
			var pattern = "$$VAL$$ OR";
			input = pattern + Environment.NewLine + input;
			return ApplyPattern(input)
				.RemoveTrailing("OR")
				.SurroundWith("(", ")")
				.ToSingleLine();
		}

		public string ToSingleToLine(string input)
		{
			return input.ToSingleLine();
		}

		public string ToSqlSelectAs(string input)
		{
			var pattern = "    $$VAL$$ as '$$VAL$$',";
			input = pattern + Environment.NewLine + input;
			return "select" + Environment.NewLine
			                + ApplyPattern(input)
				                .RemoveTrailing(",");
		}

		public string AssignValuesToVariables(string input)
		{
			return new Sql().AssignValuesToVariables(input);
		}

		private static byte[] StringToByte(string hexSequence)
		{
			hexSequence = hexSequence.Trim().RemoveLeading("0x");

			if (hexSequence.Length % 2 > 0)
				throw new Exception("The provided byte sequence has odd number of characters, which makes it invalid");

			return Enumerable.Range(0, hexSequence.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hexSequence.Substring(x, 2), 16))
				.ToArray();
		}
	}
}