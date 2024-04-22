using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Contracts.v2;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.GetDetailedStatistics;

internal sealed class GetDetailedStatisticsHandler(
    NotificationDbContext _dbContext,
    IMediator _mediator)
        : IRequestHandler<GetDetailedStatisticsRequest, GetDetailedStatisticsResponse>
{
    public async Task<GetDetailedStatisticsResponse> Handle(GetDetailedStatisticsRequest request, CancellationToken cancellationToken)
    {
        var dateFrom = request.StatisticsDate.ToDateTime().Date;
        var dateTo = dateFrom.AddDays(1);

        // statistiky
        var statisticsRequest = new GetStatisticsRequest
        {
            TimeFrom = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dateFrom),
            TimeTo = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dateTo)
        };
        if (request.Channels is not null)
        {
            statisticsRequest.Channels.AddRange(request.Channels);
        }
        if (request.States is not null)
        {
            statisticsRequest.States.AddRange(request.States);
        }

        var statistics = await _mediator.Send(statisticsRequest, cancellationToken);

        // notifikace
        var query = _dbContext.Notifications
            .Where(t => t.CreatedTime >= dateFrom && t.CreatedTime <= dateTo)
            .AsQueryable();

        if (request.States != null && request.States.Count > 0)
            query = query.Where(t => request.States.Contains(t.State));

        if (request.Channels != null && request.Channels.Count > 0)
            query = query.Where(t => request.Channels.Contains(t.Channel));

        var data = await query
            .OrderBy(t => t.CreatedTime)
            .Take(1000) // radsi omezit max?
            .ToListAsync(cancellationToken);

        var response = new GetDetailedStatisticsResponse()
        {
            Statistics = statistics.Statistics
        };
        response.Results.AddRange(data.Select(t => t.MapToResultDataV2()));
        return response;
    }
}
