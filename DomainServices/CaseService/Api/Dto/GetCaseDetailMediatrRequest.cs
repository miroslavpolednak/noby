namespace DomainServices.CaseService.Api.Dto;

internal sealed class GetCaseDetailMediatrRequest
    : IRequest<Contracts.CaseModel>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }

    public GetCaseDetailMediatrRequest(Contracts.GetCaseDetailRequest request)
    {
        CaseId = request.CaseId;
    }
}
