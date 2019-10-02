﻿using System;
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
				if (IsNumberOrNull(values[i]))
					sb.AppendLine(string.Format("\t{0} = {1},", variables[i], values[i]));
				else // enclose in quotes
					sb.AppendLine(string.Format("\t{0} = '{1}',", variables[i], values[i]));

			var output = ConvertTabsTo4Spaces("select " + Environment.NewLine + sb.ToString().TrimEnd().TrimEnd(','));
			return AlignOn(output, "=");
		}

		private bool IsNumberOrNull(string input)
		{
			return
				Regex.IsMatch(input, @"^-?[0-9\.]+")
				&& !input.Contains(":")
				|| string.Compare(input.Trim(), "null", true /*ignore case */) == 0;
		}

		private string AlignOn(string input, string keyword)
		{
			if (string.IsNullOrEmpty(input))
				return input;


			var lines = input.Replace("\r\n", "\n").Split('\n');

			var rightMostPoisition = FindRightMostPositionOfKeywordInLines(lines, keyword);
			InsertSpacesToAlignToPosition(lines, keyword, rightMostPoisition);

			return string.Join(Environment.NewLine, lines);
		}

		private int FindRightMostPositionOfKeywordInLines(string[] lines, string keyword)
		{
			var rightMostPosition = 0;
			foreach (var line in lines)
			{
				var position = line.IndexOf(keyword);
				if (rightMostPosition < position)
					rightMostPosition = position;
			}

			return rightMostPosition;
		}

		private void InsertSpacesToAlignToPosition(string[] lines, string keyword, int alignToPosition)
		{
			for (var i = 0; i < lines.Length; i++)
			{
				var currentPosition = lines[i].IndexOf(keyword);
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