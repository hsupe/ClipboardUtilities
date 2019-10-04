using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ClipboardUtilities.Lib
{
	public class Sql
	{
		public string AssignValuesToVariables(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			input = input.Replace(Environment.NewLine, "\n");

			var lines = input.Split('\n');
			var variables = lines[0].Split('\t');
			var values = lines[1].Split('\t');

			var sb = new StringBuilder();

			for (var i = 0; i < variables.Length; i++)
			{
				sb.AppendLine(IsNumberOrNull(values[i])
					? $"\t{variables[i]} = {values[i]},"
					: $"\t{variables[i]} = '{values[i]}',");
			}

			var output = ConvertTabsTo4Spaces("select " + Environment.NewLine + sb.ToString().TrimEnd().TrimEnd(','));
			return AlignOnAssignmentOperator(output);
		}

		private static string AlignOnAssignmentOperator(string output)
		{
			return new TextAligner(output, "=").Align();
		}

		private bool IsNumberOrNull(string input)
		{
			return
				Regex.IsMatch(input, @"^-?[0-9\.]+")
				&& !input.Contains(":")
				|| String.Compare(input.Trim(), "null", StringComparison.OrdinalIgnoreCase) == 0;
		}

		private string ConvertTabsTo4Spaces(string input)
		{
			return new TabToSpacesConvertor(4).Convert(input);
		}
	}
}