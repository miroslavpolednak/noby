using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Statistics;
using CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Statistics;

internal sealed class GetStatisticsHandler
    : IRequestHandler<GetStatisticsRequest, GetStatisticsResponse>
{
    public async Task<GetStatisticsResponse> Handle(GetStatisticsRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckReceiveStatisticsAccess();

        var query = _dbContext.Results.AsQueryable();

        if (request.States != null && request.States.Count > 0)
            query = query.Where(t => request.States.Contains(t.State));

        if (request.Channels != null && request.Channels.Count > 0)
            query = query.Where(t => request.Channels.Contains(t.Channel));

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
                Delivered = getEmailCount(NotificationState.Delivered),
                Error = getEmailCount(NotificationState.Error),
                InProgress = getEmailCount(NotificationState.InProgress),
                Invalid = getEmailCount(NotificationState.Invalid),
                Sent = getEmailCount(NotificationState.Sent),
                Unsent = getEmailCount(NotificationState.Unsent)
            },
            SMS = new Status 
            {
                Delivered = getSmsCount(NotificationState.Delivered),
                Error = getSmsCount(NotificationState.Error),
                InProgress = getSmsCount(NotificationState.InProgress),
                Invalid = getSmsCount(NotificationState.Invalid),
                Sent = getSmsCount(NotificationState.Sent),
                Unsent = getSmsCount(NotificationState.Unsent)
            }
        };

        return new GetStatisticsResponse() { Statistics = statistics };

        int? getEmailCount(NotificationState state)
            => data.Where(t => t.Channel == NotificationChannel.Email && t.State == state).FirstOrDefault()?.Count ?? getDefault(state);

        int? getSmsCount(NotificationState state)
            => data.Where(t => t.Channel == NotificationChannel.Sms && t.State == state).FirstOrDefault()?.Count ?? getDefault(state);

        int? getDefault(NotificationState state)
            => request.States == null || request.States.Contains(state) ? 0 : null;
    }

    private readonly NotificationDbContext _dbContext;
    private readonly IUserAdapterService _userAdapterService;

    public GetStatisticsHandler(NotificationDbContext dbContext, IUserAdapterService userAdapterService)
    {
        _dbContext = dbContext;
        _userAdapterService = userAdapterService;
    }
}
