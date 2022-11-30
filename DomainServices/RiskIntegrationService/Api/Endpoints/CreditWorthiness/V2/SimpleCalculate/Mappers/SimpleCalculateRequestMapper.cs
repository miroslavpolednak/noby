using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class SimpleCalculateRequestMapper
{
    public async Task<_C4M.CreditWorthinessCalculationArguments> MapToC4m(
        _V2.CreditWorthinessSimpleCalculateRequest request, 
        CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellation)
    {
        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId!),
            ItChannel = FastEnum.Parse<_C4M.CreditWorthinessCalculationArgumentsItChannel>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4mAplCode,
                AmountRequired = request.Product!.LoanAmount,
                Annuity = request.Product.LoanPaymentAmount,
                FixationPeriod = request.Product.FixedRatePeriod,
                InterestRate = request.Product.LoanInterestRate,
                Maturity = request.Product.LoanDuration
            }
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
                requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
            else
                requestModel.KbGroupPerson = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
        }

        // client
        var counterParty = new _C4M.LoanApplicationCounterParty
        {
            Id = string.IsNullOrEmpty(request.PrimaryCustomerId) ? null : _C4M.ResourceIdentifier.CreateResourceCounterParty(request.PrimaryCustomerId, !riskApplicationType.MandantId.HasValue || (CIS.Foms.Enums.Mandants)riskApplicationType.MandantId == CIS.Foms.Enums.Mandants.Kb ? "KBCZ" : "MPSS"),
            IsPartner = 0,
            MaritalStatus = _C4M.LoanApplicationCounterPartyMaritalStatus.M,
            LoanApplicationIncome = createIncome(request.TotalMonthlyIncome)
        };

        // household
        var household = new _C4M.LoanApplicationHousehold
        {
            ChildrenOver10 = request.ChildrenCount,
            ExpensesSummary = (request.ExpensesSummary ?? new _V2.CreditWorthinessSimpleExpensesSummary()).ToC4M(),
            CreditLiabilitiesSummary = createCreditLiabilitiesSummary(),
            CreditLiabilitiesSummaryOut = createCreditLiabilitiesSummaryOut(request.ObligationsSummary?.AuthorizedOverdraftsAmount, request.ObligationsSummary?.CreditCardsAmount),
            InstallmentsSummary = createInstallmentsSummary(),
            InstallmentsSummaryOut = createInstallmentsSummaryOut(request.ObligationsSummary?.LoansInstallmentsAmount),
            Clients = new List<_C4M.LoanApplicationCounterParty> { counterParty }
        };

        requestModel.Households = new List<_C4M.LoanApplicationHousehold>() { household };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
                requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
            else
                requestModel.KbGroupPerson = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
        }

        return requestModel;
    }

    #region liabilities installments
    private static List<_C4M.CreditLiabilitiesSummaryHomeCompany> createCreditLiabilitiesSummary()
        => new()
        {
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup.AD, Amount = 0, AmountConsolidated = 0 },
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup.CC, Amount = 0, AmountConsolidated = 0 }
        };

    private static List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummaryOut(decimal? authorizedOverdraftsTotalAmount, decimal? creditCardsTotalAmount)
        => new()
        {
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryProductGroup.AD, Amount = authorizedOverdraftsTotalAmount.GetValueOrDefault(), AmountConsolidated = 0 },
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryProductGroup.CC, Amount = creditCardsTotalAmount.GetValueOrDefault(), AmountConsolidated = 0 }
        };

    private static List<_C4M.InstallmentsSummaryHomeCompany> createInstallmentsSummary()
        => new()
        {
            new() { ProductGroup = _C4M.InstallmentsSummaryHomeCompanyProductGroup.CL, Amount = 0, AmountConsolidated = 0 },
            new() { ProductGroup = _C4M.InstallmentsSummaryHomeCompanyProductGroup.ML, Amount = 0, AmountConsolidated = 0 }
        };

    private static List<_C4M.InstallmentsSummaryOutHomeCompany> createInstallmentsSummaryOut(decimal? loansTotalInstallments)
        => new()
        {
            new() { ProductGroup = _C4M.InstallmentsSummaryOutHomeCompanyProductGroup.CL, Amount = loansTotalInstallments.GetValueOrDefault(), AmountConsolidated = 0 },
            new() { ProductGroup = _C4M.InstallmentsSummaryOutHomeCompanyProductGroup.ML, Amount = 0, AmountConsolidated = 0 }
        };
    #endregion

    private static List<_C4M.LoanApplicationIncome> createIncome(decimal? totalMonthlyIncome)
        => new List<_C4M.LoanApplicationIncome>()
        {
            new() { Category = _C4M.LoanApplicationIncomeCategory.SALARY, Month = 1 },
            new() { Category = _C4M.LoanApplicationIncomeCategory.ENTERPRISE, Month = 12 },
            new() { Category = _C4M.LoanApplicationIncomeCategory.RENT, Month = 1 },
            new() { Category = _C4M.LoanApplicationIncomeCategory.OTHER, Month = 1, Amount = totalMonthlyIncome.GetValueOrDefault() }
        };


    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public SimpleCalculateRequestMapper(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> xxvConnectionProvider)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _xxvConnectionProvider = xxvConnectionProvider;
    }
}
