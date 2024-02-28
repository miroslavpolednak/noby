using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetTriggers;

internal sealed class GetTriggersHandler
    : IRequestHandler<GetTriggersRequest, GetTriggersResponse>
{
    public async Task<GetTriggersResponse> Handle(GetTriggersRequest request, CancellationToken cancellation)
    {
        var result = await _connectionProvider.ExecuteDapperRawSqlToListAsync<Trigger>(_sql, cancellation);

        var response = new GetTriggersResponse();
        response.Triggers.AddRange(result.Select(t =>
        {
            string description = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(t.Cron, new CronExpressionDescriptor.Options
            {
                DayOfWeekStartIndexZero = true,
                Use24HourTimeFormat = true
            });

            return new GetTriggersResponse.Types.Trigger
            {
                TriggerId = t.ScheduleTriggerId.ToString(),
                TriggerName = t.TriggerName,
                CronExpression = t.Cron,
                CronExpressionText = description,
                JobId = t.ScheduleJobId.ToString(),
                JobName = t.JobName,
                JobType = t.JobType,
                IsDisabled = t.IsDisabled
            };
        }));
        return response;
    }

    sealed class Trigger
    {
        public Guid ScheduleTriggerId { get; set; }
        public Guid ScheduleJobId { get; set; }
        public string JobName { get; set; } = null!;
        public string JobType { get; set; } = null!;
        public string TriggerName { get; set; } = null!;
        public string Cron { get; set; } = null!;
        public bool IsDisabled { get; set; }
    }

    private const string _sql = "SELECT A.*, B.JobName, B.JobType FROM [dbo].[ScheduleTrigger] A INNER JOIN [dbo].[ScheduleJob] B ON A.ScheduleJobId=B.ScheduleJobId";

    private readonly Core.Data.IConnectionProvider _connectionProvider;

    public GetTriggersHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
