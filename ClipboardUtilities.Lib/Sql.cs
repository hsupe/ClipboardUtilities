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

		private static bool IsNumberOrNull(string input) => IsNumber(input) || IsNullConstant(input);

		private static bool IsNumber(string input) =>
			Regex.IsMatch(input, @"^-?[0-9\.]+")
			&& !input.Contains(":"); //// TODO can this : be handled in regex? Does it exclude dates?
                            
		private static bool IsNullConstant(string input) => String.Compare(input.Trim(), "null", StringComparison.OrdinalIgnoreCase) == 0;

		private static string ConvertTabsTo4Spaces(string input) => new TabToSpacesConvertor(4).Convert(input);
		private static string AlignOnAssignmentOperator(string output) => new TextAligner(output, "=").Align();
	}
}