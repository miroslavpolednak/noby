using CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job2;

internal sealed class Job2Handler
    : INotificationHandler<Job2Notification>
{
    public Task Handle(Job2Notification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("EXCEPTION chyba");
        throw new Exception("tohle je strasna chyba");
    }
}
