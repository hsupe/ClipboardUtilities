// This code is borrowed from my another repository: https://raw.githubusercontent.com/hlsupe/TabToSpacesConvertor/master/TabToSpacesConvertor/TabToSpacesConvertor.cs

using System.Text;

namespace ClipboardUtilities.Lib;

public class TabToSpacesConvertor
{
	public TabToSpacesConvertor(int tabLength)
	{
		TabLength = tabLength;
	}

	private int TabLength { get; }

	public string Convert(string input)
	{
		if (string.IsNullOrEmpty(input))
			return input;

		var output = new StringBuilder();
		var positionInOutput = 1;
		foreach (var c in input)
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
		var spacesToAdd = NumberOfSpacesToTabStop(positionInOutput) - positionInOutput;
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

	private bool AtTabStop(int currentPosition)
	{
		return currentPosition % TabLength == 1;
	}

	private int GetNextTabStop(int currentPosition)
	{
		return currentPosition + TabLength;
	}

	private int GetNearestTabStop(int currentPosition)
	{
		for (var i = 0; i < TabLength; i++, currentPosition++)
			if (AtTabStop(currentPosition))
				break;
		return currentPosition;
	}

	private string RepeatSpaces(int spacesToAdd)
	{
		return new string(' ', spacesToAdd);
	}
}