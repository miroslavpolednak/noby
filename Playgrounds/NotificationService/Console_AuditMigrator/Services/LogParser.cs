using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Console_AuditMigrator.Models;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Console_AuditMigrator.Services;

public class LogParser : ILogParser
{
    private readonly string[] _formats;
    private const string _groupName = "Variable";
    private const string _timestampPattern = @"^(?<Variable>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})";
    private const string _variablePattern = @"\[(?<Variable>[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'Close-Open'\])[^\[\]]*)+)*(?(Open)(?!)))\]";
    
    public LogParser(IOptions<AppConfiguration> options)
    {
        _formats = options.Value.DateTimeFormats;
    }
    
    public async Task<IList<ApplicationLog>> ParseFile(string fileName)
    {
        var lines = await File.ReadAllLinesAsync(fileName);
        return ParseLines(lines).ToList();
    }

    private IEnumerable<ApplicationLog> ParseLines(string[] lines)
    {
        if (!lines.Any())
        {
            yield break;
        }
        
        var builder = new StringBuilder().Append(lines[0]);
        if (lines.Length == 1)
        {
            yield return ParseLog(builder.ToString());
        }

        for (var i = 1; i < lines.Length; i++)
        {
            // if next starts with timestamp
            if (Regex.IsMatch(lines[i], _timestampPattern))
            {
                var log = builder.ToString();
                builder = new StringBuilder().Append(lines[i]);
                yield return ParseLog(log);
            }
            else
            {
                builder.AppendLine(lines[i]);
            }
        }
        
    }

    private ApplicationLog ParseLog(string log)
    {
        const string pattern = $"{_timestampPattern}|{_variablePattern}";
        var matches = Regex.Matches(log, pattern);

        return new ApplicationLog
        {
            Timestamp = DateTime.ParseExact(matches[0].Groups[_groupName].Value, _formats, CultureInfo.InvariantCulture, DateTimeStyles.None),
            ThreadId = matches[1].Groups[_groupName].Value,
            Level = matches[2].Groups[_groupName].Value,
            TraceId = matches[3].Groups[_groupName].Value,
            SpanId = matches[4].Groups[_groupName].Value,
            ParentId = matches[5].Groups[_groupName].Value,
            CisAppKey = matches[6].Groups[_groupName].Value,
            Version = matches[7].Groups[_groupName].Value,
            Assembly = matches[8].Groups[_groupName].Value,
            SourceContext = matches[9].Groups[_groupName].Value,
            MachineName = matches[10].Groups[_groupName].Value,
            ClientIp = matches[11].Groups[_groupName].Value,
            CisUserId = matches[12].Groups[_groupName].Value,
            CisUserIdent = matches[13].Groups[_groupName].Value,
            RequestId = matches[14].Groups[_groupName].Value,
            RequestPath = matches[15].Groups[_groupName].Value,
            ConnectionId = matches[16].Groups[_groupName].Value,
            Message = matches[17].Groups[_groupName].Value,
            Exception = matches[18].Groups[_groupName].Value,
        };
    }
}