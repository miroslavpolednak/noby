using CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Requests;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Handlers;

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
        var notificationReport = request.Message.Value;
        if (!Guid.TryParse(notificationReport.id, out var notificationId))
        {
            _logger.LogError("Could not parse notificationId: {id}", notificationReport.id);
        }
        
        var notificationResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Delivered,
            new HashSet<string>(),
            cancellationToken);

        return new ResultConsumeResponse();
    }

}