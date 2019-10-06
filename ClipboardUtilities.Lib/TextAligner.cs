using System;
using System.Linq;

namespace ClipboardUtilities.Lib
{
	public class TextAligner
	{
		private readonly string _input;
		private readonly string _keyword;
		private string[] _lines;

		public TextAligner(string input, string keyword)
		{
			_input = input;
			_keyword = keyword;
		}

		public string Align()
		{
			if (string.IsNullOrEmpty(_input))
				return _input;

			_lines = _input.Replace("\r\n", "\n")
							.Split('\n');

			var rightMostPosition = FindRightMostPositionOfKeywordInLines();
			InsertSpacesToAlignToPosition(rightMostPosition);

			return string.Join(Environment.NewLine, _lines);
		}

		private int FindRightMostPositionOfKeywordInLines() => _lines.Select(line => line.IndexOf(_keyword, StringComparison.Ordinal)).Concat(new[] { 0 }).Max();

		private void InsertSpacesToAlignToPosition(int alignToPosition)
		{
			for (var i = 0; i < _lines.Length; i++)
			{
				var currentPosition = _lines[i].IndexOf(_keyword, StringComparison.Ordinal);

				if (currentPosition <= 0) continue;

				var spacesToInsert = alignToPosition - currentPosition;
				_lines[i] = _lines[i].Insert(currentPosition, new string(' ', spacesToInsert));
			}
		}
	}
}