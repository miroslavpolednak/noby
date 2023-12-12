using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Statistics;
using CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Statistics;

public class GetStatisticsHandler
    : IRequestHandler<GetStatisticsRequest, GetStatisticsResponse>
{
    public async Task<GetStatisticsResponse> Handle(GetStatisticsRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckReceiveStatisticsAccess();

        var query = _dbContext.Results.AsQueryable();

        if (request.States != null && request.States.Count > 0)
            query = query.Where(t => request.States.Contains(t.State));

        if (request.Channels != null && request.Channels.Count > 0)
        {
            //query = query.Where(t => (new List<int> { 1, 2 }).Contains((int)t.Channel));
            //var i = new List<int> { 1, 2 };
            //query = query.Where(t => i.Contains((int)t.Channel));
            query = query.Where(t => request.Channels.Select(t => (int)t).ToList().Any(tt => tt == (int)t.Channel));
        }

        if (request.TimeFrom != null)
            query = query.Where(t => t.RequestTimestamp >= request.TimeFrom);

        if (request.TimeTo != null)
            query = query.Where(t => t.RequestTimestamp <= request.TimeTo);

        var data = await query
            .GroupBy(t => new { t.Channel, t.State })
            .Select(t => new
            {
                t.Key.Channel,
                t.Key.State,
                Count = t.Count()
            })
            .ToListAsync(cancellationToken);

        Contracts.Statistics.Dto.Statistics statistics = new()
        {
            Email = new Status
            {
                DELIVERED = GetEmailCount(Contracts.Result.Dto.NotificationState.Delivered),
                ERROR = GetEmailCount(Contracts.Result.Dto.NotificationState.Error),
                INPROGRESS = GetEmailCount(Contracts.Result.Dto.NotificationState.InProgress),
                INVALID = GetEmailCount(Contracts.Result.Dto.NotificationState.Invalid),
                SENT = GetEmailCount(Contracts.Result.Dto.NotificationState.Sent),
                UNSENT = GetEmailCount(Contracts.Result.Dto.NotificationState.Unsent)
            },
            SMS = new Status 
            {
                DELIVERED = GetSmsCount(Contracts.Result.Dto.NotificationState.Delivered),
                ERROR = GetSmsCount(Contracts.Result.Dto.NotificationState.Error),
                INPROGRESS = GetSmsCount(Contracts.Result.Dto.NotificationState.InProgress),
                INVALID = GetSmsCount(Contracts.Result.Dto.NotificationState.Invalid),
                SENT = GetSmsCount(Contracts.Result.Dto.NotificationState.Sent),
                UNSENT = GetSmsCount(Contracts.Result.Dto.NotificationState.Unsent)
            }
        };

        return new GetStatisticsResponse() { Statistics = statistics };

        int GetEmailCount(Contracts.Result.Dto.NotificationState state)
            => data.Where(t => t.Channel == Contracts.Result.Dto.NotificationChannel.Email && t.State == state).Count();

        int GetSmsCount(Contracts.Result.Dto.NotificationState state)
            => data.Where(t => t.Channel == Contracts.Result.Dto.NotificationChannel.Sms && t.State == state).Count();
    }

    private readonly NotificationDbContext _dbContext;
    private readonly IUserAdapterService _userAdapterService;

    public GetStatisticsHandler(NotificationDbContext dbContext, IUserAdapterService userAdapterService)
    {
        _dbContext = dbContext;
        _userAdapterService = userAdapterService;
    }
}
