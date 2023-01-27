using EquationSolver.Solvers;

namespace EquationSolver.Tests
{
    public class QuadricEquationSolverTests
    {
        public static IEnumerable<object[]> CalculateTestData => new List<object[]>
        {
            new object[] { 1, 2, 3, Array.Empty<double>() },
            new object[] { 1, 0, 1, Array.Empty<double>() },
            new object[] { 0, 0, 0, Array.Empty<double>() },
            new object[] { 0, 4, 8, new double[] { -2 }},
            new object[] { -1, 6, -5, new double[] { 5, 1 }},
            new object[] { 1, -6, 9, new double[] { 3 }}
        };

        [Theory]
        [MemberData(nameof(CalculateTestData))]
        public void CalculateTest(double a, double b, double c, double[] expected)
        {
            var actual = new QuadricEquationSolver().GetEquationResult(a, b, c);

            Assert.Equal(expected, actual);
        }
    }
}
