using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.GetResult;

public class GetResultHandler : IRequestHandler<ResultGetRequest, ResultGetResponse>
{
    public async Task<ResultGetResponse> Handle(ResultGetRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new ResultGetResponse
        {
            NotificationId = "result notification id",
            Channel = NotificationChannel.Email,
            State = NotificationState.Sent,
            Errors = new List<ResultError>()
        };
    }
}