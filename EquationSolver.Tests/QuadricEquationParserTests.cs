using EquationSolver.Parsers;

namespace EquationSolver.Tests
{
    public class QuadricEquationParserTests
    {
        public static IEnumerable<object[]> ValidArgumentsTestData => new List<object[]>
        {
            new object[] { "1 2 3", new double[] { 1, 2, 3 }},
            new object[] { "-1 6 -5", new double[] { -1, 6, -5 } },
            new object[] { "2 5 -3.5", new double[] { 2, 5, -3.5 }},
            new object[] { "2 5 -3.567 ", new double[] { 2, 5, -3.567 }},
            new object[] { " 2.2 5      -3.5", new double[] { 2.2, 5, -3.5 }},
            new object[] { " 2     -5.75 -3.5 ", new double[] { 2, -5.75, -3.5 }}
        };

        public static IEnumerable<object[]> InvalidArgumentsTestData => new List<object[]>
        {
            new object[] {"qwe 2 3 4 asd" },
            new object[] {"qwe 2 3 4" },
            new object[] {"2 3 4 asd" },
            new object[] {"a b c" },
            new object[] {"test" }
        };

        public static IEnumerable<object[]> WrongAmountTestData => new List<object[]>
        {
            new object[] { "" },
            new object[] { "1" },
            new object[] { "1 2" },
            new object[] { "1 2 3 4" }
        };

        [Theory]
        [MemberData(nameof(ValidArgumentsTestData))]
        public void ValidArgumentsTest(string argumentsString, double[] expected)
        {
            var actual = new QuadricEquationParser().Parse(argumentsString);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsTestData))]
        public void InvalidArgumentsTest(string argumentsString)
        {
            Assert.Throws<FormatException>(() => new QuadricEquationParser().Parse(argumentsString));
        }

        [Theory]
        [MemberData(nameof(WrongAmountTestData))]
        public void WrongAmountTest(string argumentsString)
        {
            Assert.Throws<ArgumentException>(() => new QuadricEquationParser().Parse(argumentsString));
        }
    }
}
