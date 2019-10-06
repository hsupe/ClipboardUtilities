// This code is borrowed from my another repository: https://raw.githubusercontent.com/hlsupe/TabToSpacesConvertor/master/TabToSpacesConvertor/TabToSpacesConvertor.cs
using System.Text;

namespace ClipboardUtilities.Lib
{
	public class TabToSpacesConvertor
	{
		public TabToSpacesConvertor(int tabLength) => TabLength = tabLength;

		public string Convert(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			StringBuilder output = new StringBuilder();
			int positionInOutput = 1;
			foreach (char c in input)
				switch (c)
				{
					case '\t':
						positionInOutput = ReplaceTabBySpaces(positionInOutput, output);
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

		private int ReplaceTabBySpaces(int positionInOutput, StringBuilder output)
		{
			int spacesToAdd = NumberOfSpacesToTabStop(positionInOutput) - positionInOutput;
			output.Append(RepeatSpaces(spacesToAdd));
			positionInOutput += spacesToAdd;
			return positionInOutput;
		}

		private int NumberOfSpacesToTabStop(int currentPosition)
		{
			if (AtTabStop(currentPosition))
				return GetNextTabStop(currentPosition);
			return GetNearestTabStop(currentPosition);
		}

		private bool AtTabStop(int currentPosition) => currentPosition % TabLength == 1;

		private int GetNextTabStop(int currentPosition) => currentPosition + TabLength;

		private int GetNearestTabStop(int currentPosition)
		{
			for (int i = 0; i < TabLength; i++, currentPosition++)
				if (AtTabStop(currentPosition))
					break;
			return currentPosition;
		}

		private string RepeatSpaces(int spacesToAdd) => new string(' ', spacesToAdd);

		private int TabLength { get; }
	}
}