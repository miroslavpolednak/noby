namespace DomainServices.SalesArrangementService.Api.Dto;

internal class UpdateSalesArrangementDataMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.UpdateSalesArrangementDataRequest Request { get; init; }

    public UpdateSalesArrangementDataMediatrRequest(Contracts.UpdateSalesArrangementDataRequest request)
    {
        Request = request;
    }
}
