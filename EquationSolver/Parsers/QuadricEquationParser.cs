using System.Globalization;

namespace EquationSolver.Parsers
{
    public class QuadricEquationParser : ArgumentParser
    {
        /// <summary>
        /// Parse arguments for quadric equation from string
        /// </summary>
        /// <param name="argumentsString">Arguments string</param>
        /// <returns>Array of numeric arguments</returns>
        public override double[] Parse(string argumentsString)
        {
            var result = argumentsString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(argument => double.Parse(argument, CultureInfo.InvariantCulture))
                .ToArray();

            if (result.Length != 3)
                throw new ArgumentException($"В строке {argumentsString} некорректное количество аргументов");

            return result;
        }
    }
}
