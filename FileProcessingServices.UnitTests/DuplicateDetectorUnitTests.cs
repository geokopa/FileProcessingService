using FileProcessingService.Shared.Extensions;
using Xunit;

namespace FileProcessingServices.UnitTests
{
    public class DuplicateDetectorUnitTests
    {
        [Fact]
        public void FindTwoDuplicateWithValidString()
        {
            // Arrange
            string input = "George is very George";

            // Act
            var result = input.FindDuplicates().GetCountForWord("George");

            // Assert
            Assert.True(result == 2);
        }

        [Fact]
        public void SerializeDuplicateWordsToJson()
        {
            // Arrange
            string input = "George is very George";

            // Act
            var result = input.FindDuplicates().ToJson();

            // Assert
            Assert.NotEmpty(result);

        }
    }
}
