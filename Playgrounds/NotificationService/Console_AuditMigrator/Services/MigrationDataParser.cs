using System.Text.RegularExpressions;
using Console_AuditMigrator.Database;
using Console_AuditMigrator.Database.Entities;
using Console_AuditMigrator.Models;
using Console_AuditMigrator.Models.LogParameters;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Console_AuditMigrator.Services;

public class MigrationDataParser : IMigrationDataParser
{
    private readonly LogDbContext _dbContext;
    
    public MigrationDataParser(LogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task ParseFromApplicationLogs()
    {
        var applicationLogs = await _dbContext.ApplicationLog
            .OrderBy(l => l.Timestamp)
            .ToListAsync();

        var lookup = applicationLogs.ToLookup(l => l.LogType);

        var httpRequest = lookup[LogType.ReceivedHttpRequest].Select(ParseHttpRequest).ToList();
        var httpResponse = lookup[LogType.SendingHttpResponse].Select(ParseHttpResponse).ToList();
        var producing = lookup[LogType.ProducingToKafka].Select(ParseProducing).ToList();
        var produced = lookup[LogType.ProducedToKafka].Select(ParseProduced).ToList();
        var couldNotProduce = lookup[LogType.CouldNotProduceToKafka].Select(ParseCouldNotProduce).ToList();
        var receivedReport = lookup[LogType.ReceivedReport].Select(ParseReceivedReport).ToList();

        await _dbContext.MigrationData.AddRangeAsync(httpRequest);
        await _dbContext.MigrationData.AddRangeAsync(httpResponse);
        await _dbContext.MigrationData.AddRangeAsync(producing);
        await _dbContext.MigrationData.AddRangeAsync(produced);
        await _dbContext.MigrationData.AddRangeAsync(couldNotProduce);
        await _dbContext.MigrationData.AddRangeAsync(receivedReport);

        // todo:
        // await _dbContext.SaveChangesAsync();
    }

    private static MigrationData ParseHttpRequest(ApplicationLog applicationLog)
    {
        // todo:
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            NotificationId = null,
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new HttpRequestParameters
            {
                // todo:
            })
        };
    }
    
    private static MigrationData ParseHttpResponse(ApplicationLog applicationLog)
    {
        // todo:
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            // NotificationId = todo: parse,
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new HttpResponseParameters
            {
                // todo:
            })
        };
    }
    
    private static MigrationData ParseProducing(ApplicationLog applicationLog)
    {
        const string typePattern = @"with type '""([^""]*)""'";
        const string consumerPatter = @"by consumer '""([^""]*)""'.$";

        var input = applicationLog.Message ?? string.Empty;
        var typeMatch = Regex.Match(input, typePattern);
        var consumerMatch = Regex.Match(input, consumerPatter);
        
        var type = typeMatch.Groups[1].Value;
        var consumer = consumerMatch.Groups[1].Value;

        if (!typeMatch.Success || !consumerMatch.Success)
        {
            throw new InvalidDataException();
        }
        
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            NotificationId = null,
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new ProducingParameters
            {
                Type = type,
                Consumer = consumer
            })
        };
    }
    
    private static MigrationData ParseProduced(ApplicationLog applicationLog)
    {
        // todo:
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            // NotificationId = todo: parse
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new ProducedParameters
            {
                // todo:
            })
        };
    }

    private static MigrationData ParseCouldNotProduce(ApplicationLog applicationLog)
    {
        // todo:
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            NotificationId = null,
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new CouldNotProduceParameters
            {
                // todo:
            })
        };
    }
    
    private static MigrationData ParseReceivedReport(ApplicationLog applicationLog)
    {
        // todo:
        return new MigrationData
        {
            ApplicationLog = applicationLog,
            Timestamp = applicationLog.Timestamp,
            LogType = applicationLog.LogType,
            // NotificationId = todo: parse,
            RequestId = applicationLog.RequestId,
            Payload = JsonConvert.SerializeObject(new ReceivedReportParameters
            {
                // todo:  
            })
        };
    }
}