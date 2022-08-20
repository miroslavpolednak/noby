using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;
using DomainServices.RiskIntegrationService.Api.Clients;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CommitCase;

internal static class CommitCaseRequestExtensions
{
    public static _C4M.CommitRequest ToC4M(this _V2.RiskBusinessCaseCommitCaseRequest request, 
        string chanel,
        List<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> riskApplicationTypes)
        => new _C4M.CommitRequest
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId, chanel),
            ItChannel = FastEnum.Parse<_C4M.CommitRequestItChannel>(chanel, true),
            LoanApplicationProduct = new _C4M.LoanApplicationProduct()
            {
                ProductClusterCode = Helpers.GetRiskApplicationType(riskApplicationTypes, request.ProductTypeId)!.C4mAplCode
            },
            LoanSoldProduct = new _C4M.LoanSoldProduct
            {
                Id = new _C4M.ResourceIdentifier
                {
                    Id = request.SoldProduct.Id,
                    Instance = request.SoldProduct.Company,
                    Domain = Constants.PCP,
                    Resource = Constants.LoanSoldProduct,
                    Variant = chanel //TODO ano/ne?
                }
            },
            ApprovalDate = request.ApprovalDate
        };
}
