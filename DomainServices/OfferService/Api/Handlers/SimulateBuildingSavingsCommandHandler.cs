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
        var timestamp = DateTime.Now;
        _logger.LogInformation("Run Savings simulation in {time} with {inputs}", timestamp, request);

        var simulationType = request.Request.InputData.IsWithLoan ? SimulationTypes.BuildingSavingsWithLoan : SimulationTypes.BuildingSavings;
        var inputs = request.Request.InputData.ToContract();

        if (simulationType == SimulationTypes.BuildingSavingsWithLoan)
        {
            inputs.USS_SimulovatUver = 1;
            inputs.USS_KodAkcie = request.Request.InputData.LoanActionCode.GetValueOrDefault(0);
        }

        // spustit simulaci
        var result = getEasResult(await _easClient.RunSimulation(inputs));

        // transformovat
        var savingsData = result.ToBuildingSavingsData();
        var loanData = result.ToLoanData();
        var scheduleItems = result.ToScheduleItems();

        // ulozit do databaze
        int modelationId = await _repository.Save(simulationType, timestamp, request.Request.InputData, savingsData, loanData, scheduleItems);

        _logger.LogInformation("Simulation #{id} created", modelationId);

        // vytvorit
        return new SimulateBuildingSavingsResponse
        {
            BuildingSavings = savingsData,
            Loan = loanData,
            OfferInstanceId = modelationId,
            InsertStamp = new(_userProvider.Get()?.Id ?? 0, timestamp)
        };
    }

    private Eas.EasWrapper.ESBI_SIMULATION_RESULTS getEasResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<Eas.EasWrapper.ESBI_SIMULATION_RESULTS> r  when r.Model.SIM_error == 0 => r.Model,
            SuccessfulServiceCallResult<Eas.EasWrapper.ESBI_SIMULATION_RESULTS> r when r.Model.SIM_error != 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, "Incorrect inputs to EAS Simulation", 10011, new()
            {
                ("eassimerrorcode", r.Model.SIM_error.ToString()),
                ("eassimerrortext", Uri.EscapeDataString(r.Model.SIM_error_text))
            }),
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

    private readonly Repositories.SimulateBuildingSavingsRepository _repository;
    private readonly ILogger<SimulateBuildingSavingsCommandHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly CIS.Core.Security.ICurrentUserProvider _userProvider;

    public SimulateBuildingSavingsCommandHandler(
        Repositories.SimulateBuildingSavingsRepository repository,
        ILogger<SimulateBuildingSavingsCommandHandler> logger,
        Eas.IEasClient easClient, CIS.Core.Security.
        ICurrentUserProvider userProvider)
    {
        _repository = repository;
        _logger = logger;
        _easClient = easClient;
        _userProvider = userProvider;
    }
}
