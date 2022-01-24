using DomainServices.DocumentService.Api.Dto;
using DomainServices.DocumentService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.DocumentService.Api.Services;

[Authorize]
public class DocumentService : Contracts.v1.DocumentService.DocumentServiceBase
{
    private readonly IMediator _mediator;

    public DocumentService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<GetDocumentResponse> GetDocument(GetDocumentRequest request, ServerCallContext context)
        => await _mediator.Send(new GetDocumentMediatrRequest(request));

    public override async Task<GetDocumentsListResponse> GetDocumentsListByCaseId(GetDocumentsListByCaseIdRequest request, ServerCallContext context)
        => await _mediator.Send(new GetDocumentsListByCaseIdMediatrRequest(request));

    public override async Task<GetDocumentsListResponse> GetDocumentsListByContractNumber(GetDocumentsListByContractNumberRequest request, ServerCallContext context)
       => await _mediator.Send(new GetDocumentsListByContractNumberMediatrRequest(request));

    public override async Task<GetDocumentsListResponse> GetDocumentsListByCustomerId(GetDocumentsListByCustomerIdRequest request, ServerCallContext context)
        => await _mediator.Send(new GetDocumentsListByCustomerIdMediatrRequest(request));

    public override async Task<GetDocumentsListResponse> GetDocumentsListByRelationId(GetDocumentsListByRelationIdRequest request, ServerCallContext context)
       => await _mediator.Send(new GetDocumentsListByRelationIdMediatrRequest(request));
}
