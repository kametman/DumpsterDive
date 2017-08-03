using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DumpsterDive
{
    class Program
    {
        static string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
        static string NUMBERS = "0123456789";
        static string SYMBOLS = "!@#$%^&*()`-=[];',./~_+{}:<>?";

        static void Main(string[] args)
        {
            char sizeType = ' ';
            int chunks = 0;
            bool? includeUpper = null;
            bool? includeLower = null;
            bool? includeNumbers = null;
            bool? includeSymbols = null;
            bool wait = true;

            Console.WriteLine("DumpsterDive Garbage File Generator");

            if (args.Length > 0)
            {
                Console.WriteLine("Parsing command line parameters...");
                foreach (var arg in args)
                {
                    switch (arg[1])
                    {
                        case 't': sizeType = arg.ToUpper()[2]; break;
                        case 's': int.TryParse(arg.Substring(2), out chunks); break;
                        case 'c':
                            foreach (char c in arg.Substring(2))
                            {
                                if (c.Equals('A')) { includeUpper = true; }
                                if (c.Equals('a')) { includeLower = true; }
                                if (c.Equals('1')) { includeNumbers = true; }
                                if (c.Equals('!')) { includeSymbols = true; }
                            }
                            break;
                        case 'n': wait = false; break;
                        default: Console.WriteLine($"\tInvalid argument: {arg}"); break;
                    }
                }
            }

            if (sizeType.Equals(' '))
            {
                try
                {
                    Console.WriteLine("File size type (KB/MB)? ");
                    sizeType = Console.ReadLine().ToUpper()[0];
                }
                catch { sizeType = ' '; }
            }
            if (chunks == 0 && (sizeType.Equals('K') || sizeType.Equals('M')))
            {
                try
                {
                    Console.Write($"File size (in {sizeType}B)? ");
                    var sizeInput = Console.ReadLine();

                    chunks = 0;
                    int.TryParse(sizeInput, out chunks);
                }
                catch { chunks = 0; }
            }
            if (!includeUpper.HasValue && !includeLower.HasValue && !includeNumbers.HasValue && !includeSymbols.HasValue)
            {
                try
                {
                    Console.Write("Include all characters (y/n)? ");
                    var charInput = Console.ReadLine().ToLower()[0];
                    if (charInput.Equals('n'))
                    {
                        Console.WriteLine("Include upper-case characters (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        includeUpper = charInput.Equals('y');

                        Console.WriteLine("Include lower-case characters (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        includeLower = charInput.Equals('y');

                        Console.WriteLine("Include numbers (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        includeNumbers = charInput.Equals('y');

                        Console.WriteLine("Include symbols (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        includeSymbols = charInput.Equals('y');
                    }
                    else
                    {
                        includeUpper = charInput.Equals('y');
                        includeLower = charInput.Equals('y');
                        includeNumbers = charInput.Equals('y');
                        includeSymbols = charInput.Equals('y');
                    }
                }
                catch { }
            }

            if (ValidateParameters(sizeType, chunks, includeUpper ?? false, includeLower ?? false, includeNumbers ?? false, includeSymbols ?? false))
            {
                var charSets = new List<string>();
                if (includeLower.HasValue && includeLower.Value) { charSets.Add(UPPER_CASE); }
                if (includeUpper.HasValue && includeUpper.Value) { charSets.Add(LOWER_CASE); }
                if (includeNumbers.HasValue && includeNumbers.Value) { charSets.Add(NUMBERS); }
                if (includeSymbols.HasValue && includeSymbols.Value) { charSets.Add(SYMBOLS); }

                GenerateGarbage(sizeType, chunks, charSets);
            }
            else { Console.WriteLine("Invalid parameters were found. File not generated."); }

            if (wait)
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        static bool ValidateParameters(char sizeType, int chunks, bool upper, bool lower, bool numbers, bool symbols)
        {
            bool isValid = true;
            if (!sizeType.Equals('M') && !sizeType.Equals('K'))
            {
                Console.WriteLine("\tInvalid file size type!");
                isValid = false;
            }
            if (chunks < 1)
            {
                Console.WriteLine("\tInvalid file size!");
                isValid = false;
            }
            if (!upper && !lower && !numbers && !symbols)
            {
                Console.WriteLine("\tNo character sets selected!");
                isValid = false;
            }

            return isValid;
        }

        static void GenerateGarbage(char sizeType, int chunks, IList<string> charSets)
        {
            var randGen = new Random();
            var fileName = Guid.NewGuid().ToString();
            var fileSize = 1024 * chunks;
            if (sizeType.Equals('M')) { fileSize *= 1024; }

            Console.WriteLine($"Generating {chunks}{sizeType}B garbage file...");
            Console.WriteLine($"File name: {fileName}");

            var n = -1;
            using (var sw = new StreamWriter(fileName))
            {
                for (int i = 0; i < fileSize; i++)
                {
                    n = randGen.Next(charSets.Count());
                    sw.Write(charSets[n][randGen.Next(charSets[n].Length)]);
                }

                sw.Close();
            }

            Console.WriteLine($"Garbage file '{fileName}' completed ({chunks}{sizeType}B).");
        }
    }
}