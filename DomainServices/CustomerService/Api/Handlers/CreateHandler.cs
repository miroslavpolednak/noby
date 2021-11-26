using Grpc.Core;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.ExternalServices.MpHome;
using DomainServices.CustomerService.Api.ExternalServices.EAS;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class CreateHandler : IRequestHandler<CreateMediatrRequest, Contracts.CreateResponse>
    {
        private readonly ILogger<CreateHandler> _logger;
        private readonly ExternalServices.MpHome.IMpHomeClient _mpHome;
        private readonly ExternalServices.EAS.IEasClient _eas;

        public CreateHandler(ILogger<CreateHandler> logger, ExternalServices.MpHome.IMpHomeClient mpHome, ExternalServices.EAS.IEasClient eas)
        {
            _logger = logger;
            _mpHome = mpHome;
            _eas = eas;
        }

        public async Task<CreateResponse> Handle(CreateMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run Create with {inputs}", request);

            // vytvorit klienta v eas kvuli rezervaci id partnera
            //var easResult = await _eas.NewKlient(request.ToKlientData());
            var easResult = new ExternalServices.EAS.EasWrapper.S_KLIENTDATA { klient_id = 300495957 };

            // zkontrolovat chybu z eas
            if (easResult.return_val != 0)
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to EAS NewKlientAsync", 10011, new()
                {
                    ("eassimerrorcode", easResult.return_info.ToString()),
                    ("eassimerrortext", easResult.return_info)
                });

            // u cizincu aktualizovat data v modelu
            if (request.Request.EasTypKlienta() == EasKlientTypes.CizinecBezRc)
                request.Request.BirthNumber = easResult.rodne_cislo_ico;

            // poslat klienta do mphome
            await _mpHome.Create(request.Request.ToMpHomePartner(), easResult.klient_id);

            return await Task.FromResult(new CreateResponse { Identity = 123 });
        }
    }
}
