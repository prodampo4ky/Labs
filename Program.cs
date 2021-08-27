using System;
using System.Collections.Generic;
using System.IO;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool err = true;
            int symbolsQ;
            double entropy;
            Dictionary<char, double> symbolsDict = new Dictionary<char, double>();
            try
            {
                do
                {
                    string path = Input();
                    if (File.Exists(path))
                    {
                        FileInfo file = new FileInfo(path);
                        long sizeF = file.Length;
                        symbolsQ = FillDict(path, symbolsDict);
                        Probability(symbolsDict, symbolsQ);
                        entropy = Entropy(symbolsDict);
                        Output(symbolsDict, symbolsQ, entropy, sizeF);
                        err = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Something went wrong, try again");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("------------------------------------------------------");
                    }
                } while (err == true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        public static string Input()
        {
            string nameF, path;
            bool err;
            try
            {
                do
                {
                    path = @"C:\Users\artem\Desktop\CompSys\lab1\";
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Введите название файла: ");
                    nameF = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("------------------------------------------------------");
                    path += nameF + ".txt";
                } while (err == true);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error in input Format");
            }
            return path;
        }
    }

    public static int FillDict(string path, Dictionary<char, double> symbols)
    {
        string text = File.ReadAllText(path);
        int sizeF = text.Length;
        for (int i = 0; i < sizeF; i++)
        {
            if (symbols.ContainsKey(text[i])) symbols[text[i]]++;
            else symbols.Add(text[i], 1);
        }
        return sizeF;
    }

    public static double Entropy(Dictionary<char, double> symbols)
    {
        int elementQ = symbols.Keys.Count;
        char[] keysDict = new char[elementQ];
        symbols.Keys.CopyTo(keysDict, 0);
        double entr = 0;
        for (int i = 0; i < elementQ; i++) entr -= symbols[keysDict[i]] * Math.Log(symbols[keysDict[i]], 2);
        return entr;
    }

    public static void Probability(Dictionary<char, double> symbols, int symbolsQ)
    {
        int elementQ = symbols.Keys.Count;
        char[] keysDict = new char[elementQ];
        symbols.Keys.CopyTo(keysDict, 0);
        for (int iter = 0; iter < elementQ; iter++) symbols[keysDict[iter]] /= symbolsQ;
    }

    public static void Output(Dictionary<char, double> symbols, int symbolsQ, double entr, long fileSize)
    {
        char[] alphabet = new char[32] {
                'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я'
            };
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("File size = {0:F4} bytes", fileSize);
        Console.WriteLine("The amount of information = {0:F4} bytes", symbolsQ * entr / 8);
        Console.WriteLine("Common entropy = {0:F5}", entr);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("------------------------------------------------------");
        SortedDictionary<char, double> sorted = new SortedDictionary<char, double>(symbols);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Frequency of symbols:");
        foreach (KeyValuePair<char, double> n in sorted)
        {
            switch (n.Key)
            {
                case '\r':
                    Console.WriteLine("Frequency of symbol \"/r\" in text = {0:f3}%", n.Value);
                    break;
                case '\n':
                    Console.WriteLine("Frequency of symbol \"/n\" in text = {0:f3}%", n.Value);
                    break;
                case 'і':
                    Console.WriteLine("Frequency of symbol \"i\" in text = {0:f3}%", n.Value);
                    break;
                default:
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        if (alphabet[i] == n.Key) Console.WriteLine("Frequency of symbol \"{0}\" in text = {1:f3}%", n.Key, n.Value);
                    }
                    break;
            }
        }
    }
}
}