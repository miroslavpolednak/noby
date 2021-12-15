namespace DomainServices.SalesArrangementService.Api.Dto;

internal class UpdateSalesArrangementDataMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }
    public Google.Protobuf.WellKnownTypes.Any SalesArrangement { get; init; }

    public UpdateSalesArrangementDataMediatrRequest(Contracts.UpdateSalesArrangementDataRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
        SalesArrangement = request.SalesArrangement;
    }
}
