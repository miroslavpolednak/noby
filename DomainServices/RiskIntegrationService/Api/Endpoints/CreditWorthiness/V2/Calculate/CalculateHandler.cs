using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CreditWorthinessCalculateRequest, _V2.CreditWorthinessCalculateResponse>
{
    public async Task<_V2.CreditWorthinessCalculateResponse> Handle(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellation)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = await getRiskApplicationType(request.Product!.ProductTypeId, cancellation);
        var maritalStatuses = await _codebookService.MaritalStatuses(cancellation);
        var mainIncomeTypes = await _codebookService.IncomeMainTypes(cancellation);
        var obligationTypes = await _codebookService.ObligationTypes(cancellation);
        var liabilitiesFlatten = request.Households!.SelectMany(h => h.Customers!.Where(x => x.Obligations is not null).SelectMany(x => x.Obligations!)).ToList();

        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.Create("MPSS", "OM", "OfferInstance", request.ResourceProcessId),
            ItChannel = FastEnum.Parse<_C4M.CreditWorthinessCalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            //RiskBusinessCaseId = Convert.ToInt64(request.RiskBusinessCaseId!),
            LoanApplicationProduct = request.Product.ToC4m(riskApplicationType.C4mAplCode),
            Households = request.Households!.Select(h => new _C4M.LoanApplicationHousehold
            {
                ChildrenOver10 = h.ChildrenOverTenYearsCount,
                ChildrenUnderAnd10 = h.ChildrenUpToTenYearsCount,
                ExpensesSummary = (h.ExpensesSummary ?? new _V2.CreditWorthinessExpenses()).ToC4m(),
                Clients = h.Customers!.ToC4m(riskApplicationType.MandantId, maritalStatuses, mainIncomeTypes),
                CreditLiabilitiesSummary = liabilitiesFlatten.ToC4mCreditLiabilitiesSummary(obligationTypes.Where(o => o.Id == 3 || o.Id == 4)),
                CreditLiabilitiesSummaryOut = liabilitiesFlatten.ToC4mCreditLiabilitiesSummaryOut(obligationTypes.Where(o => o.Id == 3 || o.Id == 4)),
                InstallmentsSummary = liabilitiesFlatten.ToC4mInstallmentsSummary(obligationTypes.Where(o => o.Id == 1 || o.Id == 2)),
                InstallmentsSummaryOut = liabilitiesFlatten.ToC4mInstallmentsSummaryOut(obligationTypes.Where(o => o.Id == 1 || o.Id == 2))
            }).ToList()
        };

        // human user instance
        /*var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity!.IdentityId, request.UserIdentity.IdentityScheme, cancellation);
        if (Helpers.IsKbGroupPerson(request.UserIdentity.IdentityScheme))
            requestModel.KbGroupPerson = userInstance.ToC4mKbPerson(request.UserIdentity);
        else
            requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);*/

        // volani c4m
        var response = await _client.Calculate(requestModel, cancellation);

        return response.ToServiceResponse();
    }

    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw new CisValidationException(0, $"ProductTypeId={productTypeId} is missing in RiskApplicationTypes codebook");

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CalculateHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.CreditWorthiness.V1.ICreditWorthinessClient client, 
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _client = client;
    }
}
