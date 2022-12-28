using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class GetResultHandler : IRequestHandler<GetResultRequest, GetResultResponse>
{
    private readonly NotificationRepository _repository;

    public GetResultHandler(NotificationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<GetResultResponse> Handle(GetResultRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.GetResult(request.NotificationId, cancellationToken);
        
        return new GetResultResponse
        {
            NotificationId = notificationResult.Id,
            Channel = notificationResult.Channel,
            State = notificationResult.State,
            Errors = notificationResult.ErrorSet.ToList()
        };
    }
}