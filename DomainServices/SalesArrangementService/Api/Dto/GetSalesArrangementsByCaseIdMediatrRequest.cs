namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementsByCaseIdMediatrRequest
    : IRequest<Contracts.GetSalesArrangementsByCaseIdResponse>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.GetSalesArrangementsByCaseIdRequest Request { get; init; }

    public GetSalesArrangementsByCaseIdMediatrRequest(Contracts.GetSalesArrangementsByCaseIdRequest request)
    {
        Request = request;
    }
}
