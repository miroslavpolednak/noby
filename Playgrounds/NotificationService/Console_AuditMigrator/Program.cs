// See https://aka.ms/new-console-template for more information

using Console_AuditMigrator;
using Console_AuditMigrator.Database;
using Console_AuditMigrator.Database.Entities;
using Console_AuditMigrator.Services;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

Console.WriteLine("Starting.");

// setup
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddCommandLine(args)
    .Build();

var services = new ServiceCollection();

services
    .AddSingleton<IConfiguration>(configuration)
    .AddOptions<AppConfiguration>()
    .Bind(configuration.GetSection(nameof(AppConfiguration)));

var serviceProvider = services
    .AddServices(configuration)
    .BuildServiceProvider();

// services
var appConfiguration = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
var logParser = serviceProvider.GetRequiredService<ILogParser>();
var dbContext = serviceProvider.GetRequiredService<LogDbContext>();

// file processing
var files = Directory.GetFiles(appConfiguration.LogsFolder);

foreach (var file in files.Where(f => f.EndsWith(".log")))
{
    var logs = await logParser.ParseFile(file);

    var processedFile = new ProcessedFile
    {
        Timestamp = DateTime.Now,
        FileName = file
    };
    
    var receivedRequest = CreateApplicationLogs(logs, "Received HTTP Request", processedFile, LogType.ReceivedHttpRequest);
    var sendingResponse = CreateApplicationLogs(logs, "Sending HTTP Response", processedFile, LogType.SendingHttpResponse);
    var producing = CreateApplicationLogs(logs, "Producing message SendSMS", processedFile, LogType.ProducingToKafka);
    var produced = CreateApplicationLogs(logs, "Produced message SendSMS", processedFile, LogType.ProducedToKafka);
    var couldNot = CreateApplicationLogs(logs, "Could not produce message SendSMS", processedFile, LogType.CouldNotProduceToKafka);
    var receivedReport = CreateApplicationLogs(logs, "Received notification report", processedFile, LogType.ReceivedReport);
    
    Console.WriteLine($"File: {file}");
    Console.WriteLine($"receivedRequest:\t{receivedRequest.Count}");
    Console.WriteLine($"sendingResponse:\t{sendingResponse.Count}");
    Console.WriteLine($"producing:\t\t{producing.Count}");
    Console.WriteLine($"produced:\t\t{produced.Count}");
    Console.WriteLine($"couldNot:\t\t{couldNot.Count}");
    Console.WriteLine($"receivedReport:\t\t{receivedReport.Count}");
    Console.WriteLine("------------------------------------");

    await dbContext.ProcessedFiles.AddAsync(processedFile);
    await dbContext.ApplicationLogs.AddRangeAsync(receivedRequest);
    await dbContext.ApplicationLogs.AddRangeAsync(sendingResponse);
    await dbContext.ApplicationLogs.AddRangeAsync(producing);
    await dbContext.ApplicationLogs.AddRangeAsync(produced);
    await dbContext.ApplicationLogs.AddRangeAsync(couldNot);
    await dbContext.ApplicationLogs.AddRangeAsync(receivedReport);
}

await dbContext.SaveChangesAsync();
Console.WriteLine("Done.");

static List<ApplicationLog> CreateApplicationLogs(
    IList<Console_AuditMigrator.Models.ApplicationLog> logs,
    string startWith,
    ProcessedFile processedFile,
    LogType logType) => logs
    .Where(log => log.Message?.StartsWith(startWith) ?? false)
    .Select(log => CreateApplicationLog(log, processedFile, logType))
    .ToList();

static ApplicationLog CreateApplicationLog(
    Console_AuditMigrator.Models.ApplicationLog model,
    ProcessedFile processedFile,
    LogType logType) => new ()
{
    ProcessedFile = processedFile,
    LogType = logType,
    Timestamp = model.Timestamp!.Value,
    ThreadId = model.ThreadId,
    Level = model.Level,
    TraceId = model.TraceId,
    SpanId = model.SpanId,
    ParentId = model.ParentId,
    CisAppKey = model.CisAppKey,
    Version = model.Version,
    Assembly = model.Assembly,
    SourceContext = model.SourceContext,
    MachineName = model.MachineName,
    ClientIp = model.ClientIp,
    CisUserId = model.CisUserId,
    CisUserIdent = model.CisUserIdent,
    RequestId = model.RequestId,
    RequestPath = model.RequestPath,
    ConnectionId = model.ConnectionId,
    Message = model.Message,
    Exception = model.Exception
};