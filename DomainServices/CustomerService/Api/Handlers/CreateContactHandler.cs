using Grpc.Core;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.ExternalServices.MpHome;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class CreateContactHandler : IRequestHandler<CreateContactMediatrRequest, Contracts.CreateContactResponse>
    {
        private readonly ILogger<CreateContactHandler> _logger;
        private readonly ExternalServices.MpHome.IMpHomeClient _mpHome;

        public CreateContactHandler(ILogger<CreateContactHandler> logger, ExternalServices.MpHome.IMpHomeClient mpHome)
        {
            _logger = logger;
            _mpHome = mpHome;
        }

        public async Task<CreateContactResponse> Handle(CreateContactMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run CreateContact with {inputs}", request);

            var response = await _mpHome.CreateContact(request.Request.Contact.ToMpHomeContactData(), request.Request.Identity);

            return new CreateContactResponse { ContactId = response };
        }
    }
}
