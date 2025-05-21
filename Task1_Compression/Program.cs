class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку:");
        string original = Console.ReadLine() ?? "";
        string compressed = Compress(original);
        string decompressed = Decompress(compressed);

        Console.WriteLine("Исходная строка:    " + original);
        Console.WriteLine("Сжатая строка:      " + compressed);
        Console.WriteLine("Декомпрессированная: " + decompressed);
    }

    static string Compress(string input)
    {
        string result = "";
        char curentС = ' ';
        int count = 1;
        foreach (char c in input)
        {
            if (c == curentС)
            {
                count++;
                continue;
            }
            else if (count > 1)
            {
                result += $"{count}";
            }
            result += c;
            count = 1;
            curentС = c;
        }
        if (count > 1)
            result += $"{count}";
        return result.ToString();
    }

    static string Decompress(string input)
    {
        string result = "";
        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];
            char next = '\0';
            if (i + 1 < input.Length)
                next = input[i + 1];

            if (char.IsLetter(current) && char.IsDigit(next))
            {
                string add = $"{next}";
                int j = i + 2;
                while (j < input.Length && char.IsDigit(input[j]))
                {
                    add += input[j];
                    j++;
                }
                result += new string(current, int.Parse(add));

            }
            else if (char.IsLetter(current))
            {
                result += current;
            }
        }
        return result.ToString();
    }
}