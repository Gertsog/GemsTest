namespace EquationSolver.Solvers
{
    public abstract class EquationSolver
    {
        /// <summary>
        /// Calculate equation result
        /// </summary>
        /// <param name="arguments">Arguments of equation</param>
        /// <returns>Result of equation</returns>
        public abstract double[] GetEquationResult(params double[] arguments);
    }
}
