using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class CreateContactHandler : IRequestHandler<CreateContactMediatrRequest, Contracts.CreateContactResponse>
    {
        private readonly ILogger<CreateContactHandler> _logger;
        private readonly MpHome.IMpHomeClient _mpHome;

        public CreateContactHandler(ILogger<CreateContactHandler> logger, MpHome.IMpHomeClient mpHome)
        {
            _logger = logger;
            _mpHome = mpHome;
        }

        public async Task<CreateContactResponse> Handle(CreateContactMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run CreateContact with {inputs}", request);

            var response = (await _mpHome.CreateContact(request.Request.Contact.ToMpHomeContactData(), request.Request.Identity)).ToMpHomeResult<MpHome.MpHomeWrapper.ContactIdResponse>();
            
            return new CreateContactResponse { ContactId = (int)response.ContactId };
        }

        
    }
}
