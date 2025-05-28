using LogsToStandart;

Console.WriteLine("Введите полный путь до файла логов: ");

string inputFile = Console.ReadLine();
if(!File.Exists(inputFile)) 
{ 
    Console.WriteLine("Файл не существует"); 
    return; 
}

string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), "outputLogFile.txt");

try
{
    string errorFilePath = Path.Combine(Path.GetDirectoryName(outputFile), "problems.txt");
    LogStandardizer.ProcessLogFile(inputFile, outputFile, errorFilePath);
    Console.WriteLine("Log file processed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}