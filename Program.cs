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
            Console.WriteLine("DumpsterDive Garbage File Generator");
            Console.WriteLine("File size type (KB/MB)? ");
            var sizeType = Console.ReadLine().ToUpper()[0];
            if (sizeType.Equals("K") || sizeType.Equals("M"))
            {
                Console.Write($"File size (in {sizeType}B)? ");
                var sizeInput = Console.ReadLine();

                var chunks = 0;
                if (int.TryParse(sizeInput, out chunks) && (sizeType.Equals('K') || sizeType.Equals('M')))
                {
                    var fileSize = 1024 * chunks;
                    if (sizeType.Equals('m')) { fileSize *= 1024; }

                    var fileChars = new List<string>();
                    Console.Write("Include all characters (y/n)? ");
                    var charInput = Console.ReadLine().ToLower()[0];
                    if (charInput.Equals('n'))
                    {
                        Console.WriteLine("Include upper-case characters (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        if (charInput.Equals('y')) { fileChars.Add(UPPER_CASE); }

                        Console.WriteLine("Include lower-case characters (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        if (charInput.Equals('y')) { fileChars.Add(LOWER_CASE); }

                        Console.WriteLine("Include numbers (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        if (charInput.Equals('y')) { fileChars.Add(NUMBERS); }

                        Console.WriteLine("Include symbols (y/n)? ");
                        charInput = Console.ReadLine().ToLower()[0];
                        if (charInput.Equals('y')) { fileChars.Add(SYMBOLS); }
                    }
                    else { fileChars.AddRange(new string[] { UPPER_CASE, LOWER_CASE, NUMBERS, SYMBOLS }); }

                    if (fileChars.Count() > 0)
                    {
                        var randGen = new Random();
                        var fileName = Guid.NewGuid().ToString();
                        Console.WriteLine($"Generating {chunks}{sizeType}B garbage file...");
                        Console.WriteLine($"File name: {fileName}");

                        var n = -1;
                        using (var sw = new StreamWriter(fileName))
                        {
                            for (int i = 0; i < fileSize; i++)
                            {
                                n = randGen.Next(fileChars.Count());
                                sw.Write(fileChars[n][randGen.Next(fileChars[n].Length)]);
                            }

                            sw.Close();
                        }

                        Console.WriteLine($"Garbage file '{fileName}' completed ({chunks}{sizeType}B).");
                    }
                    else { Console.WriteLine("No characters selected! Exiting program..."); }
                }
                else { Console.WriteLine("Invalid file size! Exiting program..."); }
            }
            else { Console.WriteLine("Invalid file size type! Exiting program..."); }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
