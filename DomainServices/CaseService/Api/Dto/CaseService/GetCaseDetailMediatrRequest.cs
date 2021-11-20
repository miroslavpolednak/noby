namespace DomainServices.CaseService.Api.Dto.CaseService;

internal sealed class GetCaseDetailMediatrRequest
    : IRequest<Contracts.GetCaseDetailResponse>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }

    public GetCaseDetailMediatrRequest(Contracts.GetCaseDetailRequest request)
    {
        CaseId = request.CaseId;
    }
}
