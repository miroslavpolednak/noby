using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MassTransit;
using MassTransit.Mediator;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers;

// todo: change Mcs.SendEmail for Mpss.SendEmail
public class SendEmailConsumer : IConsumer<SendEmail>
{
    private readonly IMediator _mediator;

    public SendEmailConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        // todo:
        var request = new SendEmailConsumeRequest {  };
        await _mediator.Send(request);
    }
}