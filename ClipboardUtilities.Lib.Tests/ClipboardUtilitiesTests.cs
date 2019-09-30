using Xunit;

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
			Assert.Equal("1\r\n2\r\n3", _sut.Sort("3\r\n1\r\n2"));
			Assert.Equal("abc\r\npqr\r\nxyz", _sut.Sort("xyz\r\npqr\r\nabc"));
		}
	}
}