namespace DomainServices.CaseService.Api.Dto.SalesArrangement;

internal sealed class GetSalesArrangementsByCaseIdMediatrRequest
    : IRequest<Contracts.GetSalesArrangementsByCaseIdResponse>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int[] States { get; init; }

    public GetSalesArrangementsByCaseIdMediatrRequest(Contracts.GetSalesArrangementsByCaseIdRequest request)
    {
        this.CaseId = request.CaseId;
        this.States = request.States != null && request.States.Any() ? request.States.ToArray() : new int[0];
    }
}
