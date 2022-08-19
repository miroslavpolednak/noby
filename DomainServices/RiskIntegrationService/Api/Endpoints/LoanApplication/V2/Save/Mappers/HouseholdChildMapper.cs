using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using _CB = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdChildMapper
{
    public async Task<List<_C4M.LoanApplicationHousehold>> MapHouseholds(List<_V2.LoanApplicationHousehold> households)
    {
        var householdTypes = await _codebookService.HouseholdTypes(_cancellationToken);
        var propSettlements = await _codebookService.PropertySettlements(_cancellationToken);

        List<_C4M.LoanApplicationHousehold> list = new List<LoanApplicationHousehold>();
        foreach (var household in households)
        {
            list.Add(new _C4M.LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = householdTypes.FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                HouseholdExpensesSummary = mapExpenses(household.Expenses),
                SettlementTypeCode = propSettlements.FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                CounterParty = await _customerMapper.MapCustomers(household.Customers!),
                HouseholdCreditLiabilitiesSummaryOutHomeCompany = null,
                HouseholdInstallmentsSummaryOutHomeCompany = null
            });
        }
        
        return list;
    }

    /// <summary>
    /// Namapovat vydaje
    /// </summary>
    static List<_C4M.ExpensesSummary> mapExpenses(Contracts.Shared.V1.ExpensesSummary? expenses)
        => new()
        {
            new() { Amount = (expenses?.Rent ?? 0M).ToAmount(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = (expenses?.Saving ?? 0).ToAmount(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = (expenses?.Insurance ?? 0).ToAmount(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = (expenses?.Other ?? 0).ToAmount(), Category = ExpensesSummaryCategory.OTHER }
        };

    private readonly HouseholdCustomerChildMapper _customerMapper;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;

    public HouseholdChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
        _customerMapper = new HouseholdCustomerChildMapper(codebookService, riskApplicationType, cancellationToken);
    }
}
