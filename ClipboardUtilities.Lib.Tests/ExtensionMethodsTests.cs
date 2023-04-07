using FluentAssertions;
using Xunit;

namespace ClipboardUtilities.Lib.Tests
{
    public class ExtensionMethodsTests
    {
        [Fact]
        public void RemoveTrailingRemovesAdditionalCommaAtTheEnd()
        {
            @"3 OR\r\n3 OR\r\n3 OR\r\n3 OR\r\n1 OR\r\n OR".RemoveTrailing("OR").Should().Be("1 OR 2 OR 3");
            "1,2,3, ".RemoveTrailing(",").Should().Be("1,2,3");
        }
    }
}