using CIS.InternalServices.Notification.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CIS.InternalServices.Notification.Mailing.Handlers
{
    internal class SaveCommandHandler : AsyncRequestHandler<Dto.SaveRequest>
    {
        private readonly Repositories.MailingRepository _repository;
        private readonly ILogger<SaveCommandHandler> _logger;

        public SaveCommandHandler(ILogger<SaveCommandHandler> logger, Repositories.MailingRepository repository)
        {
            this._repository = repository;
            this._logger = logger;
        }

        protected override Task Handle(Dto.SaveRequest request, CancellationToken cancellationToken)
        {

            throw new System.NotImplementedException();
        }
    }
}
