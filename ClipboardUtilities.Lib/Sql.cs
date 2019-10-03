using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ClipboardUtilities.Lib
{
	// TODO Clean up
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
				sb.AppendLine(IsNumberOrNull(values[i])
					? $"\t{variables[i]} = {values[i]},"
					: $"\t{variables[i]} = '{values[i]}',");

			var output = ConvertTabsTo4Spaces("select " + Environment.NewLine + sb.ToString().TrimEnd().TrimEnd(','));
			return AlignOn(output, "=");
		}

		private bool IsNumberOrNull(string input)
		{
			return
				Regex.IsMatch(input, @"^-?[0-9\.]+")
				&& !input.Contains(":")
				|| String.Compare(input.Trim(), "null", StringComparison.OrdinalIgnoreCase) == 0;
		}

		//TODO Refactor AlignOn
		private string AlignOn(string input, string keyword)
		{
			if (string.IsNullOrEmpty(input))
				return input;


			var lines = input.Replace("\r\n", "\n").Split('\n');

			var rightMostPosition = FindRightMostPositionOfKeywordInLines(lines, keyword);
			InsertSpacesToAlignToPosition(lines, keyword, rightMostPosition);

			return string.Join(Environment.NewLine, lines);
		}

		private int FindRightMostPositionOfKeywordInLines(string[] lines, string keyword)
		{
			var rightMostPosition = 0;
			foreach (var line in lines)
			{
				var position = line.IndexOf(keyword, StringComparison.Ordinal);
				if (rightMostPosition < position)
					rightMostPosition = position;
			}

			return rightMostPosition;
		}

		private void InsertSpacesToAlignToPosition(string[] lines, string keyword, int alignToPosition)
		{
			for (var i = 0; i < lines.Length; i++)
			{
				var currentPosition = lines[i].IndexOf(keyword, StringComparison.Ordinal);
				if (currentPosition > 0)
				{
					var spacesToInsert = alignToPosition - currentPosition;
					lines[i] = lines[i].Insert(currentPosition, new string(' ', spacesToInsert));
				}
			}
		}

		private int GetNearestTabStop(int currentPosition, int tabLength)
		{
			// if already at the tab stop, jump to next tab stop.
			if (currentPosition % tabLength == 1)
				currentPosition += tabLength;
			else
				// if in the middle of two tab stops, move forward to the nearest.
				for (var i = 0; i < tabLength; i++, currentPosition++)
					if (currentPosition % tabLength == 1)
						break;

			return currentPosition;
		}
		//TODO Refactor ConvertTabsTo4Spaces
		private string ConvertTabsTo4Spaces(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			var TabLength = 4;
			var output = new StringBuilder();

			var positionInOutput = 1;
			foreach (var c in input)
				switch (c)
				{
					case '\t':
						var spacesToAdd = GetNearestTabStop(positionInOutput, TabLength) - positionInOutput;
						output.Append(new string(' ', spacesToAdd));
						positionInOutput += spacesToAdd;
						break;

					case '\n':
						output.Append(c);
						positionInOutput = 1;
						break;

					default:
						output.Append(c);
						positionInOutput++;
						break;
				}

			return output.ToString();
		}
	}
}