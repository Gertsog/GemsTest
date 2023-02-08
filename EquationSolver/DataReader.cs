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
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                            if (line != "")
                                result.Add(_argumentParser.Parse(line));

                        sr.Close();
                    }
                    catch (FormatException)
                    {
                        throw new FormatException($"Строка {line} содержит некорректные символы");
                    }
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException($"Файл {input} не найден");
                }
            }
            else
            {
                try
                {
                    result.Add(_argumentParser.Parse(input));
                }
                catch (FormatException)
                {
                    throw new FormatException($"Строка {input} содержит некорректные символы");
                }
            }

            return result;
        }
    }
}
