using CIS.Foms.Enums;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Services;

internal interface ISalesArrangementDataService
{
    Task<Dto.MortgageDetailDto> GetData(
        long caseId,
        int? offerId,
        SalesArrangementStates salesArrangementState,
        CancellationToken cancellationToken);
}