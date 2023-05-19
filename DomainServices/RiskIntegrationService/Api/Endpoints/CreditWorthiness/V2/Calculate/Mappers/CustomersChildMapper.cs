using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class CustomersChildMapper
{
    public async Task<List<_C4M.LoanApplicationCounterParty>> MapCustomers(List<_V2.CreditWorthinessCustomer> customers, int? mandantId, CancellationToken cancellation)
    {
        var maritalStatuses = await _codebookService.MaritalStatuses(cancellation);
        var mainIncomeTypes = await _codebookService.IncomeMainTypes(cancellation);

        return customers.Select(t =>
        {
            // income
            List<_C4M.LoanApplicationIncome> incomes = new() // vychozi nastaveni prijmu
            {
                new() { Category = _C4M.LoanApplicationIncomeType.SALARY, Months = 1 },
                new() { Category = _C4M.LoanApplicationIncomeType.ENTERPRISE, Months = 12 },
                new() { Category = _C4M.LoanApplicationIncomeType.RENT, Months = 1 },
                new() { Category = _C4M.LoanApplicationIncomeType.OTHER, Months = 1 }
            };
            t.Incomes?.ForEach(i =>
            {
                string incomeCode = mainIncomeTypes.FirstOrDefault(t => t.Id == i.IncomeTypeId)?.Code ?? throw new CisValidationException(17007, $"IncomeType={i.IncomeTypeId} not found in IncomeMainTypes codebook");
                var income = incomes.First(t => t.Category == FastEnum.Parse<_C4M.LoanApplicationIncomeType>(incomeCode));
                if (income.Amount == null)
                    income.Amount = 0.ToAmount();
                income.Amount!.Value += i.Amount;
            });

            // marital status
            _C4M.MartialStatusType maritalStatus = FastEnum.TryParse(maritalStatuses.FirstOrDefault(m => m.Id == t.MaritalStateId)?.RdmMaritalStatusCode, out _C4M.MartialStatusType ms) ? ms : _C4M.MartialStatusType.M;

            // Id, IsPartner
            return new _C4M.LoanApplicationCounterParty
            {
                Id = string.IsNullOrEmpty(t.PrimaryCustomerId) ? null : _C4M.ResourceIdentifier.CreateResourceCounterParty(t.PrimaryCustomerId, !mandantId.HasValue || (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Kb ? "KBCZ" : "MPSS").ToC4M(),
                IsPartner = t.HasPartner,
                MaritalStatus = maritalStatus,
                Income = incomes,
            };
        }).ToList();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;

    public CustomersChildMapper(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
    }
}
