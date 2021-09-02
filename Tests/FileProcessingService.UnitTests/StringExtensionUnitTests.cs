using FileProcessingService.Infrastructure.Extensions;
using System.Linq;
using Xunit;

namespace FileProcessingService.UnitTests
{
    public class StringExtensionUnitTests
    {
        [Theory]
        [InlineData("li;p;a")]
        public void SemicolonSeperatedStringReturnsArray(string input)
        {
            // Arrange && Act
            string[] words = input.AsCleanedArray();

            // Assert
            Assert.Equal(3, words.Count());
        }
    }
}
