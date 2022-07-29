﻿using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Calculate;

internal static class CalculateRequestExtensions
{
    public static _C4M.LoanApplicationProduct ToC4m(this Contracts.CreditWorthiness.CreditWorthinessProduct product, string clusterCode)
        => new()
        {
            ProductClusterCode = clusterCode,
            AmountRequired = product.LoanAmount,
            Annuity = product.LoanPaymentAmount,
            FixationPeriod = product.FixedRatePeriod,
            InterestRate = product.LoanInterestRate,
            Maturity = product.LoanDuration
        };

    public static List<_C4M.ExpensesSummary> ToC4m(this Contracts.CreditWorthiness.CreditWorthinessExpenses expenses)
        => new List<_C4M.ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.SAVING },
            new() { Amount = expenses.Insurance.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault(), Category = _C4M.ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };

    public static List<_C4M.LoanApplicationCounterParty> ToC4m(this List<Contracts.CreditWorthiness.CreditWorthinessCustomer> clients,
        int mandantId,
        List<CodebookService.Contracts.Endpoints.MaritalStatuses.MaritalStatusItem> maritalStatuses,
        List<CodebookService.Contracts.GenericCodebookItemWithCode> mainIncomeTypes)
        => clients.Select(t =>
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
            _C4M.LoanApplicationCounterPartyMaritalStatus maritalStatus = FastEnum.TryParse(maritalStatuses.FirstOrDefault(m => m.Id == t.MaritalStatusStateId)?.RdmMaritalStatusCode, out _C4M.LoanApplicationCounterPartyMaritalStatus ms) ? ms : _C4M.LoanApplicationCounterPartyMaritalStatus.M;

            // Id, IsPartner
            return new _C4M.LoanApplicationCounterParty
            {
                Id = new _C4M.ResourceIdentifier
                {
                    Id = t.IdMp,
                    Instance = (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "MPSS" : "KBCZ",
                },
                IsPartner = t.HasPartner ? 1 : 0,
                MaritalStatus = maritalStatus,
                LoanApplicationIncome = incomes,
            };
        }).ToList();

    #region liabilities
    public static List<_C4M.CreditLiabilitiesSummaryHomeCompany> ToC4mCreditLiabilitiesSummary(this List<Contracts.CreditWorthiness.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && !x.IsObligationCreditorExternal).ToList();
                return new _C4M.CreditLiabilitiesSummaryHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.CreditLiabilitiesSummaryHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();

    public static List<_C4M.CreditLiabilitiesSummary> ToC4mCreditLiabilitiesSummaryOut(this List<Contracts.CreditWorthiness.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && x.IsObligationCreditorExternal).ToList();
                return new _C4M.CreditLiabilitiesSummary
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.CreditLiabilitiesSummaryProductGroup>(t.Code)
                };
            })
            .ToList();

    public static List<_C4M.InstallmentsSummaryHomeCompany> ToC4mInstallmentsSummary(this List<Contracts.CreditWorthiness.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && !x.IsObligationCreditorExternal).ToList();
                return new _C4M.InstallmentsSummaryHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.InstallmentsSummaryHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();

    public static List<_C4M.InstallmentsSummaryOutHomeCompany> ToC4mInstallmentsSummaryOut(this List<Contracts.CreditWorthiness.CreditWorthinessObligation>? liabilites, IEnumerable<CodebookService.Contracts.Endpoints.ObligationTypes.ObligationTypesItem> obligationTypes)
        => obligationTypes
            .Select(t =>
            {
                var coll = liabilites?.Where(x => x.ObligationTypeId == t.Id && x.IsObligationCreditorExternal).ToList();
                return new _C4M.InstallmentsSummaryOutHomeCompany
                {
                    Amount = coll?.Sum(x => x.Amount) ?? 0,
                    AmountConsolidated = coll?.Sum(x => x.AmountConsolidated) ?? 0,
                    ProductGroup = FastEnum.Parse<_C4M.InstallmentsSummaryOutHomeCompanyProductGroup>(t.Code)
                };
            })
            .ToList();
    #endregion liabilities

    #region human user
    public static _C4M.Dealer ToC4mDealer(this Dto.C4mUserInfoData userInfo, Contracts.Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser),
            CompanyId = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
        };

    public static _C4M.Person ToC4mKbPerson(this Dto.C4mUserInfoData userInfo, Contracts.Identity humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("PM", "KBGroupPerson", humanUser),
            Surname = userInfo.PersonSurname,
            OrgUnit = new _C4M.OrganizationUnit
            {
                Id = userInfo.DealerCompanyId.ToString(),
                JobPost = new _C4M.JobPost
                {
                    Id = userInfo.PersonJobPostId
                },
                Name = userInfo.PersonOrgUnitName
            }
        };
    #endregion human user
}
