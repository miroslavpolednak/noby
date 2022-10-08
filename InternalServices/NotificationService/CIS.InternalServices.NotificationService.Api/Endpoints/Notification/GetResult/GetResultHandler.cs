using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.GetResult;

public class GetResultHandler : IRequestHandler<ResultGetRequest, ResultGetResponse>
{
    private readonly IMemoryCache _memoryCache;

    public GetResultHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public async Task<ResultGetResponse> Handle(ResultGetRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        return _memoryCache.TryGetValue(request.NotificationId, out ResultGetResponse resultGetResponse)
            ? resultGetResponse
            : new ResultGetResponse
            {
                NotificationId = string.Empty,
                Channel = NotificationChannel.Unknown,
                State = NotificationState.Unknown,
                Errors = new List<ResultError>()
            };
    }
}