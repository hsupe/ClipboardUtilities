using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using DotNetIpAddress = System.Net.IPAddress;


namespace ClipboardUtilities.Lib
{
	public interface IClipboardUtilities
	{
		string Trim(string input);
		string Sort(string input);
		string Reverse(string input);
		string RemoveDuplicates(string input);
		string ConvertDecimalTo8BytesLowercaseHex(string input);
		string ConvertHexToDecimal(string input);
		string ToSqlInList(string input, bool includeValuesInQuotes = false);
		string ToSqlInListQuoted(string input);
		string IpAddressToHexNumber(string input);
		string HexToIpAddress(string input);
		string ApplyPattern(string input);
	}

	internal static class ExtensionMethods
	{
		public static string[] SplitInputIntoLines(this string input)
		{
			return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
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
				new List<string>() { begin, input, end}, 
				" ");
		}
	}

	public class ClipboardUtilities : IClipboardUtilities
	{
		public string Trim(string input)
		{
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

		public string ToSqlInList(string input, bool includeValuesInQuotes = false)
		{
			return input.SplitInputIntoLines()
				.Select(x => includeValuesInQuotes ? $"'{x}'," : $"{x},")
				.JoinIntoString(" ")
				.TrimEnd(',')
				.SurroundWith("(", ")");
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
			//input = input.Remove(0, input.IndexOf(System.Environment.NewLine));
			//return Converter(
			//	input,
			//	x => pattern.Replace("$$VAL$$", x)
			//);
			return input;
		}
	}

	public class IpAddress
	{
		public string ToHexNumberAsString(string ipAddress)
		{
			return ByteArrayToHexString(ToNumber(ipAddress));
		}

		public string ToString(string ipAddressAsHexNumber)
		{
			return ToString(HexStringToByteArray(ipAddressAsHexNumber));
		}

		public byte[] ToNumber(string ipAddress)
		{
			try
			{
				return DotNetIpAddress.Parse(ipAddress).GetAddressBytes();
			}
			catch (Exception e)
			{
				//log.InfoFormat($"{typeof(IpAddress).FullName}: {ipAddress}. {e}");
				throw;
			}
		}

		public string ToString(byte[] ipAddress)
		{
			try
			{
				DotNetIpAddress dotNetIpAddress = new DotNetIpAddress(ipAddress);

				if (dotNetIpAddress.AddressFamily == AddressFamily.InterNetwork)
					return dotNetIpAddress.MapToIPv4().ToString();
				else if (dotNetIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
					return dotNetIpAddress.MapToIPv6().ToString();
				else
					throw new ArgumentException(string.Format("Ip address belongs to {0} which is not supported.", dotNetIpAddress.AddressFamily));
			}
			catch (Exception e)
			{
				//log.InfoFormat(string"ByteArrayToHexString(ipAddress)}. {e}");
				throw;
			}
		}
		private static string ByteArrayToHexString(byte[] bytes)
		{
			return ("0x" + BitConverter.ToString(bytes).Replace("-", string.Empty)).ToUpper();
		}

		private byte[] HexStringToByteArray(string hexString)
		{
			hexString = Regex.Replace(hexString, "0x", string.Empty, RegexOptions.IgnoreCase);
			SoapHexBinary shb = SoapHexBinary.Parse(hexString);
			return shb.Value;
		}

		//private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(IpAddress));
	}
}