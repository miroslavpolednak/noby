using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestLoanApplicationProductRelationExtensions
{
    public static async Task<List<_C4M.LoanApplicationProductRelation>> ToC4m(this List<_V2.LoanApplicationProductRelation> relations, _RAT.RiskApplicationTypeItem riskApplicationType, CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, CancellationToken cancellation)
    {
        var customerRoles = (await codebookService.CustomerRoles(cancellation));

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
                            Instance = Helpers.GetResourceInstanceFromMandant(riskApplicationType.MandantId)
                        },
                        RoleCode = role
                    };
                }).ToList()
            })
            .ToList();
    }
}
