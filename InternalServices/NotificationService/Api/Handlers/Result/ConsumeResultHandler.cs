using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class ConsumeResultHandler : IRequestHandler<ResultConsumeRequest, ResultConsumeResponse>
{
    private readonly NotificationRepository _repository;
    private readonly ILogger<ConsumeResultHandler> _logger;

    public ConsumeResultHandler(
        NotificationRepository repository,
        ILogger<ConsumeResultHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    
    public async Task<ResultConsumeResponse> Handle(ResultConsumeRequest request, CancellationToken cancellationToken)
    {
        var notificationReport = request.NotificationReport;
        if (!Guid.TryParse(notificationReport.id, out var notificationId))
        {
            _logger.LogInformation("Skipped for notificationId: {id}", notificationReport.id);
        }

        try
        {
            // todo: consume state and errors from request
            var notificationResult = await _repository.UpdateResult(
                notificationId,
                NotificationState.Delivered,
                new HashSet<string>(),
                cancellationToken);
        }
        catch (CisNotFoundException)
        {
            _logger.LogInformation("Skipped for notificationId: {id}", notificationReport.id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed for notificationId: {id}", notificationReport.id);
            throw;
        }

        return new ResultConsumeResponse();
    }

}