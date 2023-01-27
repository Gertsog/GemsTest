namespace EquationSolver
{
    public abstract class ArgumentParser
    {
        /// <summary>
        /// Parse arguments from string
        /// </summary>
        /// <param name="argumentsString">Arguments string</param>
        /// <returns>Array of numeric arguments</returns>
        public abstract double[] Parse(string argumentsString);
    }
}
