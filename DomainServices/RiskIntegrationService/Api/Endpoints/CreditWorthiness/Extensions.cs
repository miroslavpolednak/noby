
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness;

internal static class Extensions
{
    public static _C4M.ResourceIdentifier ToC4m(this Contracts.CreditWorthiness.CalculateRequest request)
        => _C4M.ResourceIdentifier.Create("MPSS", "OM", "OfferInstance", request.ResourceProcessIdMp);

    public static _C4M.LoanApplicationProduct ToC4m(this Contracts.CreditWorthiness.LoanApplicationProduct product, string clusterCode)
        => new()
        {
            ProductClusterCode = clusterCode,
            AmountRequired = product.AmountRequired,
            Annuity = product.Annuity,
            FixationPeriod = product.FixationPeriod,
            InterestRate = product.InterestRate,
            Maturity = product.Maturity
        };

    public static List<_C4M.ExpensesSummary> ToC4m(this Contracts.CreditWorthiness.ExpensesSummary expenses)
        => new List<_C4M.ExpensesSummary>()
        {
            new() { Amount = expenses.Rent, Category = _C4M.ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving, Category = _C4M.ExpensesSummaryCategory.SAVING },
            new() { Amount = expenses.Insurance, Category = _C4M.ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other, Category = _C4M.ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };

    public static List<_C4M.LoanApplicationCounterParty> ToC4m(this List<Contracts.CreditWorthiness.LoanApplicationCounterParty> clients, 
        int mandantId, 
        List<CodebookService.Contracts.Endpoints.MaritalStatuses.MaritalStatusItem> maritalStatuses)
        => clients.Select(t => {
            var result = new _C4M.LoanApplicationCounterParty
            {
                Id = new _C4M.ResourceIdentifier
                {
                    Id = t.IdMp,
                    Instance = (CIS.Foms.Enums.Mandants)mandantId == CIS.Foms.Enums.Mandants.Mp ? "MPSS" : "KBCZ",
                },
                IsPartner = t.IsPartnerMp ? 1 : 0,
                LoanApplicationIncome = (t.LoanApplicationIncome ?? new(0)).Select(i => new _C4M.LoanApplicationIncome
                {
                    Amount = i.Amount
                }).ToList()
            };

            if (Enum.TryParse<_C4M.LoanApplicationCounterPartyMaritalStatus>(maritalStatuses.FirstOrDefault(m => m.Id == t.MaritalStatusMp)?.RdmMaritalStatusCode, out _C4M.LoanApplicationCounterPartyMaritalStatus ms))
                result.MaritalStatus = ms;

            if (t.LoanApplicationIncome is not null && t.LoanApplicationIncome.Any())
                result.LoanApplicationIncome = t.LoanApplicationIncome.Select(i => new _C4M.LoanApplicationIncome
                {
                    Amount = i.Amount,
                }).ToList();

            return result;
        }).ToList();

    #region human user
    public static _C4M.Dealer ToC4mDealer(this Dto.C4mUserInfoData userInfo, Contracts.HumanUser humanUser)
        => new()
        {
            Id = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser),
            CompanyId = _C4M.ResourceIdentifier.Create("BM", "Broker", humanUser, userInfo.DealerCompanyId?.ToString())
        };

    public static _C4M.Person ToC4mKbPerson(this Dto.C4mUserInfoData userInfo, Contracts.HumanUser humanUser)
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

    #region response
    public static Contracts.CreditWorthiness.CalculateResponse ToServiceResponse(this _C4M.CreditWorthinessCalculation response)
        => new()
        {
            InstallmentLimit = response.InstallmentLimit,
            MaxAmount = response.MaxAmount,
            RemainsLivingAnnuity = response.RemainsLivingAnnuity,
            RemainsLivingInst = response.RemainsLivingInst,
            ResultReason = response.ResultReason is null ? null : new Contracts.CreditWorthiness.ResultReason
            {
                Code = response.ResultReason.Code,
                Description = response.ResultReason.Description
            },
            WorthinessResult = response.InstallmentLimit > response.RemainsLivingAnnuity ? 1 : 0
        };
    #endregion response
}
