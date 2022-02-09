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
        Repositories.OfferRepository repository,
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

        var resourceProcessId = await CheckResourceProcessId(request.Request.ResourceProcessId);
        //var inputs = request.Request.InputData.ToContract();

        // spustit simulaci
        //var result = getEasResult(await _easClient.RunSimulation(inputs));

        // transformovat
        var outputs = new Dto.Models.MortgageDataModel
        {
            Mortgage = this.GenerateFakeSimulationData(request.Request.InputData)
        };

        // ulozit do databaze
        int offerId = await _repository.SaveOffer(resourceProcessId, request.Request.InputData.ProductInstanceTypeId, request.Request.InputData, outputs);

        _logger.LogInformation("Simulation #{id} created", offerId);

        var entity = await _repository.Get(offerId);

        // vytvorit
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ProductInstanceTypeId = entity.ProductInstanceTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            //Created = ToCreated(entity),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            InputData = entity.Inputs.ToMortgageInput(),
            Mortgage = outputs.Mortgage,
        };

    }

    // TODO: redirect to EAS
    private MortgageData GenerateFakeSimulationData(MortgageInput input) {

        // neměnit vstup uživatele!
        // input.ExpectedDateOfDrawing = input.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D
        // input.PaymentDayOfTheMonth = input.PaymentDayOfTheMonth ?? 15;

        var output = new MortgageData
        {
            InterestRate = 0.02m,                            //mock: 0.02
            LoanAmount = input.LoanAmount,                  //mock: ze vstupu
            LoanDuration = input.LoanDuration ?? 0,         //mock: 0 (pokud na vstupu nezadáno)?
            LoanPaymentAmount =                             //mock: (náhodné číslo generované např. jako výše úvěru / splatností)
                input.LoanAmount / (input.LoanDuration ?? 1),
            LoanToValue = input.LoanToValue,                //mock: ze vstupu
            LoanToCost = 0.0m,                               //mock: (celková výše investice / celková výše vlastních zdrojů) ... neznáme vlastní zdroje
            Aprc = 0.25m,                                    //mock: 0.25
            LoanTotalAmount = (input.LoanAmount + 1000000), //mock: (vstupní hodnota výše úvěru + 1 000 000)
            StatementTypeId = 1,                            //mock: 1
        };

        output.LoanPurpose.AddRange(input.LoanPurpose);     //mock: ze vstupu

        return output;
    }

}
