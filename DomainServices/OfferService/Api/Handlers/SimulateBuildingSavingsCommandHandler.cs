using DomainServices.OfferService.Contracts;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using CIS.Core.Results;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateBuildingSavingsCommandHandler 
    : IRequestHandler<Dto.SimulateBuildingSavingsMediatrRequest, SimulateBuildingSavingsResponse>
{
    public async Task<SimulateBuildingSavingsResponse> Handle(Dto.SimulateBuildingSavingsMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Run Savings simulation with {inputs}", request);

        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);
        var inputs = request.Request.InputData.ToContract();

        // spustit simulaci
        var result = getEasResult(await _easClient.RunSimulation(inputs));

        // transformovat
        var data = new Dto.Models.BuildingSavingsDataModel
        {
            Savings = result.ToBuildingSavingsData(),
            Loan = result.ToLoanData()
        };

        // ulozit do databaze
        int offerInstanceId = await _repository.SaveOffer(resourceProcessId, _configuration.BuldingSavingsProductInstanceType, request.Request.InputData, data);
        // ulozit splatkove kalendare
        await _repository.SaveSchedules(offerInstanceId, result.ToScheduleItems());

        _logger.LogInformation("Simulation #{id} created", offerInstanceId);

        // vytvorit
        return new SimulateBuildingSavingsResponse
        {
            BuildingSavings = data.Savings,
            Loan = data.Loan,
            OfferInstanceId = offerInstanceId
        };
    }

    private Eas.EasWrapper.ESBI_SIMULATION_RESULTS getEasResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<Eas.EasWrapper.ESBI_SIMULATION_RESULTS> r  when r.Model.SIM_error == 0 => r.Model,
            SuccessfulServiceCallResult<Eas.EasWrapper.ESBI_SIMULATION_RESULTS> r when r.Model.SIM_error != 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to EAS Simulation", 10011, new()
            {
                ("eassimerrorcode", r.Model.SIM_error.ToString()),
                ("eassimerrortext", r.Model.SIM_error_text)
            }),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private readonly Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly ILogger<SimulateBuildingSavingsCommandHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly AppConfiguration _configuration;

    public SimulateBuildingSavingsCommandHandler(
        AppConfiguration configuration,
        Repositories.SimulateBuildingSavingsRepository repository,
        ILogger<SimulateBuildingSavingsCommandHandler> logger,
        Eas.IEasClient easClient)
    {
        _configuration = configuration;
        _repository = repository;
        _logger = logger;
        _easClient = easClient;
    }
}
