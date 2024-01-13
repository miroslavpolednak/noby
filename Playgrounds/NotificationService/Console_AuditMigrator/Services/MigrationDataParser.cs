using System.Text.RegularExpressions;
using Console_AuditMigrator.Database;
using Console_AuditMigrator.Database.Entities;
using Console_AuditMigrator.Models;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Console_AuditMigrator.Services;

public class MigrationDataParser : IMigrationDataParser
{
    private readonly LogDbContext _dbContext;


    public MigrationDataParser(LogDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
    }

    public async Task ParseFromApplicationLogs()
    {
        var applicationLogs = await _dbContext.ApplicationLog
            .Where(t => t.Level == "INFORMATION")
            .OrderBy(l => l.Timestamp)
            .ToListAsync();

        foreach (var applicationLog in applicationLogs)
        {
            switch (applicationLog.LogType)
            {
                case LogType.ReceivedHttpRequest:
                    applicationLog.ParseHttpRequest();
                    break;
                case LogType.ProducingToKafka:
                    applicationLog.ParseProducing();
                    break;
                case LogType.ProducedToKafka:
                    applicationLog.ParseProduced();
                    break;
                case LogType.CouldNotProduceToKafka:
                    applicationLog.ParseCouldNotProduce();
                    break;
                case LogType.SendingHttpResponse:
                    applicationLog.ParseHttpResponse();
                    break;
                case LogType.ReceivedReport:
                    applicationLog.ParseReceivedReport();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}

internal static class MigrationDataParserExtensions
{
    internal static void ParseHttpRequest(this ApplicationLog applicationLog)
    {
        const string pathPattern = @"RequestPath: ""([^""]*)""";
        const string queryPattern = @"RequestQuery: ""([^""]*)""";
        const string headersPattern = @"RequestHeaders: \[(.*?)\],";
        const string bodyPattern = @"RequestBody: ""(.*)"", RequestContentType";

        var input = Regex.Replace(applicationLog.Message ?? string.Empty, @"\r\n?|\n", "");
        var pathMatch = Regex.Match(input, pathPattern);
        var queryMatch = Regex.Match(input, queryPattern);
        var headersMatch = Regex.Match(input, headersPattern);
        var bodyMatch = Regex.Match(input, bodyPattern);

        if (!pathMatch.Success || !queryMatch.Success || !headersMatch.Success || !bodyMatch.Success)
            throw new InvalidDataException();

        var path = pathMatch.Groups[1].Value;
        var query = queryMatch.Groups[1].Value;
        var headers = headersMatch.Groups[1].Value;
        var body = bodyMatch.Groups[1].Value;

        headers = "{ " 
            + headers
            .Replace("(\"","\"")
            .Replace("])", "]") 
            + "}";

        body = body
            .Replace("\\\"", @"""");

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string> {
            { "requestPath", path },
            { "requestQuery", query },
            { "rawHttpRequestHeaders", headers },
            { "rawHttpRequestBody", body }
        });
    }

    internal static void ParseProducing(this ApplicationLog applicationLog)
    {
        const string typePattern = @"with type '""([^""]*)""'";
        const string consumerPattern = @"by consumer '""([^""]*)""'.$";

        var input = applicationLog.Message ?? string.Empty;
        var typeMatch = Regex.Match(input, typePattern);
        var consumerMatch = Regex.Match(input, consumerPattern);

        if (!typeMatch.Success || !consumerMatch.Success)
            throw new InvalidDataException();

        var type = typeMatch.Groups[1].Value;
        var consumer = consumerMatch.Groups[1].Value;

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string> {
            { "smsType", type },
            { "consumer", consumer },
            { "identity", string.Empty },
            { "identityScheme", string.Empty },
            { "caseId", string.Empty },
            { "customId", string.Empty },
            { "documentId", string.Empty },
            { "documentHash", string.Empty },
            { "hashAlgorithm", string.Empty }
        });
    }

    internal static void ParseProduced(this ApplicationLog applicationLog)
    {
        const string notificationIdPattern = @"notification id '([^']*)'";

        var input = applicationLog.Message ?? string.Empty;
        var notificationIdMatch = Regex.Match(input, notificationIdPattern);

        if (!notificationIdMatch.Success)
            throw new InvalidDataException();

        var notificationId = notificationIdMatch.Groups[1].Value;

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            { "notificationId", notificationId }
        });
    }

    internal static void ParseCouldNotProduce(this ApplicationLog applicationLog)
    {
        const string typePattern = @"with type '""([^""]*)""'";
        const string consumerPattern = @"by consumer '""([^""]*)""'.$";

        var input = applicationLog.Message ?? string.Empty;
        var typeMatch = Regex.Match(input, typePattern);
        var consumerMatch = Regex.Match(input, consumerPattern);

        if (!typeMatch.Success || !consumerMatch.Success)
            throw new InvalidDataException();

        var type = typeMatch.Groups[1].Value;
        var consumer = consumerMatch.Groups[1].Value;

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string> {
            { "smsType", type },
            { "consumer", consumer },
            { "identity", string.Empty },
            { "identityScheme", string.Empty },
            { "caseId", string.Empty },
            { "customId", string.Empty },
            { "documentId", string.Empty },
            { "documentHash", string.Empty },
            { "hashAlgorithm", string.Empty }
        });
    }

    internal static void ParseHttpResponse(this ApplicationLog applicationLog)
    {
        const string bodyPattern = @"ResponseBody: ""(.*)"", ResponseContentType";

        var input = applicationLog.Message ?? string.Empty;
        var bodyMatch = Regex.Match(input, bodyPattern);

        if (!bodyMatch.Success)
            throw new InvalidDataException();

        var body = bodyMatch.Groups[1].Value;

        body = body
            .Replace("\\\"", @"""");

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string> {
            { "traceId", applicationLog.RequestId ?? string.Empty },
            { "responseStatus", "200" },
            { "rawHttpResponseBody", body }
        });
    }

    internal static void ParseReceivedReport(this ApplicationLog applicationLog)
    {
        const string notificationIdPattern = @"NotificationId: ""([^""]*)""";
        const string statePattern = @"State: ""([^""]*)""";

        var input = applicationLog.Message ?? string.Empty;
        var notificationIdMatch = Regex.Match(input, notificationIdPattern);
        var stateMatch = Regex.Match(input, statePattern);

        if (!notificationIdMatch.Success || !stateMatch.Success)
            throw new InvalidDataException();

        var notificationId = notificationIdMatch.Groups[1].Value;
        var state = stateMatch.Groups[1].Value;

        applicationLog.ParsedObject = JsonConvert.SerializeObject(new Dictionary<string, string> {
            { "smsType", string.Empty },
            { "notificationId", notificationId },
            { "state", state },
            { "errors", "null" }
        });
    }
}