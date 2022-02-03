using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateMortgageHandler
    : BaseHandler, IRequestHandler<Dto.SimulateMortgageMediatrRequest, SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly Eas.IEasClient _easClient;

    public SimulateMortgageHandler(
        Repositories.OfferInstanceRepository repository,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient
        ) : base(repository, codebookService)
    {
        _logger = logger;
        _easClient = easClient;
    }

    #endregion

    public async Task<SimulateMortgageResponse> Handle(Dto.SimulateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Run Mortgage simulation with {inputs}", request);

        // kontrola ProductInstanceTypeId (zda je typu Mortgage)
        await checkProductInstanceTypeCategory(
            request.Request.InputData.ProductInstanceTypeId,
            CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.Mortgage
        );

        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);
        //var inputs = request.Request.InputData.ToContract();

        // spustit simulaci
        //var result = getEasResult(await _easClient.RunSimulation(inputs));

        // transformovat
        var data = new Dto.Models.MortgageDataModel
        {
            Mortgage = this.GenerateFakeSimulationData(request.Request.InputData)
        };

        // ulozit do databaze
        int offerInstanceId = await _repository.SaveOffer(resourceProcessId, request.Request.InputData.ProductInstanceTypeId, request.Request.InputData, data);

        _logger.LogInformation("Simulation #{id} created", offerInstanceId);

        // vytvorit
        return new SimulateMortgageResponse
        {
            OfferInstanceId = offerInstanceId,
            Mortgage = data.Mortgage,
        };
    }

    // TODO: redirect to EAS
    private MortgageData GenerateFakeSimulationData(MortgageInput input) {
        return new MortgageData
        {
            InterestRate = 0,
            LoanAmount = 0,
            LoanDuration = 0,
            LoanPaymentAmount = 0,
            EmployeeBonusLoanCode = 0,
            Ltv = 0,
            Aprc = 0,
            //LoanPurpose = new Google.Protobuf.Collections.RepeatedField<LoanPurpose>(),
            StatementTypeId = 0,
        };
    }

}
