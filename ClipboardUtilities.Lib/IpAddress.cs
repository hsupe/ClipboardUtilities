using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;

namespace ClipboardUtilities.Lib
{
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
			return IPAddress.Parse(ipAddress).GetAddressBytes();
		}

		public string ToString(byte[] ipAddress)
		{
			IPAddress dotNetIpAddress = new IPAddress(ipAddress);

			if (dotNetIpAddress.AddressFamily == AddressFamily.InterNetwork)
				return dotNetIpAddress.MapToIPv4().ToString();
			else if (dotNetIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
				return dotNetIpAddress.MapToIPv6().ToString();
			else
				throw new ArgumentException(
					$"Ip address belongs to {dotNetIpAddress.AddressFamily} which is not supported.");
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
	}
}