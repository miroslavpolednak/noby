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
            var easResult = getEasResult(await _eas.CreateNewOrGetExisingClient(request.Request.ToEasKlientData()));
            //var easResult = new Eas.EasWrapper.S_KLIENTDATA { klient_id = 300500167 };

            // u cizincu aktualizovat data v modelu
            if (!string.IsNullOrEmpty(easResult.BirthNumber))
                request.Request.BirthNumber = easResult.BirthNumber;

            // poslat klienta do mphome
            (await _mpHome.Create(request.Request.ToMpHomePartner(), easResult.Id)).CheckMpHomeResult();

            return await Task.FromResult(new CreateResponse { Identity = easResult.Id });
        }

        private ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse getEasResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model,
            ErrorServiceCallResult r => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to EAS NewKlient", 10011, new()
            {
                ("eassimerrorcode", r.Errors.First().Key.ToString()),
                ("eassimerrortext", r.Errors.First().Message)
            }),
            _ => throw new NotImplementedException()
        };
    }
}
