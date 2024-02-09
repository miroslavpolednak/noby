using CIS.Core.Attributes;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job2;

internal sealed class Job2Handler
    : IJob
{
    public Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        Console.WriteLine("EXCEPTION chyba");
        throw new Exception("tohle je strasna chyba");
    }
}
