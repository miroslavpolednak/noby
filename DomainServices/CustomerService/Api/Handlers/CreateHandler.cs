using Grpc.Core;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using CIS.Core.Results;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class CreateHandler : IRequestHandler<CreateMediatrRequest, Contracts.CreateResponse>
    {
        private readonly ILogger<CreateHandler> _logger;
        private readonly MpHome.IMpHomeClient _mpHome;
        private readonly Eas.IEasClient _eas;

        public CreateHandler(ILogger<CreateHandler> logger, MpHome.IMpHomeClient mpHome, Eas.IEasClient eas)
        {
            _logger = logger;
            _mpHome = mpHome;
            _eas = eas;
        }

        public async Task<CreateResponse> Handle(CreateMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run Create with {inputs}", request);

            // vytvorit klienta v eas kvuli rezervaci id partnera
            var easResult = getEasResult(await _eas.NewKlient(request.Request.ToEasKlientData()));
            //var easResult = new ExternalServices.EAS.EasWrapper.S_KLIENTDATA { klient_id = 300495957 };

            // u cizincu aktualizovat data v modelu
            if (request.Request.EasTypKlienta() == EasKlientTypes.CizinecBezRc)
                request.Request.BirthNumber = easResult.rodne_cislo_ico;

            // poslat klienta do mphome
            await _mpHome.Create(request.Request.ToMpHomePartner(), easResult.klient_id);

            return await Task.FromResult(new CreateResponse { Identity = easResult.klient_id });
        }

        private Eas.EasWrapper.S_KLIENTDATA getEasResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<Eas.EasWrapper.S_KLIENTDATA> r when r.Model.return_val == 0 => r.Model,
            SuccessfulServiceCallResult<Eas.EasWrapper.S_KLIENTDATA> r when r.Model.return_val != 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to EAS NewKlient", 10011, new()
            {
                ("eassimerrorcode", r.Model.return_val.ToString()),
                ("eassimerrortext", r.Model.return_info)
            }),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };
    }
}
