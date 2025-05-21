using System.Text.RegularExpressions;

namespace Task3_LogStandardizer
{
    abstract class LogParser
    {
        protected readonly Regex Regex;

        protected LogParser(string pattern)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public bool TryParse(string line, out ParsedLogEntry parsed)
        {
            parsed = new ParsedLogEntry();
            var match = Regex.Match(line);
            if (!match.Success) return false;

            return ParseMatch(match, out parsed);
        }
        protected static string ConvertLogLevel(string level) => level switch
        {
            "INFORMATION" or "INFO" => "INFO",
            "WARNING" or "WARN" => "WARN",
            "ERROR" => "ERROR",
            "DEBUG" => "DEBUG",
            _ => "DEFAULT"
        };
        protected abstract bool ParseMatch(Match match, out ParsedLogEntry parsed);
    }
    class Format1Parser : LogParser
    {
        public Format1Parser() : base(@"(?<Date>\d{2}\.\d{2}\.\d{4}) (?<Time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<Level>[A-Z]+) (?<Message>.+)")
        { }

        protected override bool ParseMatch(Match match, out ParsedLogEntry parsed)
        {
            parsed = new ParsedLogEntry
            {
                Date = DateTime.ParseExact(match.Groups["Date"].Value, "dd.MM.yyyy", null).ToString("yyyy-MM-dd"),
                Time = match.Groups["Time"].Value,
                Level = ConvertLogLevel(match.Groups["Level"].Value),
                Method = "DEFAULT",
                Message = match.Groups["Message"].Value
            };
            return true;
        }
    }
    class Format2Parser : LogParser
    {
        public Format2Parser() : base(
        @"^(?<Date>\d{4}-\d{2}-\d{2})\s+" +
        @"(?<Time>\d{2}:\d{2}:\d{2}\.\d+)\|\s*" +
        @"(?<Level>[A-Z]+)\|\s*\d+\|\s*" + // пропустил поток, или что бы это ни было)) вероятно ошибка в задании
        @"(?<Method>[\w\.]+)\|\s*" +
        @"(?<Message>.+)$")
        { }

        protected override bool ParseMatch(Match match, out ParsedLogEntry parsed)
        {
            parsed = new ParsedLogEntry
            {
                Date = match.Groups["Date"].Value,
                Time = match.Groups["Time"].Value,
                Level = ConvertLogLevel(match.Groups["Level"].Value),
                Method = match.Groups["Method"].Value,
                Message = match.Groups["Message"].Value
            };
            return true;
        }
    }
    class ParsedLogEntry
    {
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public string Level { get; set; } = "";
        public string Method { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
   
    

    

