namespace DomainServices.CaseService.Api.Dto;

internal sealed class UpdateCaseDataMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.UpdateCaseDataRequest Request { get; init; }

    public UpdateCaseDataMediatrRequest(Contracts.UpdateCaseDataRequest request)
    {
        Request = request;
    }
}
