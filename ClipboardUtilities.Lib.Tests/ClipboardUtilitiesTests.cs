using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace ClipboardUtilities.Lib.Tests
{
	public class ClipboardUtilitiesTests
	{
		private IClipboardUtilities _sut = new ClipboardUtilities();

		[Fact]
		public void TrimShouldTrimLeadingAndEndingWhitespaces()
		{
			Assert.Equal("single line", _sut.Trim("\t\tsingle line "));
			Assert.Equal("Line1\r\nLine2", _sut.Trim("\t\tLine1 \r\n  Line2  "));
		}

		[Fact]
		public void Sort()
		{
			Test(new List<string>() {"3", "1", "2"}, new List<string>() { "1", "2", "3" }, _sut.Sort);
			Test(new List<string>() { "xyz", "abc", "pqr" }, new List<string>() { "abc", "pqr", "xyz" }, _sut.Sort);
		}

		[Fact]
		public void Reverse()
		{
			Test(new List<string>() { "abc", "pqr", "xyz" }, new List<string>() { "xyz", "pqr", "abc" }, _sut.Reverse);
		}

		[Fact]
		public void RemoveDuplicates()
		{
			Test(new List<string>() { "abc", "abc", "xyz" }, new List<string>() { "abc", "xyz"}, _sut.RemoveDuplicates);
		}

		[Fact]
		public void ConvertDecimalTo8BytesLowercaseHex()
		{
			Test(new List<string>() { "12341", "4324", "234" }, new List<string>() { "00003035", "000010e4", "000000ea" }, _sut.ConvertDecimalTo8BytesLowercaseHex);
		}

		[Fact]
		public void ConvertHexToDecimal()
		{
			Test(new List<string>() { "0x00003035", "0X000010e4", "000000ea" }, new List<string>() { "12341", "4324", "234" },  _sut.ConvertHexToDecimal);
		}

		[Fact]
		public void ToSqlInListQuoted()
		{
			Assert.Equal("( '1', '2', '3' )", 
				_sut.ToSqlInListQuoted(new List<string>() { "1", "2", "3" }.JoinIntoString()));
		}

		[Fact]
		public void ToSqlInList()
		{
			Assert.Equal("( 1, 2, 3 )",
				_sut.ToSqlInList(new List<string>() { "1", "2", "3" }.JoinIntoString()));
		}

		delegate string TestMethod(string input);
		private void Test(IEnumerable<string> input, IEnumerable<string> expected, TestMethod method)
		{
			Assert.Equal(expected.JoinIntoString(), method(input.JoinIntoString()));
		}
		[Fact]
		public void IpAddressToHexNumber()
		{
			Test(new List<string>() { "191.168.12.21", "191.200.12.13", "0.0.0.0" }, new List<string>() { "0XBFA80C15", "0XBFC80C0D", "0X00000000" }, _sut.IpAddressToHexNumber);
		}


		[Fact(Skip = "Missing functionality: Bad elements in input set")]
		public void IpAddressToHexNumberInvalidInput()
		{
			Test(new List<string>() { "256.256.256.256" }, new List<string>() { "Invalid 256.256.256.256" },
				_sut.IpAddressToHexNumber);
		}

		[Fact]
		public void HexToIpAddress()
		{
			Test(new List<string>() { "0XBFA80C15", "0XBFC80C0D", "0X00000000" }, new List<string>() { "191.168.12.21", "191.200.12.13", "0.0.0.0" }, _sut.HexToIpAddress);
		}
		//[Fact]
		//public void ApplyPattern()
		//{
		//	Test(new List<string>() { "$$VAL$$ is $$VAL$$", "Monday", "Tuesday" }, 
		//		new List<string>() { "Monday is Monday", "Tuesday is Tuesday"}, _sut.ApplyPattern);
		//}
	}

	internal static class ExtensionMethods
	{
		public static string JoinIntoString<T>(this IEnumerable<T> lines)
		{
			return string.Join(Environment.NewLine, lines);
		}
	}
}