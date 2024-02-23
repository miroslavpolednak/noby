using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.ExecuteJob;

internal sealed class ExecuteJobHandler
    : IRequestHandler<ExecuteJobRequest, ExecuteJobResponse>
{
    public Task<ExecuteJobResponse> Handle(ExecuteJobRequest request, CancellationToken cancellation)
    {
        var result = _jobExecutor.EnqueueJob(Guid.Parse(request.JobId), null, request.JobData, cancellation);

        if (result.IsSucessful)
        {
            return Task.FromResult(new ExecuteJobResponse
            {
                TraceId = result.TraceId,
                ScheduleJobStatusId = result.ScheduleJobStatusId.ToString()
            });
        }
        else
        {
            throw new CIS.Core.Exceptions.CisValidationException(result.ErrorMessage!);
        }
    }

    private readonly JobExecutor _jobExecutor;
    
    public ExecuteJobHandler(JobExecutor jobExecutor)
    {
        _jobExecutor = jobExecutor;
    }
}
