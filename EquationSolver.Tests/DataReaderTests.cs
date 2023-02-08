using EquationSolver.Parsers;
using Xunit;

namespace EquationSolver.Tests
{
    public class DataReaderTests
    {
        //Я не смог адекватно получить доступ к папке с тестовыми файлами
        private static readonly string _parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        private static readonly string _testFilesDirectory = Path.Combine(_parentDirectory, "TestFiles");
        private static readonly string _testFilePath1 = Path.Combine(_testFilesDirectory, "TestFile1.txt");
        private static readonly string _testFilePath2 = Path.Combine(_testFilesDirectory, "TestFile2.txt");
        private static readonly string _testFilePath3 = Path.Combine(_testFilesDirectory, "TestFile3.txt");

        private static readonly List<double[]> _correctQuadricEquationParsingResult = new List<double[]>() 
        {
            new double[] { 1, 0, 1 },
            new double[] { 2, 5, -3.5 },
            new double[] { 1, 1, 1 },
            new double[] { 1, 4, 1 }
        };

        public static IEnumerable<object[]> QuadricEquationReaderTestData => new List<object[]>
        {
            new object[] { "1 2 3", new List<double[]>() { new double[] { 1, 2, 3 } } },
            new object[] { "-2 10 2", new List<double[]>() { new double[] { -2, 10, 2 } } },
            new object[] { _testFilePath1, _correctQuadricEquationParsingResult },
            new object[] { _testFilePath2, _correctQuadricEquationParsingResult },
            new object[] { _testFilePath3, new List<double[]>() }
        };

        public static IEnumerable<object[]> FileNotExistsTestData => new List<object[]>
        {
            new object[] { Path.Combine(_testFilesDirectory, "TestFile4.txt") }
        };

        [Theory]
        [MemberData(nameof(QuadricEquationReaderTestData))]
        public void QuadricEquationReaderTest(string input, List<double[]> expected)
        {
            var actual = new DataReader(new QuadricEquationParser()).GetData(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(FileNotExistsTestData))]
        public void FileNotExistsTest(string input)
        {
            Assert.Throws<FileNotFoundException>(() => new DataReader(new QuadricEquationParser()).GetData(input));
        }
    }
}
