using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using CIS.Infrastructure.gRPC.CisTypes;

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
        _logger.RequestHandlerStarted(nameof(SimulateMortgageHandler));

        // kontrola ProductTypeId (zda je typu Mortgage)
        await CheckProductTypeCategory(
            request.Request.Inputs.ProductTypeId,
            CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage
        );

        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);

        // setup input default values
        var inputs = SetUpInputDefaults(request.Request.Inputs);

        // get simulation outputs
        //var result = getEasResult(await _easClient.RunSimulation(inputs));
        var outputs = this.GenerateFakeSimulationData(inputs);

        // save to DB
        var entity = await _repository.SaveOffer(resourceProcessId, inputs.ProductTypeId, inputs, outputs, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ProductTypeId = entity.ProductTypeId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            Inputs = inputs,
            Outputs = outputs,
        };

    }

    private MortgageInput SetUpInputDefaults(MortgageInput input) 
    {
        input.ExpectedDateOfDrawing = input.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D
        input.PaymentDayOfTheMonth = input.PaymentDayOfTheMonth ?? 15;

        return input;
    }

    // TODO: redirect to EAS
    private MortgageOutput GenerateFakeSimulationData(MortgageInput input) {

        int loanDuration = (input.LoanDuration ?? 0);

        var output = new MortgageOutput
        {
            ProductTypeId = input.ProductTypeId,
            InterestRate = 0.02m,                           //mock: 0.02
            LoanAmount = input.LoanAmount,                  //mock: ze vstupu
            LoanDuration = loanDuration,                    //mock: 0 (pokud na vstupu nezadáno)
                                                            //mock: (náhodné číslo generované např. jako výše úvěru / splatností)
            LoanPaymentAmount = loanDuration == 0 ? 0 : input.LoanAmount / loanDuration,
            LoanToValue = input.LoanToValue,                //mock: ze vstupu
            LoanToCost = 0.0m,                               //mock: (celková výše investice / celková výše vlastních zdrojů) ... neznáme vlastní zdroje
            Aprc = 0.25m,                                    //mock: 0.25
            LoanTotalAmount = (input.LoanAmount + 1000000), //mock: (vstupní hodnota výše úvěru + 1 000 000)
            StatementTypeId = 1,                            //mock: 1
                                                            //mock: currentDate + 1D je defaultní hodnota při nezadání
            ExpectedDateOfDrawing = (input.ExpectedDateOfDrawing == null) ? new GrpcDate(DateTime.Now.AddDays(1)) : new GrpcDate(input.ExpectedDateOfDrawing.Year, input.ExpectedDateOfDrawing.Month, input.ExpectedDateOfDrawing.Day),
                                                            //mock: 15 je defaultní hodnota při nezadání
            PaymentDayOfTheMonth = input.PaymentDayOfTheMonth ?? 15,
        };

        output.LoanPurpose.AddRange(input.LoanPurpose);     //mock: ze vstupu

        return output;
    }

}