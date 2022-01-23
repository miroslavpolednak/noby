using DomainServices.DocumentService.Contracts;

namespace DomainServices.DocumentService.Api.Dto;

internal sealed class GetDocumentsListByCaseIdMediatrRequest : IRequest<GetDocumentsListByCaseIdResponse>, CIS.Core.Validation.IValidatableRequest
{
    public Int32 CaseId { get; init; }

    public GetDocumentsListByCaseIdMediatrRequest(GetDocumentsListByCaseIdRequest request)
    {
        CaseId = request.CaseId;
    }
}

