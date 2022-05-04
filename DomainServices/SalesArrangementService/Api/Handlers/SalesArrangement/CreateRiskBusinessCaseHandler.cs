using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Rip.V1;
using ExternalServices.Rip.V1.RipWrapper;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateRiskBusinessCaseHandler
    : IRequestHandler<Dto.CreateRiskBusinessCaseMediatrRequest, CreateRiskBusinessCaseResponse>
{
    public async Task<CreateRiskBusinessCaseResponse> Handle(Dto.CreateRiskBusinessCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateRiskBusinessCaseHandler), request.SalesArrangementId);

        // instance SA
        var saInstance = await _repository.GetSalesArrangementEntity(request.SalesArrangementId, cancellation);

        // kontroly
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(16000, $"Sales Arrangement #{request.SalesArrangementId} is not linked to Offer");

        if (!string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.AlreadyExists, $"Sales Arrangement #{request.SalesArrangementId} already contains RiskBusinessCaseId {saInstance.RiskBusinessCaseId}", 16000);

        // TODO: ziskat RBCID
        /*
        // System = "NOBY" ... přidat do konfigurace a do externí služby posílat jen SalesArrangementId ???
        var ripCreateRequest = new CreateRequest {
            LoanApplicationIdMp = new SystemId { Id = saInstance.SalesArrangementId.ToString(System.Globalization.CultureInfo.InvariantCulture), System = "NOBY" },
            ResourceProcessIdMp = "NOBY",
            ItChannel = "NOBY"
        };
        */

        var riskBusinessCaseId = saInstance.SalesArrangementId.ToString(System.Globalization.CultureInfo.InvariantCulture); //TODO: ServiceCallResult.ResolveAndThrowIfError<string>(await _ripClient.CreateRiskBusinesCase(ripCreateRequest));

        // ulozit ho do DB
        saInstance.RiskBusinessCaseId = riskBusinessCaseId;
        await _repository.SaveChangesAsync(cancellation);

        return new CreateRiskBusinessCaseResponse
        {
            RequestId = "zzzz",
            RiskBusinessCaseId = riskBusinessCaseId
        };
    }

    private async Task TestRipComputeCreditWorthiness()
    {
        /*
        curl --location --request PUT 'https://rip-sit1.vsskb.cz/v1/credit-worthiness' \
            --header 'Authorization: Basic YTph' \
            --header 'Content-Type: application/json' \
            --data-raw '{"ResourceProcessIdMp":"00ae56b0-d910-4072-ab6e-9f8b809ae390","ItChannel":"NOBY","RiskBusinessCaseIdMp":"9523467","HumanUser":{"Identity":"AADAMKOV","IdentityScheme":"KBAD"},"LoanApplicationProduct":{"Product":20001,"Maturity":300,"InterestRate":4.29,"AmountRequired":2000000,"Annuity":4987,"FixationPeriod":60},"Households":[{"ChildrenUnderAnd10":1,"ChildrenOver10":1,"ExpensesSummary":{"Rent":7500,"Saving":3500,"Insurance":2500,"Other":2000},"Clients":[{"IdMp":"5555555555555555251277105011","IsPartnerMp":false,"MaritalStatusMp":2,"LoanApplicationIncome":[{"CategoryMp":1,"Amount":42000},{"CategoryMp":2,"Amount":240000}],"CreditLiabilities":[{"LiabilityType":1,"Limit":0,"AmountConsolidated":0,"Installment":5823,"InstallmentConsolidated":0,"OutHomeCompanyFlag":true},{"LiabilityType":3,"Limit":40000,"AmountConsolidated":20000,"Installment":0,"InstallmentConsolidated":0,"OutHomeCompanyFlag":false}]}]}]}'
        */

        var arguments = new CreditWorthinessCalculationArguments {
            ResourceProcessIdMp = "00ae56b0-d910-4072-ab6e-9f8b809ae390",
            ItChannel = "NOBY",
            RiskBusinessCaseIdMp = "9523467",
            HumanUser = new HumanUser {
                Identity = "AADAMKOV", 
                IdentityScheme="KBAD"
            },
            LoanApplicationProduct = new LoanApplicationProduct {
                Product = 20001,
                Maturity = 300,
                InterestRate = 4.29,
                AmountRequired = 2000000,
                Annuity = 4987,
                FixationPeriod = 60
            },
            Households = new List<LoanApplicationHousehold> {
                new LoanApplicationHousehold {
                    ChildrenUnderAnd10 = 1,
                    ChildrenOver10=1,
                    ExpensesSummary = new ExpensesSummary
                    {
                        Rent = 7500,
                        Saving = 3500,
                        Insurance = 2500,
                        Other = 2000,
                    },
                    Clients = new List<LoanApplicationCounterParty> { 
                        new LoanApplicationCounterParty
                        {
                            IdMp = "",
                            IsPartnerMp = false,
                            MaritalStatusMp = 2,
                            LoanApplicationIncome = new List<LoanApplicationIncome>
                            {
                                new LoanApplicationIncome { CategoryMp = 1, Amount = 42000 },
                                new LoanApplicationIncome { CategoryMp = 2, Amount = 240000 },
                            },
                            CreditLiabilities = new List<CreditLiability>
                            {
                                new CreditLiability{ LiabilityType = 1, Limit = 0, AmountConsolidated = 0, Installment = 5823, InstallmentConsolidated = 0, OutHomeCompanyFlag = true },
                                new CreditLiability{ LiabilityType = 3, Limit = 40000, AmountConsolidated = 20000, Installment = 0, InstallmentConsolidated = 0, OutHomeCompanyFlag = false },
                            }
                        }

                    },
                }
            },
        };

        var calculation = await _ripClient.ComputeCreditWorthiness(arguments);
    }


    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;
    private readonly IRipClient _ripClient;

    public CreateRiskBusinessCaseHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateRiskBusinessCaseHandler> logger,
        IRipClient ripClient)
    {
        _repository = repository;
        _logger = logger;
        _ripClient = ripClient;
    }
}
