using ExternalServices.Rip.V1.RipWrapper;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal class GetCreditWorthinessHandler
    : IRequestHandler<GetCreditWorthinessRequest, GetCreditWorthinessResponse>
{
    public async Task<GetCreditWorthinessResponse> Handle(GetCreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        CreditWorthinessCalculationArguments ripRequest = new();

        // SA instance
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        // case instance
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));
        // offer instance
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(saInstance.OfferId!.Value, cancellationToken));
        // seznam domacnosti na SA
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_SA.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        if (!households.Any())
            throw new CisValidationException("There is no household bound for this SA");
        if (!households[0].CustomerOnSAId1.HasValue)
            throw new CisValidationException("There is not client bound to primary household");

        ripRequest.ResourceProcessIdMp = offerInstance.ResourceProcessId;
        ripRequest.RiskBusinessCaseIdMp = saInstance.RiskBusinessCaseId;
        ripRequest.ItChannel = "NOBY";
        ripRequest.HumanUser = new HumanUser
        {
            IdentityScheme = "MPAD",
            Identity = _userAccessor.User!.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        // modelace
        ripRequest.LoanApplicationProduct = new LoanApplicationProduct
        {
            Product = caseInstance.Data.ProductTypeId,
            Maturity = offerInstance.Outputs.LoanDuration,
            InterestRate = (double)offerInstance.Outputs.LoanInterestRate,
            AmountRequired = Convert.ToInt32(offerInstance.Outputs.LoanAmount),
            Annuity = Convert.ToInt32(offerInstance.Outputs.LoanPaymentAmount),
            FixationPeriod = offerInstance.Inputs.FixedRatePeriod
        };
        // domacnosti
        ripRequest.Households = new List<LoanApplicationHousehold>();
        foreach (var household in households)
        {
            var h = new LoanApplicationHousehold
            {
                ChildrenUnderAnd10 = household.Data.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.Data.ChildrenOverTenYearsCount,
                Clients = new List<LoanApplicationCounterParty>()
            };
            
            // expenses
            if (household.Expenses is not null)
                h.ExpensesSummary = new ExpensesSummary
                {
                    Rent = household.Expenses.HousingExpenseAmount.GetValueOrDefault(0),
                    Saving = household.Expenses.SavingExpenseAmount.GetValueOrDefault(0),
                    Insurance = household.Expenses.InsuranceExpenseAmount.GetValueOrDefault(0),
                    Other = household.Expenses.OtherExpenseAmount.GetValueOrDefault(0)
                };

            // clients
            if (household.CustomerOnSAId1.HasValue)
            {
                var customer = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSaService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken));
                h.Clients.Add(createClient(customer));
            }

            ripRequest.Households.Add(h);
        }

        var ripResult = ServiceCallResult.ResolveAndThrowIfError<CreditWorthinessCalculation>(await _ripClient.ComputeCreditWorthiness(ripRequest));

        return new GetCreditWorthinessResponse
        {
            InstallmentLimit = Convert.ToInt32(ripResult.InstallmentLimit),
            MaxAmount = Convert.ToInt32(ripResult.MaxAmount),
            RemainsLivingAnnuity = Convert.ToInt32(ripResult.RemainsLivingAnnuity),
            RemainsLivingInst = Convert.ToInt32(ripResult.RemainsLivingInst),
            WorthinessResult = ripResult.WorthinessResult,
            ResultReasonCode = ripResult.ResultReason?.Code,
            ResultReasonDescription = ripResult.ResultReason?.Description
        };
    }

    static LoanApplicationCounterParty createClient(_SA.CustomerOnSA customer)
    {
        var c = new LoanApplicationCounterParty
        {
            IsPartnerMp = customer.HasPartner,
            //MaritalStatusMp = 1 //nacitat z CustomerService???
        };

        // neni tu zadani jake ID posilat, tak beru prvni
        if (customer.CustomerIdentifiers is not null && customer.CustomerIdentifiers.Any())
            c.IdMp = customer.CustomerIdentifiers.First().IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture);

        if (customer.Incomes is not null && customer.Incomes.Any())
            c.LoanApplicationIncome = customer.Incomes.Select(t => new LoanApplicationIncome
            {
                Amount = t.Sum.GetValueOrDefault(),
                CategoryMp = t.IncomeTypeId
            }).ToList();

        if (customer.Obligations is not null && customer.Obligations.Any())
            c.CreditLiabilities = customer.Obligations.Select(t => new CreditLiability
            {
                AmountConsolidated = t.CreditCardLimitConsolidated.GetValueOrDefault(),
                LiabilityType = t.ObligationTypeId.GetValueOrDefault(),
                Installment = t.InstallmentAmount.GetValueOrDefault(),
                InstallmentConsolidated = t.InstallmentAmountConsolidated.GetValueOrDefault(),
                Limit = t.CreditCardLimit.GetValueOrDefault(),
                OutHomeCompanyFlag = t.IsObligationCreditorExternal
            }).ToList();

        return c;
    }

    private readonly ILogger<GetCreditWorthinessHandler> _logger;
    private readonly ExternalServices.Rip.V1.IRipClient _ripClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction _householdService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSaService;

    public GetCreditWorthinessHandler(
        ILogger<GetCreditWorthinessHandler> logger,
        ExternalServices.Rip.V1.IRipClient ripClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction householdService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSaService)
    {
        _ripClient = ripClient;
        _caseService = caseService;
        _userAccessor = userAccessor;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
        _customerOnSaService = customerOnSaService;
        _logger = logger;
    }
}
