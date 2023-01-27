using EquationSolver.Parsers;
using EquationSolver.Solvers;
using System.Text;

namespace EquationSolver
{
    public class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Введите параметры уравнения в формате \"a b c\" или путь к файлу с аргументами:");
                var inputText = Console.ReadLine();

                var argumentParser = new QuadricEquationParser();
                var dataReader = new DataReader(argumentParser);
                var solver = new QuadricEquationSolver();

                try
                {
                    var casesList = dataReader.GetData(inputText);
                    foreach (var caseArguments in casesList)
                    {
                        var result = solver.GetEquationResult(caseArguments);

                        Console.WriteLine(CreateArgumentsString(caseArguments));
                        Console.WriteLine(CreateResultString(result));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static string CreateArgumentsString(double[] arguments)
        {
            //sb на случай, если кто-то решит ввести миллион уравнений
            var sb = new StringBuilder("Аргументы:");
            foreach (var argument in arguments)
            {
                sb.Append(' ');
                sb.Append(argument);
            }

            return sb.ToString();
        }

        static string CreateResultString(double[] result)
        {
            var sb = new StringBuilder("Результат: ");

            if (result.Length == 1)
            {
                sb.Append(result[0]);
            }
            else if (result.Length == 2)
            {
                sb.Append(result[0]);
                sb.Append(" и ");
                sb.Append(result[1]);
            }
            else
                sb.Append("отсутствует");

            return sb.ToString();
        }
    }
}