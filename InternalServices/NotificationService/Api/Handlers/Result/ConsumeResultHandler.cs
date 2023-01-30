using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class ConsumeResultHandler : IRequestHandler<ResultConsumeRequest, ResultConsumeResponse>
{
    private readonly IDateTime _dateTime;
    private readonly NotificationRepository _repository;
    private readonly ILogger<ConsumeResultHandler> _logger;

    private static readonly Dictionary<string, NotificationState> _map = new()
    {
        { "INVALID", NotificationState.Invalid },
        { "UNSENT", NotificationState.Unsent },
        { "DELIVERED", NotificationState.Delivered },
        { "SENT", NotificationState.Sent }
    };

    public ConsumeResultHandler(
        IDateTime dateTime,
        NotificationRepository repository,
        ILogger<ConsumeResultHandler> logger)
    {
        _dateTime = dateTime;
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultConsumeResponse> Handle(ResultConsumeRequest request, CancellationToken cancellationToken)
    {
        var report = request.NotificationReport;
        if (!Guid.TryParse(report.id, out var id))
        {
            _logger.LogDebug("Skipped for notificationId: {id}", report.id);
        }

        try
        {
            var result = await _repository.GetResult(id, cancellationToken);
            result.HandoverToMcsTimestamp = _dateTime.Now;
            result.State = _map[report.state];

            var errorCodes = report.notificationErrors?
                .Select(e => e.code)
                .ToHashSet() ?? Enumerable.Empty<string>();
            
            var errorSet = new HashSet<string>();
            errorSet.UnionWith(result.ErrorSet);
            errorSet.UnionWith(errorCodes);
            result.ErrorSet = errorSet;

            await _repository.SaveChanges(cancellationToken);
            
            _logger.LogInformation($"Result updated for notificationId: {id}");
        }
        catch (CisNotFoundException)
        {
            _logger.LogDebug("Not found for notificationId: {id}", report.id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed for notificationId: {id}", report.id);
            throw new CisException(399, "todo");
        }

        return new ResultConsumeResponse();
    }
}