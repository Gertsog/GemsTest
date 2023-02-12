using System.Text.RegularExpressions;

namespace EquationSolver
{
    public class DataReader
    {
        private ArgumentParser _argumentParser;

        public DataReader(ArgumentParser argumentParser)
        {
            _argumentParser = argumentParser;
        }

        /// <summary>
        /// Reading data from string or from file
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>List of argument arrays</returns>
        public List<double[]> GetData(string input)
        {
            var result = new List<double[]>();

            //Регулярка для проверки введённого пути к файлу
            var pathChecker = new Regex(@"^[a-zA-Z]\:(\/|\\)([ \w\.\-]+(\/|\\))*[ \w\.\-]+$");
            if (pathChecker.IsMatch(input))
            {
                try
                {
                    using var sr = new StreamReader(input);
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            var arguments = ParseArgumentsFromString(line);
                            result.Add(arguments);
                        }
                    }
                            
                    sr.Close();
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException($"Файл {input} не найден");
                }
            }
            else
            {
                var arguments = ParseArgumentsFromString(input);
                result.Add(arguments);
            }

            return result;
        }

        private double[] ParseArgumentsFromString(string text)
        {
            try
            {
                return _argumentParser.Parse(text);
            }
            catch (FormatException)
            {
                throw new FormatException($"Строка {text} содержит некорректные символы");
            }
        }
    }
}
