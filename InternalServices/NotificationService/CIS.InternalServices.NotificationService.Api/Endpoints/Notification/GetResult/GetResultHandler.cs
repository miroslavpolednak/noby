using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.GetResult;

public class GetResultHandler : IRequestHandler<ResultGetRequest, ResultGetResponse>
{
    private readonly NotificationRepository _repository;

    public GetResultHandler(NotificationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResultGetResponse> Handle(ResultGetRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.NotificationId, out var notificationId))
        {
            throw new CisValidationException(300, $"Could not parse notificationId: {notificationId}");
        }
        
        var notificationResult = await _repository.GetResult(notificationId, cancellationToken);

        return new ResultGetResponse
        {
            NotificationId = notificationResult.Id.ToString(),
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