using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestLoanApplicationDeclaredProductRelationExtensions
{
    public static List<_C4M.LoanApplicationDeclaredProductRelation> ToC4m(this List<_V2.LoanApplicationDeclaredSecuredProduct> relations)
    {
        return relations
            .Select(t => new _C4M.LoanApplicationDeclaredProductRelation
            {
                ProductType = "KBGROUP",
                AplType = t.AplType,
                ProductClusterCode = t.ProductClusterCode,
                RelationType = "SECURED_PRODUCT",
                Value = new _C4M.LoanApplicationProductRelationValue { Value = t.RemainingExposure?.ToString(), Type = "REMAINING_EXPOSURE" }
                })
            .ToList();
    }
}
