using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace junmidsenApp
{
    public static class CompressDecompressString
    {
        public static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder compressed = new StringBuilder();
            int count = 1;
            char current = input[0];

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == current)
                {
                    count++;
                }
                else
                {
                    compressed.Append(current);
                    if (count > 1)
                        compressed.Append(count);

                    current = input[i];
                    count = 1;
                }
            }

            /// Добавляем последнюю группу
            compressed.Append(current);
            if (count > 1)
                compressed.Append(count);

            return compressed.ToString();
        }

        public static string Decompress(string compressed)
        {
            if (string.IsNullOrEmpty(compressed))
                return compressed;

            StringBuilder decompressed = new StringBuilder();
            int i = 0;

            while (i < compressed.Length)
            {
                char current = compressed[i++];
                if (i < compressed.Length && char.IsDigit(compressed[i]))
                {
                    /// Извлекаем число
                    int numStart = i;
                    while (i < compressed.Length && char.IsDigit(compressed[i]))
                        i++;

                    int count = int.Parse(compressed.Substring(numStart, i - numStart));
                    decompressed.Append(current, count);
                }
                else
                {
                    decompressed.Append(current);
                }
            }

            return decompressed.ToString();
        }
    }
}
