using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal static class GetDetailExtensions
{
    public static SalesArrangement.Dto.ParametersMortgage ToApiResponse(this _SA.SalesArrangementParametersMortgage mortgage)
        => new()
        {
            ContractSignatureTypeId = mortgage.ContractSignatureTypeId,
            ExpectedDateOfDrawing = mortgage.ExpectedDateOfDrawing,
            IncomeCurrencyCode = mortgage.IncomeCurrencyCode,
            ResidencyCurrencyCode = mortgage.ResidencyCurrencyCode,
            Agent = mortgage.Agent,
            AgentConsentWithElCom = mortgage.AgentConsentWithElCom,
            LoanRealEstates = mortgage.LoanRealEstates is null ? null : mortgage.LoanRealEstates.Select(x => new SalesArrangement.Dto.LoanRealEstateDto
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId
            }).ToList()
        };

    public static SalesArrangement.Dto.ParametersDrawing ToApiResponse(this _SA.SalesArrangementParametersDrawing model)
        => new()
        {
            DrawingDate = model
        };
}
