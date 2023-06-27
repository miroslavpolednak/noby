using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using DomainServices.CodebookService.Clients;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class ConsumeResultHandler : IRequestHandler<ResultConsumeRequest, ResultConsumeResponse>
{
    private readonly IServiceProvider _provider;
    private readonly IDateTime _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISmsAuditLogger _auditLogger;
    private readonly ILogger<ConsumeResultHandler> _logger;

    private static readonly Dictionary<string, NotificationState> _map = new()
    {
        { "INVALID", NotificationState.Invalid },
        { "UNSENT", NotificationState.Unsent },
        { "DELIVERED", NotificationState.Delivered },
        { "SENT", NotificationState.Sent }
    };

    public ConsumeResultHandler(
        IServiceProvider provider,
        IDateTime dateTime,
        ICodebookServiceClient codebookService,
        ISmsAuditLogger auditLogger,
        ILogger<ConsumeResultHandler> logger)
    {
        _provider = provider;
        _dateTime = dateTime;
        _codebookService = codebookService;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task<ResultConsumeResponse> Handle(ResultConsumeRequest request, CancellationToken cancellationToken)
    {
        var report = request.NotificationReport;
        if (!Guid.TryParse(report.id, out var id))
        {
            _logger.LogDebug("Skipped for notificationId: {id}", report.id);
            return new ResultConsumeResponse();
        }

        try
        {
            await using var scope = _provider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var result = await repository.GetResult(id, cancellationToken);
            result.ResultTimestamp = _dateTime.Now;
            result.State = _map[report.state];

            if (result is SmsResult smsResult)
            {
                var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
                var smsType = smsTypes.FirstOrDefault(s => s.Code == smsResult.Type);

                if (smsType?.IsAuditLogEnabled ?? false)
                {
                    _auditLogger.LogKafkaResultReceived(report);
                }
            }
            
            var errorCodes = report.notificationErrors?
                .Select(e => new ResultError
                {
                    Code = e.code,
                    Message = e.message
                })
                .ToHashSet() ?? Enumerable.Empty<ResultError>();
            
            var errorSet = new HashSet<ResultError>();
            errorSet.UnionWith(result.ErrorSet);
            errorSet.UnionWith(errorCodes);
            result.ErrorSet = errorSet;

            await repository.SaveChanges(cancellationToken);

            _logger.LogDebug($"Result updated for notificationId: {id}");
        }
        catch (CisNotFoundException)
        {
            _logger.LogDebug("Result not found for notificationId: {id}", report.id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update result failed for notificationId: {id}", report.id);
        }

        return new ResultConsumeResponse();
    }
}