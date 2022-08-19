using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class ProductChildMapper
{
    /// <summary>
    /// Mapovani hlavniho produktu
    /// </summary>
    public async Task<_C4M.LoanApplicationProduct> MapProduct(_V2.LoanApplicationProduct product)
    {
        // repayment type
        var repaymentType = (await _codebookService.RepaymentScheduleTypes(_cancellationToken)).FirstOrDefault(t => t.Id == product.RepaymentScheduleTypeId)?.Code ?? "A";
        if (!FastEnum.TryParse(repaymentType, out LoanApplicationProductRepaymentScheduleType repaymentTypeEnum))
            throw new CisValidationException(0, $"Can't cast RepaymentScheduleTypeId '{product.RepaymentScheduleTypeId}' to C4M enum");

        // installment period
        LoanApplicationProductInstallmentPeriod installmentPeriod;
        if (string.IsNullOrEmpty(product.InstallmentPeriod))
            installmentPeriod = LoanApplicationProductInstallmentPeriod.M;
        else
            if (!FastEnum.TryParse(product.InstallmentPeriod, out installmentPeriod))
            throw new CisValidationException(0, $"Can't cast LoanApplicationProductInstallmentPeriod '{product.InstallmentPeriod}' to C4M enum");

        var purposes = await _codebookService.LoanPurposes(_cancellationToken);
        var collaterals = await _codebookService.CollateralTypes(_cancellationToken);

        return new LoanApplicationProduct
        {
            ProductClusterCode = _riskApplicationType?.C4mAplCode,
            AplType = product.AplType ?? _riskApplicationType?.C4mAplTypeId,
            GlTableSelection = _riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? "OST" : null,
            IsProductSecured = _riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? true : default(bool?),
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
            DeveloperCode = await getDeveloper(product.DeveloperId),
            ProjectCode = await getDeveloperProject(product.DeveloperProjectId)
        };
    }

    /// <summary>
    /// Mapovani produktovych vazeb
    /// </summary>
    public async Task<List<_C4M.LoanApplicationProductRelation>?> MapProductRelations(List<_V2.LoanApplicationProductRelation>? relations)
    {
        if (relations is null) return null;

        var customerRoles = (await _codebookService.CustomerRoles(_cancellationToken));

        return relations
            .Select(t => new _C4M.LoanApplicationProductRelation
            {
                ProductId = new _C4M.ResourceIdentifier(),
                ProductType = t.ProductType,
                RelationType = t.RelationType,
                Value = new _C4M.LoanApplicationProductRelationValue
                {
                    Value = t.RemainingExposure.ToString(),
                    Type = "REMAINING_EXPOSURE"
                },
                LoanApplicationProductRelationCounterparty = t.Customers?.Select(x =>
                {
                    if (!FastEnum.TryParse(customerRoles.FirstOrDefault(c => c.Id == x.CustomerRoleId)?.RdmCode, out _C4M.LoanApplicationProductRelationCounterpartyRoleCode role))
                        throw new CisValidationException(0, $"Can't cast LoanApplicationProductRelationCounterpartyRoleCode '{x.CustomerRoleId}' to C4M enum");

                    return new _C4M.LoanApplicationProductRelationCounterparty
                    {
                        CustomerId = new _C4M.ResourceIdentifier
                        {
                            Id = x.CustomerId,
                            Domain = "CM",
                            Resource = "Customer",
                            Instance = Helpers.GetResourceInstanceFromMandant(_riskApplicationType.MandantId)
                        },
                        RoleCode = role
                    };
                }).ToList()
            })
            .ToList();
    }

#pragma warning disable CA1822 // Mark members as static
    public List<_C4M.LoanApplicationDeclaredProductRelation> MapDeclaredProductRelations(List<_V2.LoanApplicationDeclaredSecuredProduct> relations)
#pragma warning restore CA1822 // Mark members as static
        => relations
            .Select(t => new _C4M.LoanApplicationDeclaredProductRelation
            {
                ProductType = "KBGROUP",
                AplType = t.AplType,
                ProductClusterCode = t.ProductClusterCode,
                RelationType = "SECURED_PRODUCT",
                Value = new _C4M.LoanApplicationProductRelationValue { Value = t.RemainingExposure?.ToString(System.Globalization.CultureInfo.InvariantCulture), Type = "REMAINING_EXPOSURE" }
            })
            .ToList();

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

    private async Task<string?> getDeveloper(int? developerId)
        => developerId.HasValue ? (await _codebookService.Developers(_cancellationToken)).FirstOrDefault(t => t.Id == developerId)?.Cin : null;

    private async Task<string?> getDeveloperProject(int? developerProjectId)
        => developerProjectId.HasValue ? (await _codebookService.DeveloperProjects(_cancellationToken)).FirstOrDefault(t => t.Id == developerProjectId)?.Name : null;

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;

    public ProductChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
