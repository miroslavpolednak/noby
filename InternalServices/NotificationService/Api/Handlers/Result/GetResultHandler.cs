using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class GetResultHandler : IRequestHandler<ResultGetRequest, ResultGetResponse>
{
    private readonly NotificationRepository _repository;

    public GetResultHandler(NotificationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResultGetResponse> Handle(ResultGetRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.GetResult(request.NotificationId, cancellationToken);
        
        // todo: error codes from codebook service
        return new ResultGetResponse
        {
            NotificationId = notificationResult.Id,
            Channel = notificationResult.Channel,
            State = notificationResult.State,
            Errors = notificationResult.ErrorSet.Select(e => new ResultError
            {
                Code = e,
                Message = "todo"
            }).ToList()
        };
    }
}