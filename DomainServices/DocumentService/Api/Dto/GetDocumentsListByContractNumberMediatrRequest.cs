using CIS.Core.Enums;
using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;
internal sealed class GetDocumentsListByContractNumberMediatrRequest : IRequest<GetDocumentsListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public string ContractNumber { get; init; }
    public IdentitySchemes Mandant { get; init; }

    public GetDocumentsListByContractNumberMediatrRequest(GetDocumentsListByContractNumberRequest request)
    {
        ContractNumber = request.ContractNumber;
        Mandant = IdentitySchemes.Unknown; //request.Mandant;
    }
}