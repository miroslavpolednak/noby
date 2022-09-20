﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate.Mappers;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class CustomersChildMapper
{
    public async Task<List<_C4M.LoanApplicationCounterParty>> MapCustomers(List<_V2.CreditWorthinessCustomer> customers, int mandantId, CancellationToken cancellation)
    {
        var maritalStatuses = await _codebookService.MaritalStatuses(cancellation);
        var mainIncomeTypes = await _codebookService.IncomeMainTypes(cancellation);

        return customers.Select(t =>
        {
            // income
            List<_C4M.LoanApplicationIncome> incomes = new() // vychozi nastaveni prijmu
            {
                new() { Category = _C4M.LoanApplicationIncomeCategory.SALARY, Month = 1 },
                new() { Category = _C4M.LoanApplicationIncomeCategory.ENTERPRISE, Month = 12 },
                new() { Category = _C4M.LoanApplicationIncomeCategory.RENT, Month = 1 },
                new() { Category = _C4M.LoanApplicationIncomeCategory.OTHER, Month = 1 }
            };
            t.Incomes?.ForEach(i =>
            {
                string incomeCode = mainIncomeTypes.FirstOrDefault(t => t.Id == i.IncomeTypeId)?.Code ?? throw new CisValidationException(0, $"IncomeType={i.IncomeTypeId} not found in IncomeMainTypes codebook");
                incomes.First(t => t.Category == FastEnum.Parse<_C4M.LoanApplicationIncomeCategory>(incomeCode)).Amount += i.Amount;
            });

            // merital status
            _C4M.LoanApplicationCounterPartyMaritalStatus maritalStatus = FastEnum.TryParse(maritalStatuses.FirstOrDefault(m => m.Id == t.MaritalStateId)?.RdmMaritalStatusCode, out _C4M.LoanApplicationCounterPartyMaritalStatus ms) ? ms : _C4M.LoanApplicationCounterPartyMaritalStatus.M;

            // Id, IsPartner
            return new _C4M.LoanApplicationCounterParty
            {
                Id = string.IsNullOrEmpty(t.PrimaryCustomerId) ? null : new _C4M.ResourceIdentifier
                {
                    Id = t.PrimaryCustomerId,
                    Instance = (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "MPSS" : "KBCZ",
                },
                IsPartner = t.HasPartner ? 1 : 0,
                MaritalStatus = maritalStatus,
                LoanApplicationIncome = incomes,
            };
        }).ToList();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CustomersChildMapper(CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
    }
}
