using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using SharedTypes.GrpcTypes;
using DomainServices.OfferService.Api.Database;
using DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using Google.Protobuf;
using ExternalServices.EasSimulationHT.V1;
using System.Globalization;
using SharedComponents.DocumentDataStorage;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgage;

internal sealed class SimulateMortgageHandler
    : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        // setup input default values
        await setUpDefaults(request, cancellationToken);

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellationToken)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellationToken)).ToDictionary(i => i.Id);

        // get simulation outputs
        var easSimulationReq = request.SimulationInputs.ToEasSimulationRequest(request.BasicParameters, drawingDurationsById, drawingTypeById);
        var easSimulationRes = await _easSimulationHTClient.RunSimulationHT(easSimulationReq, cancellationToken);

        var additionalResults = easSimulationRes.ToAdditionalSimulationResults();

        // bonita
        MortgageCreditWorthinessSimpleResults? creditWorthinessResult = request.IsCreditWorthinessSimpleRequested switch
        {
            true => await calculateCreditWorthinessSimple(request, easSimulationRes, cancellationToken),
            _ => null
        };

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.Parse(request.ResourceProcessId),
            AdditionalSimulationResultsBin = additionalResults.ToByteArray(),
            AdditionalSimulationResults = Newtonsoft.Json.JsonConvert.SerializeObject(additionalResults),
            IsCreditWorthinessSimpleRequested = request.IsCreditWorthinessSimpleRequested,
            CreditWorthinessSimpleInputs = request.CreditWorthinessSimpleInputs is null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(request.CreditWorthinessSimpleInputs),
            CreditWorthinessSimpleInputsBin = request.CreditWorthinessSimpleInputs?.ToByteArray(),
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data simulace
        var documentEntity = _mapper.MapToData(request.BasicParameters, request.SimulationInputs, easSimulationRes);
        var id = await _documentDataStorage.Add(entity.OfferId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = request.BasicParameters,
            SimulationInputs = request.SimulationInputs,
            SimulationResults = results,
            AdditionalSimulationResults = additionalResults,
            CreditWorthinessSimpleResults = creditWorthinessResult
        };
    }

    private async Task setUpDefaults(SimulateMortgageRequest request, CancellationToken cancellation)
    {
        // basic params
        request.BasicParameters ??= new();
        request.BasicParameters.GuaranteeDateTo = ((DateTime)request.SimulationInputs.GuaranteeDateFrom).AddDays(AppDefaults.MaxGuaranteeInDays);
        request.BasicParameters.StatementTypeId ??= 1;   // Default: 1

        // inputs
        request.SimulationInputs.ExpectedDateOfDrawing ??= DateTime.Now.AddDays(1); //currentDate + 1D
        request.SimulationInputs.PaymentDay ??= (await _codebookService.PaymentDays(cancellation)).FirstOrDefault(i => i.IsDefault)?.PaymentDay ?? throw new CisNotFoundException(10002, $"Default 'PaymentDay' not found.");

        //Sleva z úrokové sazby(dle individuální cenotvorby)
        //Default: Pokud nepřijde na vstup, ukládáme hodnotu nula  "0"
        request.SimulationInputs.InterestRateDiscount ??= 0;

        // Typ čerpání
        // DrawingType(SB enum)
        // Default: "2" jednorázové
        request.SimulationInputs.DrawingTypeId ??= (int)SharedTypes.Enums.DrawingTypes.Disposable;

        //Je žádáno zaměstnanecké zvýhodnění?
        //Default: False
        request.SimulationInputs.IsEmployeeBonusRequested ??= false;

        var defaultFeeSettings = new FeeSettings() { FeeTariffPurpose = 0, IsStatementCharged = true };
        request.SimulationInputs.FeeSettings = defaultFeeSettings;

        // Určuje za jakým účelem se generuje seznam poplatků.
        // Default: 0 - za účelem nabídky
        request.SimulationInputs.FeeSettings.FeeTariffPurpose ??= defaultFeeSettings.FeeTariffPurpose;

        // Rizikové životní pojištění
        if (request.SimulationInputs.RiskLifeInsurance != null)
        {
            request.SimulationInputs.RiskLifeInsurance.Frequency ??= 1;         // Default: 1
        }

        // Pojištění nemovitosti
        if (request.SimulationInputs.RealEstateInsurance != null)
        {
            request.SimulationInputs.RealEstateInsurance.Frequency ??= 12;    // Default: 12
        }
    }

    private async Task<MortgageCreditWorthinessSimpleResults> calculateCreditWorthinessSimple(SimulateMortgageRequest request, SimulationHTResponse simulationResults, CancellationToken cancellationToken)
    {
        var kbCustomerIdentity = request.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        
        var creditWorthinessRequest = new CreditWorthinessSimpleCalculateRequest
        {
            PrimaryCustomerId = kbCustomerIdentity?.IdentityId.ToString(CultureInfo.InvariantCulture),
            ResourceProcessId = request.ResourceProcessId,
            ChildrenCount = request.CreditWorthinessSimpleInputs.ChildrenCount ?? 0,
            TotalMonthlyIncome = request.CreditWorthinessSimpleInputs.TotalMonthlyIncome,
            ExpensesSummary = request.CreditWorthinessSimpleInputs.ExpensesSummary is null
                ? null
                : new CreditWorthinessSimpleExpensesSummary
                {
                    Rent = request.CreditWorthinessSimpleInputs.ExpensesSummary.Rent,
                    Other = request.CreditWorthinessSimpleInputs.ExpensesSummary.Other
                },
            ObligationsSummary = request.CreditWorthinessSimpleInputs.ObligationsSummary is null
                ? null
                : new CreditWorthinessSimpleObligationsSummary
                {
                    CreditCardsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.CreditCardsAmount,
                    AuthorizedOverdraftsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.AuthorizedOverdraftsTotalAmount,
                    LoansInstallmentsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.LoansInstallmentsAmount,
                },
            Product = new CreditWorthinessProduct
            {
                ProductTypeId = request.SimulationInputs.ProductTypeId,
                LoanDuration = simulationResults.uverVysledky.splatnostUveru,
                LoanInterestRate = simulationResults.urokovaSazba.urokovaSazba,
                LoanAmount = (int)(decimal)simulationResults.uverVysledky.vyseUveru,
                LoanPaymentAmount = (int)((decimal?)simulationResults.uverVysledky.splatkaUveru ?? 0m),
                FixedRatePeriod = request.SimulationInputs.FixedRatePeriod ?? 0
            }
        };

        try
        {
            var result = await _creditWorthinessService.SimpleCalculate(creditWorthinessRequest, cancellationToken);

            return new MortgageCreditWorthinessSimpleResults
            {
                InstallmentLimit = (int)result.InstallmentLimit,
                MaxAmount = (int)result.MaxAmount,
                RemainsLivingAnnuity = (int?)result.RemainsLivingAnnuity,
                RemainsLivingInst = (int?)result.RemainsLivingInst,
                WorthinessResult = (WorthinessResult)result.WorthinessResult
            };
        }
        catch
        {
            return new MortgageCreditWorthinessSimpleResults { WorthinessResult = WorthinessResult.Unknown };
        }
    }

    private readonly Database.DocumentDataEntities.Mappers.OfferDataMapper _mapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IEasSimulationHTClient _easSimulationHTClient;
    private readonly ICreditWorthinessServiceClient _creditWorthinessService;
    private readonly OfferServiceDbContext _dbContext;

    public SimulateMortgageHandler(
        Database.DocumentDataEntities.Mappers.OfferDataMapper mapper,
        IDocumentDataStorage documentDataStorage,
        OfferServiceDbContext dbContext,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceClient codebookService,
        IEasSimulationHTClient easSimulationHTClient,
        ICreditWorthinessServiceClient creditWorthinessService
    )
    {
        _mapper = mapper;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
        _creditWorthinessService = creditWorthinessService;
        _dbContext = dbContext;
    }
}
