namespace DomainServices.CaseService.Api.Dto.CaseService;

internal sealed class UpdateCaseCustomerMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.UpdateCaseCustomerRequest Request { get; init; }

    public UpdateCaseCustomerMediatrRequest(Contracts.UpdateCaseCustomerRequest request)
    {
        Request = request;
    }
}
