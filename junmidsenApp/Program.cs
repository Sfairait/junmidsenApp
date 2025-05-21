// See https://aka.ms/new-console-template for more information
using junmidsenApp;

Console.WriteLine("Compress - Decompress");
bool goodInputString = false;
do
{
    Console.WriteLine("Введите строку для сжатия (если ввести пустую строку, программа перейдет к распаковке): ");
    string original = Console.ReadLine();
    ///
    goodInputString = !string.IsNullOrEmpty(original) && !string.IsNullOrWhiteSpace(original);
    string compressed = !goodInputString ? string.Empty : CompressDecompressString.Compress(original);
    if (goodInputString)
    {
        Console.WriteLine($"Сжатая строка: {compressed}");
    }
}
while (goodInputString);
///
Console.WriteLine("Введите строку для распаковки (если ввести пустую строку, программа завершится): ");
do
{
    string compressedInput = Console.ReadLine();
    goodInputString = !string.IsNullOrEmpty(compressedInput) && !string.IsNullOrWhiteSpace(compressedInput);
    string decompressed = CompressDecompressString.Decompress(compressedInput);
    if (goodInputString)
    {
        Console.WriteLine($"Распакованная строка: {decompressed}");
    }
}
while (goodInputString);
