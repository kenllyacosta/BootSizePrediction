using StatsModels.Readers;

namespace StatsModels.Tests.Readers
{
    public class CsvReaderTests
    {
        private const string TestCsvContent = "Name,Age,Height\nJohn Doe,30,5.9\nJane Smith,25,5.5";
        private const string TestCsvContentWithNull = "Name,Age,Height\nJohn Doe,30,5.9\nJane Smith,null,5.5";

        private class TestModel
        {
            public string Name { get; set; }
            public int? Age { get; set; }
            public double Height { get; set; }
        }

        [Fact]
        public void Read_ValidCsv_ReturnsCorrectData()
        {
            // Arrange
            var filePath = CreateTempCsvFile(TestCsvContent);

            // Act
            var result = CsvReader.Read<TestModel>(filePath).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Jane Smith", result[0].Name);
            Assert.Equal(25, result[0].Age);
            Assert.Equal(5.5, result[0].Height);
            Assert.Equal("John Doe", result[1].Name);
            Assert.Equal(30, result[1].Age);
            Assert.Equal(5.9, result[1].Height);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Read_CsvWithNullValues_ReturnsCorrectData()
        {
            // Arrange
            var filePath = CreateTempCsvFile(TestCsvContentWithNull);

            // Act
            var result = CsvReader.Read<TestModel>(filePath).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].Name);
            Assert.Equal(30, result[0].Age);
            Assert.Equal(5.9, result[0].Height);
            Assert.Equal("Jane Smith", result[1].Name);
            Assert.Null(result[1].Age);
            Assert.Equal(5.5, result[1].Height);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Read_InvalidCsv_ThrowsCsvReaderException()
        {
            // Arrange
            var invalidCsvContent = "Name,Age,Height\nJohn Doe,30,5.9\nJane Smith,invalid,5.5";
            var filePath = CreateTempCsvFile(invalidCsvContent);

            // Act & Assert
            var exception = Assert.Throws<CsvReaderException>(() => CsvReader.Read<TestModel>(filePath).ToList());
            Assert.Contains("Error processing line", exception.Message);

            // Cleanup
            File.Delete(filePath);
        }

        private string CreateTempCsvFile(string content)
        {
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, content);
            return filePath;
        }
    }
}