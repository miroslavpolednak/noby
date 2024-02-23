using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job2;

internal sealed class Job2Handler
    : IJob
{
    public Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        Console.WriteLine("Test job 2 chyba");
        throw new Exception("Tohle je strasna chyba jobu 2");
    }
}
