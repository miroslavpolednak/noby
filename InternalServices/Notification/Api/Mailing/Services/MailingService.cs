using Grpc.Core;
using CIS.InternalServices.Notification.Contracts;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace CIS.InternalServices.Notification.Mailing.Services
{
    public class MailingService : IMailingService
    {
        private readonly MediatR.IMediator _mediator;

        public MailingService(MediatR.IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Save(MailingSaveRequest request, CallContext context)
        {
            var model = new Dto.SaveRequest(request.Emails);
            await _mediator.Send(model);
        }

        public async Task SaveStream(MailingSaveStreamRequest request, CallContext context)
        {
            //await _mediator.Send(request);
        }
    }
}
