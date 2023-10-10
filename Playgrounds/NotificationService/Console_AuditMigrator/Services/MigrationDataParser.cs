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

        var producing = applicationLogs.Where(l => l.LogType == LogType.ProducingToKafka).ToList();
        
        // todo:
        var input = producing[0].Message ?? string.Empty;
        const string typePattern = @"with type '""([^""]*)""'";
        const string consumerPatter = @"by consumer '""([^""]*)""'.$";

        var typeMatch = Regex.Match(input, typePattern);
        var consumerMatch = Regex.Match(input, consumerPatter);

        if (typeMatch.Success && consumerMatch.Success)
        {
            var type = typeMatch.Groups[1].Value;
            var consumer = consumerMatch.Groups[1].Value;

            var migrationData = new MigrationData
            {
                ApplicationLog = producing[0],
                Timestamp = producing[0].Timestamp,
                LogType = producing[0].LogType,
                NotificationId = null,
                RequestId = producing[0].RequestId,
                Payload = JsonConvert.SerializeObject(new ProducingParameters
                {
                    Type = type,
                    Consumer = consumer
                })
            };

            await _dbContext.MigrationData.AddAsync(migrationData);
        }
        
        await _dbContext.SaveChangesAsync();
    }
}