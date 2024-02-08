using CIS.Core.Attributes;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job2;

[ScopedService, SelfService]
internal sealed class Job2Handler
    : Scheduling.IJob
{
    public Task Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine("EXCEPTION chyba");
        throw new Exception("tohle je strasna chyba");
    }
}
