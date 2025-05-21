using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace junmidsenApp
{
    public static class LogStandardizer
    {
        //static void Main(string[] args)
        //{
        //    if (args.Length != 2)
        //    {
        //        Console.WriteLine("Usage: LogStandardizer <inputFile> <outputFile>");
        //        return;
        //    }

        //    string inputFile = args[0];
        //    string outputFile = args[1];

        //    try
        //    {
        //        ProcessLogFile(inputFile, outputFile);
        //        Console.WriteLine("Log file processed successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //}

        public static void ProcessLogFile(string inputPath, string outputPath)
        {
            var outputLines = new System.Collections.Generic.List<string>();

            foreach (string line in File.ReadLines(inputPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var standardized = StandardizeLogLine(line);
                if (standardized != null)
                    outputLines.Add(standardized);
            }

            File.WriteAllLines(outputPath, outputLines);
        }

        public static string StandardizeLogLine(string line)
        {
            // Try to match Format 1
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

            // Try to match Format 2
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

            Console.WriteLine($"Warning: Could not parse line: {line}");
            return null;
        }

        public static string ConvertDateFormat(string inputDate)
        {
            if (DateTime.TryParse(inputDate, out var date))
            {
                return date.ToString("dd-MM-yyyy");
            }
            return inputDate; // fallback
        }

        public static string NormalizeLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                _ => level.ToUpper()
            };
        }

        public static string FormatStandardLogEntry(string date, string time, string level, string method, string message)
        {
            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }
    }
}
