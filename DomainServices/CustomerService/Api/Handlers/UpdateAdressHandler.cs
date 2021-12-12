using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class UpdateAdressHandler : IRequestHandler<UpdateAdressMediatrRequest, Empty>
    {
        private readonly ILogger<UpdateAdressHandler> _logger;
        private readonly MpHome.IMpHomeClient _mpHome;

        public UpdateAdressHandler(ILogger<UpdateAdressHandler> logger, MpHome.IMpHomeClient mpHome)
        {
            _logger = logger;
            _mpHome = mpHome;
        }

        public async Task<Empty> Handle(UpdateAdressMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run UpdateAdress with {inputs}", request);

            (await _mpHome.UpdateAddress(request.Request.Address.ToMpHomeAddress(), request.Request.Identity)).CheckMpHomeResult();

            return new Empty();
        }
    }
}
