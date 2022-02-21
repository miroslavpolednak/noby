using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class UpdateMortgageMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public UpdateMortgageRequest Request { get; init; }

    public UpdateMortgageMediatrRequest(UpdateMortgageRequest request)
    {
        Request = request;
    }
}