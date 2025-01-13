namespace ClipboardUtilities.Lib;

public interface IStringUtilities
{
	string Trim(string input);
	string Sort(string input);
	string Reverse(string input);
	string RemoveDuplicates(string input);
	string ConvertDecimalTo8BytesLowercaseHex(string input);
	string ConvertHexToDecimal(string input);
	string ToSqlInList(string input);
	string ToSqlInListQuoted(string input);
	string IpAddressToHexNumber(string input);
	string HexToIpAddress(string input);
	string ApplyPattern(string input);
	string ExtractPattern(string input);
	string LogDateToSplunkDate(string logDate);
	string FormatXml(string input);
	string DefineCSharpByteArray(string hexSequence);
	string RemoveEmptyLines(string input);
	string ToSplunkOr(string input);
	string ToSingleLine(string input);
	string ToSqlSelectAs(string input);
	string AssignValuesToVariables(string input);
	string ConvertPathFromWindowsToWsl(string input);
}