namespace DomainServices.SalesArrangementService.Api.Dto;

internal class UpdateSalesArrangementMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.UpdateSalesArrangementRequest Request { get; init; }

    public UpdateSalesArrangementMediatrRequest(Contracts.UpdateSalesArrangementRequest request)
    {
        Request = request;
    }
}
