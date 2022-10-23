using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Dto;

internal sealed class UpdateOfferContactsMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public UpdateOfferContactsRequest Request { get; set; }

    public UpdateOfferContactsMediatrRequest(UpdateOfferContactsRequest request)
    {
        this.Request = request;
    }
}
