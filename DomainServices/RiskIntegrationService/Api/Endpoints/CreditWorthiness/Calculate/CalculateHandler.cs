namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<Contracts.CreditWorthiness.CalculateRequest, Contracts.CreditWorthiness.CalculateResponse>
{
    public async Task<Contracts.CreditWorthiness.CalculateResponse> Handle(Contracts.CreditWorthiness.CalculateRequest request, CancellationToken cancellation)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = await getRiskApplicationType(request.LoanApplicationProduct!.Product, cancellation);
        var maritalStatuses = await _codebookService.MaritalStatuses(cancellation);

        var requestModel = new Clients.CreditWorthiness.V1.Contracts.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = request.ToC4m(),
            ItChannel = Enum.Parse<Clients.CreditWorthiness.V1.Contracts.CreditWorthinessCalculationArgumentsItChannel>(request.ItChannel!),
            RiskBusinessCaseId = Convert.ToInt64(request.RiskBusinessCaseIdMp!),
            LoanApplicationProduct = request.LoanApplicationProduct.ToC4m(riskApplicationType.C4mAplCode),
            Households = request.Households!.Select(h => new Clients.CreditWorthiness.V1.Contracts.LoanApplicationHousehold
            {
                ChildrenOver10 = h.ChildrenOver10.GetValueOrDefault(),
                ChildrenUnderAnd10 = h.ChildrenUnderAnd10.GetValueOrDefault(),
                ExpensesSummary = (h.ExpensesSummary ?? new Contracts.CreditWorthiness.ExpensesSummary()).ToC4m(),
                Clients = (h.Clients ?? new(0)).ToC4m(riskApplicationType.MandantId, maritalStatuses)
            }).ToList()
        };

        // human user instance
        var dealer = await _xxvConnectionProvider.GetC4mUserInfo(request.HumanUser!.Identity, request.HumanUser.IdentityScheme, cancellation);
        if ((new[] { "KBAD", "MPAD" }).Contains(request.HumanUser.IdentityScheme))
            requestModel.KbGroupPerson = dealer.ToC4mKbPerson(request.HumanUser);
        else
            requestModel.LoanApplicationDealer = dealer.ToC4mDealer(request.HumanUser);

        // volani c4m
        var response = await _client.Calculate(requestModel, cancellation);

        return response.ToServiceResponse();
    }

    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw new CisValidationException(0, $"productTypeId={productTypeId} is missing in RiskApplicationTypes codebook");

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CalculateHandler(
        Clients.CreditWorthiness.V1.ICreditWorthinessClient client, 
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _client = client;
    }
}
