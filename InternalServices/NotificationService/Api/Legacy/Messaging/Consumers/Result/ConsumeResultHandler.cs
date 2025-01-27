﻿using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Database.Entities;
using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using DomainServices.CodebookService.Clients;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Result;

public class ConsumeResultHandler : IRequestHandler<ConsumeResultRequest, ConsumeResultResponse>
{
    private readonly IServiceProvider _provider;
    private readonly TimeProvider _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISmsAuditLogger _smsAuditLogger;
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
        TimeProvider dateTime,
        ICodebookServiceClient codebookService,
        ISmsAuditLogger smsAuditLogger,
        ILogger<ConsumeResultHandler> logger)
    {
        _provider = provider;
        _dateTime = dateTime;
        _codebookService = codebookService;
        _smsAuditLogger = smsAuditLogger;
        _logger = logger;
    }

    public async Task<ConsumeResultResponse> Handle(ConsumeResultRequest request, CancellationToken cancellationToken)
    {
        var report = request.NotificationReport;
     
        try
        {
            await using var scope = _provider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var result = await repository.GetResult(request.Id, cancellationToken);
            result.ResultTimestamp = _dateTime.GetLocalNow().DateTime;
            result.State = _map[report.state];

            if (result is SmsResult smsResult)
            {
                var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
                var smsType = smsTypes.First(s => s.Code == smsResult.Type);
                _smsAuditLogger.LogKafkaResultReceived(smsType, report, request.Id);
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

            _logger.LogDebug($"Result updated for notificationId: {request.Id}");
        }
        catch (CisNotFoundException)
        {
            _logger.LogTrace("Result not found for notificationId: {id}", request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update result failed for notificationId: {id}", request.Id);
        }

        return new ConsumeResultResponse();
    }
}