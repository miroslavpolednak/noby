namespace DomainServices.CaseService.Api.Dto.SalesArrangement;

internal sealed class UpdateSalesArrangementStateMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }
    public int State { get; init; }

    public UpdateSalesArrangementStateMediatrRequest(Contracts.UpdateSalesArrangementStateRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
        State = request.State;
    }
}
