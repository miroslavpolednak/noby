using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class UpdateBasicDataHandler : IRequestHandler<UpdateBasicDataMediatrRequest, Empty>
    {
        private readonly ILogger<UpdateBasicDataHandler> _logger;
        private readonly MpHome.IMpHomeClient _mpHome;

        public UpdateBasicDataHandler(ILogger<UpdateBasicDataHandler> logger, MpHome.IMpHomeClient mpHome)
        {
            _logger = logger;
            _mpHome = mpHome;
        }

        public async Task<Empty> Handle(UpdateBasicDataMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run UpdateBasicData with {inputs}", request);

            (await _mpHome.UpdateBaseData(request.Request.Customer.ToMpHomePartnerBase(), request.Request.Identity)).CheckMpHomeResult();

            if (request.Request.Customer.IdentificationDocument != null)
                (await _mpHome.UpdateIdentificationDocument(request.Request.Customer.IdentificationDocument.ToMpHomeIdentificationDocument(), request.Request.Identity)).CheckMpHomeResult();

            return new Empty();
        }
    }
}
