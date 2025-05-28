using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogsToStandart
{
    public static class LogStandardizer
    {
        /// <summary>
        /// Обработка лог-файла
        /// </summary>
        /// <param name="inputPath">Файл первычных логов</param>
        /// <param name="outputPath">Файл стандартных логов</param>
        /// <param name="errorFilePath">Файл логов-ошибок</param>
        public static void ProcessLogFile(string inputPath, string outputPath, string errorFilePath)
        {
            List<string> outputLines = [];
            List<string> errorLines = [];

            foreach (string line in File.ReadLines(inputPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var standardized = StandardizeLogLine(line);
                if (!string.IsNullOrEmpty(standardized))
                    outputLines.Add(standardized);
                else
                    errorLines.Add(line);
            }

            if (outputLines.Any())
                File.WriteAllLines(outputPath, outputLines);
            if (errorLines.Any())
                File.WriteAllLines(errorFilePath, errorLines);
        }

        /// <summary>
        /// Конвертация строки из формата 1 или 2 в стандартный формат.
        /// </summary>
        /// <param name="line">Исходная строка</param>
        /// <returns>Возвращает строку в стандартном формате или пустую строку, если входной формат не совпадает</returns>
        public static string StandardizeLogLine(string line)
        {
            try
            {
                ///  Format 1
                var format1Match = Regex.Match(line,
                    @"^(?<date>\d{2}\.\d{2}\.\d{4}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3,7})\s+(?<level>\w+)\s+(?<message>.+)$");

                if (format1Match.Success)
                {
                    return FormatStandardLogEntry(
                        ConvertDateFormat(format1Match.Groups["date"].Value),
                        format1Match.Groups["time"].Value,
                        NormalizeLogLevel(format1Match.Groups["level"].Value),
                        "DEFAULT",
                        format1Match.Groups["message"].Value.Trim());
                }

                /// Format 2
                var format2Match = Regex.Match(line,
                    @"^(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3,7})\|\s*(?<level>\w+)\|\d+\|(?<method>[^|]+)\|(?<message>.+)$");

                if (format2Match.Success)
                {
                    return FormatStandardLogEntry(
                        ConvertDateFormat(format2Match.Groups["date"].Value),
                        format2Match.Groups["time"].Value,
                        NormalizeLogLevel(format2Match.Groups["level"].Value),
                        format2Match.Groups["method"].Value.Trim(),
                        format2Match.Groups["message"].Value.Trim());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return string.Empty;
        }

        /// <summary>
        /// Конвертация даты в формат дд-мм-гггг
        /// </summary>
        /// <param name="inputDate">Дата в формате исходного лога</param>
        /// <returns>Дату в формате стандартного лога</returns>
        public static string ConvertDateFormat(string inputDate)
        {
            if (DateTime.TryParse(inputDate, out var date))
            {
                return date.ToString("dd-MM-yyyy");
            }
            return inputDate; // fallback
        }

        /// <summary>
        /// Нормализация уровня логирования
        /// </summary>
        /// <param name="level">Строка исходного уровня логирования</param>
        /// <returns>СТандартная строка уровня логирования</returns>
        public static string NormalizeLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                _ => level.ToUpper()
            };
        }

        /// <summary>
        /// Форматирование строки стандартного лога
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="level"></param>
        /// <param name="method"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string FormatStandardLogEntry(string date, string time, string level, string method, string message)
        {
            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }
    }
}
