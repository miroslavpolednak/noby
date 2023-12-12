using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Statistics;
using CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Statistics;

public class GetDetailedStatisticsHandler
    : IRequestHandler<GetDetailedStatisticsRequest, GetDetailedStatisticsResponse>
{
    public async Task<GetDetailedStatisticsResponse> Handle(GetDetailedStatisticsRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckReceiveStatisticsAccess();

        var dateFrom = request.StatisticsDate.Date;
        var dateTo = dateFrom.AddDays(1);

        var statistics = await _mediator.Send(new GetStatisticsRequest
        {
            Channels = request.Channels,
            States = request.States,
            TimeFrom = dateFrom,
            TimeTo = dateTo
        }, cancellationToken);

        var query = _dbContext.Results
            .Where(t => t.RequestTimestamp >= dateFrom && t.RequestTimestamp <= dateTo)
            .AsQueryable();

        if (request.States != null && request.States.Count > 0)
            query = query.Where(t => request.States.Contains(t.State));

        if (request.Channels != null && request.Channels.Count > 0)
            query = query.Where(t => request.Channels.Contains(t.Channel));

        var data = await query
            .Select(t => new Contracts.Statistics.Dto.Result
            {

            })
            .ToListAsync(cancellationToken);

        return new GetDetailedStatisticsResponse() { Statistics = statistics.Statistics };
    }

    private readonly NotificationDbContext _dbContext;
    private readonly IUserAdapterService _userAdapterService;
    private readonly IMediator _mediator;

    public GetDetailedStatisticsHandler(NotificationDbContext dbContext, IUserAdapterService userAdapterService, IMediator mediator)
    {
        _dbContext = dbContext;
        _userAdapterService = userAdapterService;
        _mediator = mediator;
    }
}
