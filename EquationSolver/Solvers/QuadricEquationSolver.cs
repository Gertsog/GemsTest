namespace EquationSolver.Solvers
{
    public class QuadricEquationSolver : EquationSolver
    {
        /// <summary>
        /// Calculate quadric equation result
        /// </summary>
        /// <param name="arguments">Arguments of equation</param>
        /// <returns>Result of equation</returns>
        public override double[] GetEquationResult(double[] arguments)
        {
            double a = arguments[0], b = arguments[1], c = arguments[2];

            if (a == 0)
            {
                if (b != 0)
                    return new double[] { -1 * c / b };

                return Array.Empty<double>();
            }

            var discriminant = Math.Pow(b, 2) - 4 * a * c;

            if (discriminant == 0)
                return new double[] { -b / (2 * a) };

            if (discriminant > 0)
            {
                return new double[]
                {
                    (-b - Math.Sqrt(discriminant)) / (2 * a),
                    (-b + Math.Sqrt(discriminant)) / (2 * a)
                };
            }

            return Array.Empty<double>();
        }
    }
}
