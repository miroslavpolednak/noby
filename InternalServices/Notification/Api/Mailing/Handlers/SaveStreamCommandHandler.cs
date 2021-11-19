using CIS.InternalServices.Notification.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CIS.InternalServices.Notification.Mailing.Handlers
{
    /*internal class SaveStreamCommandHandler : AsyncRequestHandler<MailingSaveStreamRequest>
    {
        private readonly Repositories.MailingRepository _repository;
        private readonly ILogger<SaveStreamCommandHandler> _logger;

        public SaveStreamCommandHandler(ILogger<SaveStreamCommandHandler> logger, Repositories.MailingRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        protected override Task Handle(MailingSaveStreamRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }*/
}
