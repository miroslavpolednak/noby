using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class ProductChildMapper
{
    /// <summary>
    /// Mapovani hlavniho produktu
    /// </summary>
    public async Task<_C4M.LoanApplicationProduct> MapProduct(_V2.LoanApplicationProduct product)
    {
        var purposes = await _codebookService.LoanPurposes(_cancellationToken);
        var collaterals = await _codebookService.CollateralTypes(_cancellationToken);

        return new LoanApplicationProduct
        {
            ProductClusterCode = _riskApplicationType?.C4mAplCode,
            AplType = product.AplType ?? _riskApplicationType?.C4mAplTypeId,
            GlTableSelection = _riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? "OST" : null,
            IsProductSecured = _riskApplicationType?.MandantId == (int)CIS.Foms.Enums.Mandants.Kb ? true : default(bool?),
            LoanApplicationPurpose = product.Purposes?.Select(tranformPurpose(purposes, product))?.ToList(),
            LoanApplicationCollateral = product.Collaterals?.Select(tranformCollateral(collaterals))?.ToList(),
            AmountRequired = product.RequiredAmount.ToAmount(),
            AmountInvestment = product.InvestmentAmount.ToAmount(),
            AmountOwnResources = product.OwnResourcesAmount.ToAmount(),
            AmountForeignResources = product.ForeignResourcesAmount.ToAmount(),
            Maturity = product.LoanDuration,
            Annuity = Convert.ToInt64(product.LoanPaymentAmount ?? 0),
            FixationPeriod = product.FixedRatePeriod,
            InterestRate = product.LoanInterestRate,
            RepaymentScheduleType = Helpers.GetEnumFromString<LoanApplicationProductRepaymentScheduleType>((await _codebookService.RepaymentScheduleTypes(_cancellationToken)).FirstOrDefault(t => t.Id == product.RepaymentScheduleTypeId)?.Code, LoanApplicationProductRepaymentScheduleType.A),
            InstallmentPeriod = Helpers.GetEnumFromString<LoanApplicationProductInstallmentPeriod>(product.InstallmentPeriod, LoanApplicationProductInstallmentPeriod.M),
            InstallmentCount = product.InstallmentCount,
            DrawingPeriodStart = product.DrawingPeriodStart,
            DrawingPeriodEnd = product.DrawingPeriodEnd,
            RepaymentPeriodStart = product.RepaymentPeriodStart,
            RepaymentPeriodEnd = product.RepaymentPeriodEnd,
            HomeCurrencyIncome = product.HomeCurrencyIncome,
            HomeCurrencyResidence = product.HomeCurrencyResidence,
            FinancingType = "01",
            DeveloperCode = product.DeveloperId.ToString(),
            ProjectCode = product.DeveloperProjectId.ToString()
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
                ProductId = getProductId(t),
                ProductType = t.ProductType,
                RelationType = t.RelationType,
                Value = new _C4M.LoanApplicationProductRelationValue
                {
                    Value = t.RemainingExposure.ToString(),
                    Type = "REMAINING_EXPOSURE"
                },
                Counterparty = t.Customers?.Select(x => new _C4M.LoanApplicationProductRelationCounterparty
                {
                    CustomerId = new _C4M.ResourceIdentifier
                    {
                        Id = x.CustomerId,
                        Domain = "CM",
                        Resource = "Customer",
                        Instance = Helpers.GetResourceInstanceFromMandant(_riskApplicationType.MandantId)
                    },
                    RoleCode = Helpers.GetRequiredEnumFromString<_C4M.LoanApplicationProductRelationCounterpartyRoleCode>(customerRoles.FirstOrDefault(c => c.Id == x.CustomerRoleId)?.RdmCode ?? "")
                }).ToList()
            })
            .ToList();

        _C4M.ResourceIdentifier getProductId(_V2.LoanApplicationProductRelation relation)
        {
            if (string.IsNullOrEmpty(relation.BankAccount?.Number))
            {
                return new _C4M.ResourceIdentifier
                {
                    Id = relation.CbcbContractId,
                    Instance = "CBCB",
                    Domain = "EIS",
                    Resource = "CBCBContract"
                };
            }
            else
            {
                return new _C4M.ResourceIdentifier
                {
                    Id = String.IsNullOrEmpty(relation.BankAccount.NumberPrefix) ? relation.BankAccount.Number : $"{relation.BankAccount.NumberPrefix}-{relation.BankAccount.Number}",
                    Instance = relation.BankAccount.BankCode == "7990" ? "MPSS" : "KBCZ",
                    Domain = "PCP",
                    Resource = "LoanSoldProduct"
                };
            }
        }
    }

#pragma warning disable CA1822 // Mark members as static
    public List<_C4M.LoanApplicationDeclaredProductRelation>? MapDeclaredProductRelations(List<_V2.LoanApplicationDeclaredSecuredProduct>? relations)
#pragma warning restore CA1822 // Mark members as static
        => relations?
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
        => t => new LoanApplicationCollateral
        {
            AppraisedValue = t.AppraisedValue.ToAmount(),
            Id = string.IsNullOrEmpty(t.Id) ? null : new _C4M.ResourceIdentifier
            {
                Id = t.Id,
                Domain = "CAM",
                Instance = "KBCZ",
                Resource = "Collateral"
            },
            CategoryCode = collaterals.FirstOrDefault(x => x.CollateralType == t.CollateralType)?.CodeBgm ?? "NE"
        };

    private static Func<_V2.LoanApplicationProductPurpose, LoanApplicationPurpose> tranformPurpose(List<DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes.LoanPurposesItem> purposes, _V2.LoanApplicationProduct product)
        => t => new LoanApplicationPurpose
        {
            Amount = t.Amount,
            Code = (product.ProductTypeId == 20001 && product.LoanKindId == 2001) ? 35 : purposes.FirstOrDefault(x => x.C4mId.HasValue && x.Id == t.LoanPurposeId)?.C4mId ?? -1
        };
    
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;

    public ProductChildMapper(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
