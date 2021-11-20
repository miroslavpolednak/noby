using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Dto.CaseService;

internal sealed class CreateCaseMediatrRequest
    : IRequest<CreateCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    public CreateCaseRequest Request { get; init; }

    public CreateCaseMediatrRequest(CreateCaseRequest request)
    {
        Request = request;
    }
}
