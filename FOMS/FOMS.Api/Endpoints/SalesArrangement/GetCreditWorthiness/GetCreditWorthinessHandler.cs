using ExternalServices.Rip.V1.RipWrapper;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using CIS.Core;
using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal class GetCreditWorthinessHandler
    : IRequestHandler<GetCreditWorthinessRequest, GetCreditWorthinessResponse>
{
    public async Task<GetCreditWorthinessResponse> Handle(GetCreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        CreditWorthinessCalculationArguments ripRequest = new();

        // SA instance
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, $"Offer instance not found for SA {saInstance.SalesArrangementId}");

        // case instance
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));
        // offer instance
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));
        // user instance
        var userInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellationToken));
        // seznam domacnosti na SA
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_SA.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        if (!households.Any())
            throw new CisValidationException("There is no household bound for this SA");
        
        ripRequest.ResourceProcessIdMp = offerInstance.ResourceProcessId;
        ripRequest.RiskBusinessCaseIdMp = saInstance.RiskBusinessCaseId;
        ripRequest.ItChannel = "NOBY";
#pragma warning disable CA1305 // Specify IFormatProvider
        ripRequest.HumanUser = new HumanUser
        {
            IdentityScheme = ((CIS.Foms.Enums.UserIdentitySchemes)Convert.ToInt32(userInstance.UserIdentifiers[0].IdentityScheme)).GetAttribute<DisplayAttribute>()!.Name,
            Identity = userInstance.UserIdentifiers[0].Identity
        };

        // modelace
        ripRequest.LoanApplicationProduct = new LoanApplicationProduct
        {
            Product = caseInstance.Data.ProductTypeId,
            Maturity = offerInstance.SimulationResults.LoanDuration,
            InterestRate = (double)(decimal)offerInstance.SimulationResults.LoanInterestRate,
            AmountRequired = Convert.ToInt32((decimal?)offerInstance.SimulationResults.LoanAmount ?? 0),
            Annuity = Convert.ToInt32((decimal?)offerInstance.SimulationResults.LoanPaymentAmount ?? 0),
            FixationPeriod = offerInstance.SimulationInputs.FixedRatePeriod!.Value
        };
#pragma warning restore CA1305
        
        // domacnosti
        ripRequest.Households = new List<LoanApplicationHousehold>();
        foreach (var household in households.Where(t => t.CustomerOnSAId1.HasValue)) //neberu domacnosti bez aspon jednoho customera
            ripRequest.Households.Add(await createHousehold(household, cancellationToken));

        var ripResult = ServiceCallResult.ResolveAndThrowIfError<CreditWorthinessCalculation>(await _ripClient.ComputeCreditWorthiness(ripRequest));

        return new GetCreditWorthinessResponse
        {
            InstallmentLimit = Convert.ToInt32(ripResult.InstallmentLimit ?? 0),
            MaxAmount = Convert.ToInt32(ripResult.MaxAmount ?? 0),
            RemainsLivingAnnuity = Convert.ToInt32((decimal?)ripResult.RemainsLivingAnnuity ?? 0),
            RemainsLivingInst = Convert.ToInt32((decimal?)ripResult.RemainsLivingInst ?? 0),
            WorthinessResult = ripResult.WorthinessResult,
            ResultReasonCode = ripResult.ResultReason?.Code,
            ResultReasonDescription = ripResult.ResultReason?.Description,
            LoanAmount = offerInstance.SimulationInputs.LoanAmount,
            LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount
        };
    }

    private async Task<LoanApplicationHousehold> createHousehold(_SA.Household household, CancellationToken cancellationToken)
    {
        var h = new LoanApplicationHousehold
        {
            ChildrenUnderAnd10 = household.Data.ChildrenUpToTenYearsCount.GetValueOrDefault(),
            ChildrenOver10 = household.Data.ChildrenOverTenYearsCount.GetValueOrDefault(),
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
            h.Clients.Add(await createClient(household.CustomerOnSAId1.Value, cancellationToken));
        if (household.CustomerOnSAId2.HasValue)
            h.Clients.Add(await createClient(household.CustomerOnSAId2.Value, cancellationToken));

        // Upravit validaci na FE API tak, aby hlídala, že aspoň jeden žadatel v každé z domácností na SA má vyplněný aspoň jeden příjem (=tedy nevalidovat, že každý žadatel musí mít vyplněný příjem)
        if (!h.Clients.Any(t => t.LoanApplicationIncome.Any()))
            throw new CisValidationException("At least one customer in household must have some income");

        return h;
    }

    private async Task<LoanApplicationCounterParty> createClient(int customerOnSAId, CancellationToken cancellationToken)
    {
        // customer on SA instance
        var customer = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSaService.GetCustomer(customerOnSAId, cancellationToken));

        var c = new LoanApplicationCounterParty
        {
            // TODO:
            // IsPartnerMp = customer.HasPartner
            MaritalStatusMp = customer.MaritalStatusId
        };

        //TODO neni tu zadani jake ID posilat, tak beru prvni
        if (customer.CustomerIdentifiers is not null && customer.CustomerIdentifiers.Any())
            c.IdMp = customer.CustomerIdentifiers.First().IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture);

        if (customer.Incomes is not null && customer.Incomes.Any())
            c.LoanApplicationIncome = customer.Incomes.Select(t => new LoanApplicationIncome
            {
                Amount = Convert.ToDouble((decimal)t.Sum),
                CategoryMp = t.IncomeTypeId
            }).ToList() ?? new List<LoanApplicationIncome>();

        if (customer.Obligations is not null && customer.Obligations.Any())
            c.CreditLiabilities = customer.Obligations.Select(t => new CreditLiability
            {
                LiabilityType = t.ObligationTypeId.GetValueOrDefault(),
                Installment = t.InstallmentAmount,
                Limit = t.CreditCardLimit,
                InstallmentConsolidated = t.Correction?.InstallmentAmountCorrection,//asi?
                AmountConsolidated = t.Correction?.CreditCardLimitCorrection,//asi?
                OutHomeCompanyFlag = t.Creditor?.IsExternal.GetValueOrDefault() ?? false
            }).ToList();

        return c;
    }

    private readonly ExternalServices.Rip.V1.IRipClient _ripClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction _householdService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSaService;

    public GetCreditWorthinessHandler(
        ExternalServices.Rip.V1.IRipClient ripClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.UserService.Abstraction.IUserServiceAbstraction userService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction householdService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSaService)
    {
        _ripClient = ripClient;
        _userService = userService;
        _caseService = caseService;
        _userAccessor = userAccessor;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
        _customerOnSaService = customerOnSaService;
    }
}
