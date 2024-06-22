namespace NOBY.Api.Endpoints.Cases.CancelCase;

public sealed record CancelCaseRequest(long CaseId) 
    : IRequest<CasesCancelCaseResponse>
{
}