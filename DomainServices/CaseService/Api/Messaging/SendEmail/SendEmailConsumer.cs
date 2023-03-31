using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.SendEmail;

internal class SendEmailConsumer : IConsumer<cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail>
{
    public Task Consume(ConsumeContext<cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail> context)
    {
        var s = "xxx";
        return Task.CompletedTask;
    }
}
