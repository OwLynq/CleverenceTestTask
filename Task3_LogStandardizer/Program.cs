using Task3_LogStandardizer;

class Program
{
    static void Main()
    {
        var parsers = new List<LogParser> { new Format1Parser(), new Format2Parser() };

        string logDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty, "logs");
        string outputLogsDirectory = Path.Combine(logDirectory, "outputLogs");
        string errorLogsDirectory = Path.Combine(logDirectory, "errorLogs");

        var logFiles = Directory.GetFiles(logDirectory, "*.log");

        if (logFiles.Length == 0)
        {
            Console.WriteLine("Нет .log файлов в директории logs.");
            return;
        }

        string selectedFilePath = SelectFileFromList(logFiles);
        string outputFilePath = Path.Combine(outputLogsDirectory, Path.GetFileNameWithoutExtension(selectedFilePath) + "_standardized.txt");
        string errorFilePath = Path.Combine(errorLogsDirectory, Path.GetFileNameWithoutExtension(selectedFilePath) + "_problems.txt");

        StandardizeLogFile(parsers, selectedFilePath, outputFilePath, errorFilePath);

        Console.WriteLine("Готово!");
        if (File.Exists(outputFilePath))
            Console.WriteLine($"Результат записан в: {outputFilePath}");
        if (File.Exists(errorFilePath))
            Console.WriteLine($"Ошибки записаны в: {errorFilePath}");
    }

    static string SelectFileFromList(string[] files)
    {
        Console.WriteLine("Выберите файл для обработки:");
        for (int i = 0; i < files.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
        }

        while (true)
        {
            Console.Write("Введите номер файла: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index >= 1 && index <= files.Length)
            {
                return files[index - 1];
            }

            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
    }

    static void StandardizeLogFile(List<LogParser> parsers, string inputFilePath, string outputFilePath, string errorFilePath)
    {
        using var reader = new StreamReader(inputFilePath);

        var outputLines = new List<string>(); 
        var errorLines = new List<string>(); 

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            ParsedLogEntry? parsed = null;
            bool success = parsers.Any(p => p.TryParse(line, out parsed!));

            if (success)
            {
                outputLines.Add($"{parsed!.Date}\t{parsed.Time}\t{parsed.Level}\t{parsed.Method}\t{parsed.Message}");
            }
            else
            {           
                errorLines.Add(line);
            }
        }

        if (outputLines.Count > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)!);  
            File.WriteAllLines(outputFilePath, outputLines);  
        }

        if (errorLines.Count > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(errorFilePath)!);
            File.WriteAllLines(errorFilePath, errorLines);
        }
    }
}


