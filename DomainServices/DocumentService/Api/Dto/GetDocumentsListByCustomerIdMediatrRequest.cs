using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;

internal sealed class GetDocumentsListByCustomerIdMediatrRequest : IRequest<GetDocumentsListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public string CustomerId { get; init; }

    public GetDocumentsListByCustomerIdMediatrRequest(GetDocumentsListByCustomerIdRequest request)
    {
        CustomerId = request.CustomerId; //TODO: request params?
    }
}