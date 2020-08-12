using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace ClipboardUtilities.Lib.Tests
{
	public class StringUtilitiesTests
	{
		private readonly IStringUtilities _sut = new StringUtilities();

		private delegate string TestMethod(string input);

		private void Test(IEnumerable<string> input, IEnumerable<string> expected, TestMethod method)
		{
			method(input.JoinIntoString()).Should().Be(expected.JoinIntoString());
		}

		[Fact]
		public void FormatXml_GivenInvalidXml_ReturnsError() => _sut.FormatXml("<Project").Should().Contain("System.Xml.XmlException");

		[Fact]
		public void FormatXml_GivenNoXmlPrefix_FormatsXml()
		{
			_sut.FormatXml(
				"<Project> <PropertyGroup> <Configuration>Debug</Configuration> </PropertyGroup> </Project>").Should().Be(
				"<Project>\r\n  <PropertyGroup>\r\n    <Configuration>Debug</Configuration>\r\n  </PropertyGroup>\r\n</Project>"
				);
		}

		[Fact]
		public void FormatXml_GivenXmlPrefix_FormatsXml()
		{
			_sut.FormatXml(
					"xml=<Project> <PropertyGroup> <Configuration>Debug</Configuration> </PropertyGroup> </Project>").Should().Be("<Project>\r\n  <PropertyGroup>\r\n    <Configuration>Debug</Configuration>\r\n  </PropertyGroup>\r\n</Project>");
		}

		[Fact]
		public void DefineCSharpByteArray_GivenValidInput_ConvertsToByteArray()
		{
			var expected = "byte[] arrayName = {\r\n0x0A, 0x12, 0x34, 0x0A, 0x12, 0x34, 0x0A\r\n};";
			var actual = _sut.DefineCSharpByteArray("0A12340A12340A");
			actual.Should().Be(expected);
		}
		[Fact]
		public void DefineCSharpByteArray__GivenOddCharactersInByteSequence_ReturnsError()
		{
			var expected = "The provided byte sequence has odd number of characters, which makes it invalid";
			var actual = _sut.DefineCSharpByteArray("0A12340A1234E");
			actual.Should().Contain(expected);
		}

		[Fact]
		public void ApplyPattern()
		{
			Test(new List<string> { "$$VAL$$ is $$VAL$$", "Monday", "Tuesday" },
				new List<string> { "Monday is Monday", "Tuesday is Tuesday" }, _sut.ApplyPattern);
		}

		[Fact]
		public void AssignValuesToVariables()
		{
			Test(new List<string> { "@var1\t@var2", "10\tTen" },
				new List<string> { "select ", "    @var1 = 10,", "    @var2 = 'Ten'" }, _sut.AssignValuesToVariables);
		}

		[Fact]
		public void ConvertDecimalTo8BytesLowercaseHex()
		{
			Test(new List<string> { "12341", "4324", "234" }, new List<string> { "00003035", "000010e4", "000000ea" },
				_sut.ConvertDecimalTo8BytesLowercaseHex);
		}

		[Fact]
		public void ConvertHexToDecimal()
		{
			Test(new List<string> { "0x00003035", "0X000010e4", "000000ea" },
				new List<string> { "12341", "4324", "234" }, _sut.ConvertHexToDecimal);
		}

		[Fact]
		public void ExtractPattern()
		{
			Test(new List<string> { @"\d+", "EmpId:123", "No matching pattern", "EmpId:456" },
				new List<string> { "123", "", "456" }, _sut.ExtractPattern);
		}

		[Fact]
		public void HexToIpAddress()
		{
			// ReSharper disable StringLiteralTypo
			Test(new List<string> { "0XBFA80C15", "0XBFC80C0D", "0X00000000" },
				new List<string> { "191.168.12.21", "191.200.12.13", "0.0.0.0" }, _sut.HexToIpAddress);
			// ReSharper restore StringLiteralTypo
		}

		[Fact]
		public void IpAddressToHexNumber()
		{
			// ReSharper disable StringLiteralTypo
			Test(new List<string> { "191.168.12.21", "191.200.12.13", "0.0.0.0" },
				new List<string> { "0XBFA80C15", "0XBFC80C0D", "0X00000000" }, _sut.IpAddressToHexNumber);
			// ReSharper restore StringLiteralTypo
		}

		[Fact]
		public void IpAddressToHexNumber_GivenInvalidInput_SkipsOverInvalidInput()
		{
			Test(new List<string> { "256.256.256.256" }, new List<string> { "Invalid: [256.256.256.256]. An invalid IP address was specified." },
				_sut.IpAddressToHexNumber);
		}

		[Fact]
		public void LogDateToSplunkDate() => _sut.LogDateToSplunkDate("2015-01-20  16:47:32.777").Should().Be("\"01/20/2015:16:47:32\"");

		[Fact]
		public void RemoveDuplicates() => Test(new List<string> { "abc", "abc", "xyz" }, new List<string> { "abc", "xyz" }, _sut.RemoveDuplicates);

		[Fact]
		public void RemoveEmptyLines()
		{
			Test(new List<string> { "Sunday", "", "  ", "\t", "Monday", "Tuesday" },
				new List<string> { "Sunday", "Monday", "Tuesday" }, _sut.RemoveEmptyLines);
		}

		[Fact]
		public void Reverse() => Test(new List<string> { "abc", "pqr", "xyz" }, new List<string> { "xyz", "pqr", "abc" }, _sut.Reverse);

		[Fact]
		public void Sort()
		{
			Test(new List<string> { "3", "1", "2" }, new List<string> { "1", "2", "3" }, _sut.Sort);
			Test(new List<string> { "xyz", "abc", "pqr" }, new List<string> { "abc", "pqr", "xyz" }, _sut.Sort);
		}

		[Fact]
		public void ToSingleToLine()
		{
			// ReSharper disable StringLiteralTypo
			_sut.ToSingleToLine("Jira-1234\r\nFix the crash").Should().Be("Jira-1234 Fix the crash");
			// ReSharper restore StringLiteralTypo
		}

		[Fact]
		public void ToSplunkOr()
		{
			_sut.ToSplunkOr(new List<string> {"1", "2", "3"}.JoinIntoString()).Should().Be("( 1 OR 2 OR 3 )");
		}

		[Fact]
		public void ToSqlInList()
		{
			_sut.ToSqlInList(new List<string> {"1", "2", "3"}.JoinIntoString()).Should().Be("( 1, 2, 3 )");
		}

		[Fact]
		public void ToSqlInListQuoted()
		{
			_sut.ToSqlInListQuoted(new List<string> {"1", "2", "3"}.JoinIntoString()).Should().Be("( '1', '2', '3' )");
		}

		[Fact]
		public void ToSqlSelectAs()
		{
			Test(new List<string> { "@var1", "@var2" },
				new List<string> { "select", "    @var1 as '@var1',", "    @var2 as '@var2'" }, _sut.ToSqlSelectAs);
		}

		[Fact]
		public void Trim()
		{
			_sut.Trim("\t\tLine1 \r\n  Line2  ").Should().Be("Line1\r\nLine2");
		}
	}

	internal static class ExtensionMethods
	{
		public static string JoinIntoString<T>(this IEnumerable<T> lines)
		{
			return string.Join(Environment.NewLine, lines);
		}
	}
}