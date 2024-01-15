using Console_AuditMigrator.Database;
using Console_AuditMigrator.Database.Entities;
using Console_AuditMigrator.Models;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedAudit;

namespace Console_AuditMigrator.Services;

public class MigrationDataAggregator : IMigrationDataAggregator
{
    private readonly LogDbContext _dbContext;
    private IManualAuditLogger _logger;

    private readonly string _msgNoby012 = "NotificationService /sms or /smsFromTemplate HTTP request processed";
    private readonly string _msgNoby013Success = "Produced message SendSMS to KAFKA";
    private readonly string _msgNoby013Failed = "Could not produce message SendSMS to KAFKA";
    private readonly string _msgNoby014 = "Received notification report for sms";

    public MigrationDataAggregator(LogDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;

        var auditConf = configuration.GetSection("CisTelemetry:Logging:Audit");

        _logger = AuditLoggingStartupExtensions.CreateManualAuditLogger(
            serverIp: "/[::]:3901",
            environmentName: "PROD",
            applicationKey: "CIS:NotificationService",
            eamApplication: "NOBY",
            eamVersion: "3",
            hashSecretKey: auditConf["HashSecretKey"]!,
            databaseConnectionString: auditConf["ConnectionString"]!
            );
    }

    public async Task Aggregate()
    {
        var applicationLogs = await _dbContext.ApplicationLog
            .AsNoTracking()
            .Where(t => t.Level == "INFORMATION")
            .OrderBy(l => l.Timestamp)
            .ToListAsync();

        foreach ( var applicationLog in applicationLogs )
        {
            switch (applicationLog.LogType)
            {
                case LogType.ReceivedHttpRequest:
                    CreateLog(applicationLog, AuditEventTypes.Noby012, _msgNoby012, getObjectAfter(applicationLog.TraceId!, LogType.SendingHttpResponse));
                    break;
                case LogType.ProducingToKafka:
                    var produced = applicationLogs!.Any(t => t.TraceId == applicationLog.TraceId && t.LogType == LogType.ProducedToKafka);
                    if (produced)
                    {
                        CreateLog(applicationLog, AuditEventTypes.Noby013, _msgNoby013Success, getObjectAfter(applicationLog.TraceId!, LogType.ProducedToKafka));
                    } else
                    {
                        CreateLog(applicationLog, AuditEventTypes.Noby013, _msgNoby013Failed, null);
                    }                    
                    break;
                case LogType.ReceivedReport:
                    CreateLog(applicationLog, AuditEventTypes.Noby014, _msgNoby014, null);
                    break;
            }
        }

        await _dbContext.Database.ExecuteSqlRawAsync(@"Update NobyAudit.dbo.AuditEvent Set 
            [TimeStamp] = Cast(CONVERT(DATETIME2, JSON_VALUE(Detail,N'$.header.event.time.tsServer'), 127) As datetime),
            CreatedBy = 'VSSKB\XX_NOBY_SVC_USR_PROD',
            CreatedTime = Cast(CONVERT(DATETIME2, JSON_VALUE(Detail,N'$.header.event.time.tsServer'), 127) As datetime)");

        string getObjectAfter(string traceId, LogType logType)
            => applicationLogs!.First(t => t.TraceId == traceId && t.LogType == logType)!.ParsedObject!;
    }

    private void CreateLog(ApplicationLog applicationLog, AuditEventTypes auditEvent, string message, string? objectAfter)
    {
        Dictionary<string, string>? objBefore = string.IsNullOrEmpty(applicationLog.ParsedObject) ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(applicationLog.ParsedObject);
        Dictionary<string, string>? objAfter = string.IsNullOrEmpty(objectAfter) ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(objectAfter);

        _logger.Log(
            eventType: auditEvent,
            message: message,
            timestamp: applicationLog.Timestamp,
            correlation: string.IsNullOrEmpty(applicationLog.TraceId) ? "" : $"00-{applicationLog.TraceId}-{applicationLog.SpanId}-01",
            ipAddress: applicationLog.ClientIp,
            userIdent: null,
            sequenceId: null,
            identities: null,
            products: null,
            operation: null,
            result: null,
            bodyBefore: objBefore,
            bodyAfter: objAfter);
    }
}