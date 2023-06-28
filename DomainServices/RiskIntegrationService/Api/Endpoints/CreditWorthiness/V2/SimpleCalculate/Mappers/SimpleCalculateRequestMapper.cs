using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate.Mappers;

[Obsolete("C4Mv3", true)]
[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class SimpleCalculateRequestMapper
{
    public async Task<_C4M.CreditWorthinessCalculationArguments> MapToC4m(
        _V2.CreditWorthinessSimpleCalculateRequest request,
        RiskApplicationTypesResponse.Types.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellation)
    {
        var requestModel = new _C4M.CreditWorthinessCalculationArguments
        {
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcessId(request.ResourceProcessId!).ToC4M(),
            ItChannel = FastEnum.Parse<_C4M.ItChannelType>(_configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            LoanApplicationProduct = new()
            {
                ProductClusterCode = riskApplicationType.C4MAplCode,
                AmountRequired = request.Product!.LoanAmount.ToAmount(),
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
            if (userInstance != null)
            {
                if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                    requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
                else
                    requestModel.Person = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
            }            
        }

        // client
        var counterParty = new _C4M.LoanApplicationCounterParty
        {
            Id = string.IsNullOrEmpty(request.PrimaryCustomerId) ? null : _C4M.ResourceIdentifier.CreateResourceCounterParty(request.PrimaryCustomerId, !riskApplicationType.MandantId.HasValue || (CIS.Foms.Enums.Mandants)riskApplicationType.MandantId == CIS.Foms.Enums.Mandants.Kb ? "KBCZ" : "MPSS").ToC4M(),
            IsPartner = false,
            MaritalStatus = _C4M.MartialStatusType.M,
            Income = createIncome(request.TotalMonthlyIncome)
        };

        // household
        var household = new _C4M.LoanApplicationHousehold
        {
            ChildrenOver10 = request.ChildrenCount,
            ExpensesSummary = (request.ExpensesSummary ?? new _V2.CreditWorthinessSimpleExpensesSummary()).ToC4M(),
            CreditLiabilitiesSummaryHomeCompany = createCreditLiabilitiesSummary(),
            CreditLiabilitiesSummaryOutHomeCompany = createCreditLiabilitiesSummaryOut(request.ObligationsSummary?.AuthorizedOverdraftsAmount, request.ObligationsSummary?.CreditCardsAmount),
            InstallmentsSummaryHomeCompany = createInstallmentsSummary(),
            InstallmentsSummaryOutHomeCompany = createInstallmentsSummaryOut(request.ObligationsSummary?.LoansInstallmentsAmount),
            Clients = new List<_C4M.LoanApplicationCounterParty> { counterParty }
        };

        requestModel.LoanApplicationHousehold = new List<_C4M.LoanApplicationHousehold>() { household };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (userInstance != null)
            {
                if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                    requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
                else
                    requestModel.Person = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
            }            
        }

        return requestModel;
    }

    #region liabilities installments
    private static List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummary()
        => new()
        {
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryType.AD, Amount = 0.ToAmount(), AmountConsolidated = 0.ToAmount() },
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryType.CC, Amount = 0.ToAmount(), AmountConsolidated = 0.ToAmount() }
        };

    private static List<_C4M.CreditLiabilitiesSummary> createCreditLiabilitiesSummaryOut(decimal? authorizedOverdraftsTotalAmount, decimal? creditCardsTotalAmount)
        => new()
        {
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryType.AD, Amount = authorizedOverdraftsTotalAmount.GetValueOrDefault().ToAmount(), AmountConsolidated = 0.ToAmount() },
            new() { ProductGroup = _C4M.CreditLiabilitiesSummaryType.CC, Amount = creditCardsTotalAmount.GetValueOrDefault().ToAmount(), AmountConsolidated = 0.ToAmount() }
        };

    private static List<_C4M.LoanInstallmentsSummary> createInstallmentsSummary()
        => new()
        {
            new() { ProductGroup = _C4M.InstallmentsSummaryType.CL, Amount = 0.ToAmount(), AmountConsolidated = 0.ToAmount() },
            new() { ProductGroup = _C4M.InstallmentsSummaryType.ML, Amount = 0.ToAmount(), AmountConsolidated = 0.ToAmount() }
        };

    private static List<_C4M.LoanInstallmentsSummary> createInstallmentsSummaryOut(decimal? loansTotalInstallments)
        => new()
        {
            new() { ProductGroup = _C4M.InstallmentsSummaryType.CL, Amount = loansTotalInstallments.GetValueOrDefault().ToAmount(), AmountConsolidated = 0.ToAmount() },
            new() { ProductGroup = _C4M.InstallmentsSummaryType.ML, Amount = 0.ToAmount(), AmountConsolidated = 0.ToAmount() }
        };
    #endregion

    private static List<_C4M.LoanApplicationIncome> createIncome(decimal? totalMonthlyIncome)
        => new List<_C4M.LoanApplicationIncome>()
        {
            new() { Category = _C4M.LoanApplicationIncomeType.SALARY, Months = 1 },
            new() { Category = _C4M.LoanApplicationIncomeType.ENTERPRISE, Months = 12 },
            new() { Category = _C4M.LoanApplicationIncomeType.RENT, Months = 1 },
            new() { Category = _C4M.LoanApplicationIncomeType.OTHER, Months = 1, Amount = totalMonthlyIncome.GetValueOrDefault().ToAmount() }
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
