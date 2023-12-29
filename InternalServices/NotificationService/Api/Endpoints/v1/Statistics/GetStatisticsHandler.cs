using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Statistics;
using CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;
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
                Delivered = GetEmailCount(Contracts.Result.Dto.NotificationState.Delivered),
                Error = GetEmailCount(Contracts.Result.Dto.NotificationState.Error),
                InProgress = GetEmailCount(Contracts.Result.Dto.NotificationState.InProgress),
                Invalid = GetEmailCount(Contracts.Result.Dto.NotificationState.Invalid),
                Sent = GetEmailCount(Contracts.Result.Dto.NotificationState.Sent),
                Unsent = GetEmailCount(Contracts.Result.Dto.NotificationState.Unsent)
            },
            SMS = new Status 
            {
                Delivered = GetSmsCount(Contracts.Result.Dto.NotificationState.Delivered),
                Error = GetSmsCount(Contracts.Result.Dto.NotificationState.Error),
                InProgress = GetSmsCount(Contracts.Result.Dto.NotificationState.InProgress),
                Invalid = GetSmsCount(Contracts.Result.Dto.NotificationState.Invalid),
                Sent = GetSmsCount(Contracts.Result.Dto.NotificationState.Sent),
                Unsent = GetSmsCount(Contracts.Result.Dto.NotificationState.Unsent)
            }
        };

        return new GetStatisticsResponse() { Statistics = statistics };

        int? GetEmailCount(Contracts.Result.Dto.NotificationState state)
            => data.Where(t => t.Channel == Contracts.Result.Dto.NotificationChannel.Email && t.State == state).FirstOrDefault()?.Count;

        int? GetSmsCount(Contracts.Result.Dto.NotificationState state)
            => data.Where(t => t.Channel == Contracts.Result.Dto.NotificationChannel.Sms && t.State == state).FirstOrDefault()?.Count;
    }

    private readonly NotificationDbContext _dbContext;
    private readonly IUserAdapterService _userAdapterService;

    public GetStatisticsHandler(NotificationDbContext dbContext, IUserAdapterService userAdapterService)
    {
        _dbContext = dbContext;
        _userAdapterService = userAdapterService;
    }
}
