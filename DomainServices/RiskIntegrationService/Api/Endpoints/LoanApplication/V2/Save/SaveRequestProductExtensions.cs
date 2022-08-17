using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestProductExtensions
{
    public static async Task<LoanApplicationProduct> ToC4m(this _V2.LoanApplicationProduct product, _RAT.RiskApplicationTypeItem riskApplicationType, CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, CancellationToken cancellation)
    {
        // repayment type
        var repaymentType = (await codebookService.RepaymentScheduleTypes(cancellation)).FirstOrDefault(t => t.Id == product.RepaymentScheduleTypeId)?.Code ?? "A";
        if (!FastEnum.TryParse(repaymentType, out LoanApplicationProductRepaymentScheduleType repaymentTypeEnum))
            throw new CisValidationException(0, $"Can't cast RepaymentScheduleTypeId '{product.RepaymentScheduleTypeId}' to C4M enum");
        
        // installment period
        LoanApplicationProductInstallmentPeriod installmentPeriod;
        if (string.IsNullOrEmpty(product.InstallmentPeriod))
            installmentPeriod = LoanApplicationProductInstallmentPeriod.M;
        else
            if (!FastEnum.TryParse(product.InstallmentPeriod, out installmentPeriod))
                throw new CisValidationException(0, $"Can't cast LoanApplicationProductInstallmentPeriod '{product.InstallmentPeriod}' to C4M enum");

        var purposes = await codebookService.LoanPurposes(cancellation);
        var collaterals = await codebookService.CollateralTypes(cancellation);

        return new LoanApplicationProduct
        {
            ProductClusterCode = riskApplicationType?.C4mAplCode,
            AplType = product.AplType ?? riskApplicationType?.C4mAplTypeId,
            GlTableSelection = riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? "OST" : null,
            IsProductSecured = riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? true : default(bool?),
            LoanApplicationPurpose = product.Purposes?.Select(tranformPurpose(purposes))?.ToList(),
            LoanApplicationCollateral = product.Collaterals?.Select(tranformCollateral(collaterals))?.ToList(),
            AmountRequired = product.RequiredAmount.ToAmount(),
            AmountInvestment = product.InvestmentAmount.ToAmount(),
            AmountOwnResources = product.OwnResourcesAmount.ToAmount(),
            AmountForeignResources = product.ForeignResourcesAmount.ToAmount(),
            Maturity = product.LoanDuration,
            Annuity = Convert.ToInt64(product.LoanPaymentAmount ?? 0),
            FixationPeriod = product.FixedRatePeriod,
            InterestRate = product.LoanInterestRate,
            RepaymentScheduleType = FastEnum.Parse<LoanApplicationProductRepaymentScheduleType>(repaymentType),
            InstallmentPeriod = installmentPeriod,
            InstallmentCount = product.InstallmentCount,
            DrawingPeriodStart = product.DrawingPeriodStart,
            DrawingPeriodEnd = product.DrawingPeriodEnd,
            RepaymentPeriodStart = product.RepaymentPeriodStart,
            RepaymentPeriodEnd = product.RepaymentPeriodEnd,
            HomeCurrencyIncome = product.HomeCurrencyIncome,
            HomeCurrencyResidence = product.HomeCurrencyResidence,
            FinancingType = product.FinancingTypes?.FirstOrDefault(),
            DeveloperCode = await getDeveloper(product.DeveloperId, codebookService, cancellation),
            ProjectCode = await getDeveloperProject(product.DeveloperProjectId, codebookService, cancellation)
        };
    }

    public static async Task<List<LoanApplicationProductRelation>> ToC4m(this List<_V2.LoanApplicationProductRelation> relations, _RAT.RiskApplicationTypeItem riskApplicationType, CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, CancellationToken cancellation)
    {
        var customerRoles = (await codebookService.CustomerRoles(cancellation));

        return relations
            .Select(t => new LoanApplicationProductRelation
            {
                ProductId = new ResourceIdentifier(),
                ProductType = t.ProductType,
                RelationType = t.RelationType,
                Value = new LoanApplicationProductRelationValue
                {
                    Value = t.RemainingExposure.ToString(),
                    Type = "REMAINING_EXPOSURE"
                },
                LoanApplicationProductRelationCounterparty = t.Customers?.Select(x =>
                {
                    if (!FastEnum.TryParse(customerRoles.FirstOrDefault(c => c.Id == x.CustomerRoleId)?.RdmCode, out LoanApplicationProductRelationCounterpartyRoleCode role))
                        throw new CisValidationException(0, $"Can't cast LoanApplicationProductRelationCounterpartyRoleCode '{x.CustomerRoleId}' to C4M enum");

                    return new LoanApplicationProductRelationCounterparty
                    {
                        CustomerId = new ResourceIdentifier
                        {
                            Id = x.CustomerId,
                            Domain = "CM",
                            Resource = "Customer",
                            Instance = riskApplicationType.MandantId == (int)CIS.Foms.Enums.Mandants.Mp ? "MPSS" : "KBCZ"                        },
                        RoleCode = role
                    };
                }).ToList()
            })
            .ToList();
    }

    private static Func<_V2.LoanApplicationProductCollateral, LoanApplicationCollateral> tranformCollateral(List<DomainServices.CodebookService.Contracts.Endpoints.CollateralTypes.CollateralTypeItem> collaterals)
    {
        return t => new LoanApplicationCollateral
        {
            AppraisedValue = t.AppraisedValue.ToAmount(),
            Id = string.IsNullOrEmpty(t.Id) ? null : new ResourceIdentifier
            {
                Id = t.Id,
                Domain = "CAM",
                Instance = "MPSS",
                Resource = "Collateral"
            },
            CategoryCode = collaterals.FirstOrDefault(x => x.CollateralType == t.CollateralType)?.CodeBgm ?? "NE"
        };
    }

    private static Func<_V2.LoanApplicationProductPurpose, LoanApplicationPurpose> tranformPurpose(List<DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes.LoanPurposesItem> purposes)
    {
        return t =>
        {
#pragma warning disable CA1305 // Specify IFormatProvider
            if (!FastEnum.TryParse((purposes.FirstOrDefault(x => x.Id == t.LoanPurposeId)?.C4mId ?? -1).ToString(), out LoanApplicationPurposeCode code))
                throw new CisValidationException(0, $"Can't cast LoanApplicationPurposeCode '{t.LoanPurposeId}' to C4M enum");
#pragma warning restore CA1305 // Specify IFormatProvider

            return new LoanApplicationPurpose
            {
                Amount = t.Amount,
                Code = code
            };
        };
    }

    private static async Task<string?> getDeveloper(int? developerId, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
        => developerId.HasValue ? (await _codebookService.Developers(cancellation)).FirstOrDefault(t => t.Id == developerId)?.Cin : null;

    private static async Task<string?> getDeveloperProject(int? developerProjectId, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
        => developerProjectId.HasValue ? (await _codebookService.DeveloperProjects(cancellation)).FirstOrDefault(t => t.Id == developerProjectId)?.Name : null;
}
