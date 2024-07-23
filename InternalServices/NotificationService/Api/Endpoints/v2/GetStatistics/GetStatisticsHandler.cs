using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Contracts.v2;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.GetStatistics;

internal sealed class GetStatisticsHandler(NotificationDbContext _dbContext)
        : IRequestHandler<GetStatisticsRequest, GetStatisticsResponse>
{
    public async Task<GetStatisticsResponse> Handle(GetStatisticsRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Notifications.AsQueryable();

        if (request.States != null && request.States.Count > 0)
            query = query.Where(t => request.States.Contains(t.State));

        if (request.Channels != null && request.Channels.Count > 0)
            query = query.Where(t => request.Channels.Contains(t.Channel));

        if (request.TimeFrom != null)
        {
            var d1 = request.TimeFrom.ToDateTime();
            query = query.Where(t => t.CreatedTime >= d1);
        }

        if (request.TimeTo != null)
        {
            var d2 = request.TimeTo.ToDateTime();
            query = query.Where(t => t.CreatedTime <= d2);
        }

        var data = await query
            .GroupBy(t => new { t.Channel, t.State })
            .Select(t => new
            {
                t.Key.Channel,
                t.Key.State,
                Count = t.Count()
            })
            .ToListAsync(cancellationToken);

        StatisticsData statistics = new()
        {
            Email = new()
            {
                Delivered = getEmailCount(NotificationStates.Delivered),
                Error = getEmailCount(NotificationStates.Error),
                InProgress = getEmailCount(NotificationStates.InProgress),
                Invalid = getEmailCount(NotificationStates.Invalid),
                Sent = getEmailCount(NotificationStates.Sent),
                Unsent = getEmailCount(NotificationStates.Unsent)
            },
            SMS = new()
            {
                Delivered = getSmsCount(NotificationStates.Delivered),
                Error = getSmsCount(NotificationStates.Error),
                InProgress = getSmsCount(NotificationStates.InProgress),
                Invalid = getSmsCount(NotificationStates.Invalid),
                Sent = getSmsCount(NotificationStates.Sent),
                Unsent = getSmsCount(NotificationStates.Unsent)
            }
        };

        return new GetStatisticsResponse() { Statistics = statistics };

        int? getEmailCount(NotificationStates state)
            => data.Where(t => t.Channel == NotificationChannels.Email && t.State == state).FirstOrDefault()?.Count ?? getDefault(state);

        int? getSmsCount(NotificationStates state)
            => data.Where(t => t.Channel == NotificationChannels.Sms && t.State == state).FirstOrDefault()?.Count ?? getDefault(state);

        int? getDefault(NotificationStates state)
            => request.States == null || request.States.Contains(state) ? 0 : null;
    }
}
