using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Console_AuditMigrator.Database;
using Console_AuditMigrator.Database.Entities;
using Console_AuditMigrator.Models;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Console_AuditMigrator.Services;

public class LogParser : ILogParser
{
    private readonly LogDbContext _dbContext;
    private readonly IOptions<AppConfiguration> _options;

    private const string _groupName = "Variable";
    private const string _timestampPattern = @"^(?<Variable>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})";
    private const string _variablePattern = @"\[(?<Variable>[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'Close-Open'\])[^\[\]]*)+)*(?(Open)(?!)))\]";
    
    public LogParser(
        LogDbContext dbContext,
        IOptions<AppConfiguration> options)
    {
        _dbContext = dbContext;
        _options = options;
    }

    public async Task ParseLogFiles()
    {
        var files = Directory.GetFiles(_options.Value.LogsFolder);
        foreach (var file in files.Where(f => f.EndsWith(".log")))
        {
            var processed = await _dbContext.ProcessedFile.AnyAsync(f => f.FileName == file);

            if (processed)
            {
                Console.WriteLine($"File: {file} - Skipped - already processed.");
                continue;
            }
            Console.WriteLine($"File import: {file}");

            var logs = await ParseFile(file);
            var processedFile = new ProcessedFile { Timestamp = DateTime.Now, FileName = file };
        
            var parseError = logs.Where(t => t.Message == "ParseError").Count();

            var receivedRequest = FilterLogs(logs, "Received HTTP Request", processedFile, LogType.ReceivedHttpRequest);
            var sendingResponse = FilterLogs(logs, "Sending HTTP Response", processedFile, LogType.SendingHttpResponse);
            var producing = FilterLogs(logs, "Producing message SendSMS", processedFile, LogType.ProducingToKafka);
            var produced = FilterLogs(logs, "Produced message SendSMS", processedFile, LogType.ProducedToKafka);
            var couldNot = FilterLogs(logs, "Could not produce message SendSMS", processedFile, LogType.CouldNotProduceToKafka);
            var receivedReport = FilterLogs(logs, "Received notification report", processedFile, LogType.ReceivedReport);


            Console.WriteLine($"receivedRequest:{receivedRequest.Count}");
            Console.WriteLine($"sendingResponse:{sendingResponse.Count}");
            Console.WriteLine($"producing:{producing.Count}");
            Console.WriteLine($"produced:{produced.Count}");
            Console.WriteLine($"couldNot:{couldNot.Count}");
            Console.WriteLine($"receivedReport:{receivedReport.Count}");
            if (parseError > 0)
                Console.WriteLine($"parseError:{parseError}");

            await _dbContext.ProcessedFile.AddAsync(processedFile);
            await _dbContext.ApplicationLog.AddRangeAsync(receivedRequest);
            await _dbContext.ApplicationLog.AddRangeAsync(sendingResponse);
            await _dbContext.ApplicationLog.AddRangeAsync(producing);
            await _dbContext.ApplicationLog.AddRangeAsync(produced);
            await _dbContext.ApplicationLog.AddRangeAsync(couldNot);
            await _dbContext.ApplicationLog.AddRangeAsync(receivedReport);

            await _dbContext.SaveChangesAsync();
        }
    }
    
    private async Task<IList<ApplicationLog>> ParseFile(string fileName)
    {
        var lines = await File.ReadAllLinesAsync(fileName);
        Console.WriteLine($"lines:{lines.Length}");
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
        var formats = _options.Value.DateTimeFormats;

        try
        {
            var item = new ApplicationLog
            {
                Timestamp = DateTime.ParseExact(matches[0].Groups[_groupName].Value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None),
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
            return item;
        } catch {
            return new ApplicationLog
            {
                Message = "ParseError"
            };
        } 
    }
    
    static List<ApplicationLog> FilterLogs(IList<ApplicationLog> logs, string startWith, ProcessedFile processedFile, LogType logType)
    {
        return logs
            .Where(log => log.Message?.StartsWith(startWith) ?? false)
            .Select(log => Transform(log, processedFile, logType))
            .ToList();
    }

    static ApplicationLog Transform(ApplicationLog log, ProcessedFile processedFile, LogType logType)
    {
        log.ProcessedFile = processedFile;
        log.LogType = logType;
        return log;
    }
}