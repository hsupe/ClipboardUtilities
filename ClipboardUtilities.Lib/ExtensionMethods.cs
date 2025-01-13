using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClipboardUtilities.Lib;

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
			new List<string> { begin, input, end },
			" ");
	}

	public static string ReplaceUsingRegEx(this string input, string pattern, string replacement,
		RegexOptions options = RegexOptions.None)
	{
		options |= RegexOptions.Multiline;
		return Regex.Replace(input, pattern, replacement, options);
	}

	public static string RemoveTrailing(this string input, string pattern)
	{
		return input
			.Trim()
			.TrimEnd(' ', '\r', '\n')
			.ReplaceUsingRegEx(pattern + "$", string.Empty, RegexOptions.IgnoreCase)
			.TrimEnd(' ', '\r', '\n');
	}

	public static string RemoveLeading(this string input, string pattern)
	{
		return input
			.TrimStart()
			.ReplaceUsingRegEx("^" + pattern, string.Empty, RegexOptions.IgnoreCase)
			.TrimStart();
	}
}