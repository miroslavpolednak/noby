using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.DocumentOnSAService.Api.Endpoints;

internal sealed class DocumentOnSAServiceGrpc : Contracts.v1.DocumentOnSAService.DocumentOnSAServiceBase
{
    private readonly IMediator _mediator;

    public DocumentOnSAServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GenerateFormIdResponse> GenerateFormId(GenerateFormIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<StartSigningResponse> StartSigning(StartSigningRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> StopSigning(StopSigningRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(GetDocumentsToSignListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(GetDocumentOnSADataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SignDocumentManually(SignDocumentManuallyRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentsOnSAListResponse> GetDocumentsOnSAList(GetDocumentsOnSAListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateDocumentOnSAResponse> CreateDocumentOnSA(CreateDocumentOnSARequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> LinkEArchivIdToDocumentOnSA(LinkEArchivIdToDocumentOnSARequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetElectronicDocumentFromQueueResponse> GetElectronicDocumentFromQueue(GetElectronicDocumentFromQueueRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetElectronicDocumentPreviewResponse> GetElectronicDocumentPreview(GetElectronicDocumentPreviewRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SetDocumentOnSAArchived(SetDocumentOnSAArchivedRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SendDocumentPreview(SendDocumentPreviewRequest request, ServerCallContext context) 
    => await _mediator.Send(request, context.CancellationToken);

    public override async Task<DomainServices.DocumentOnSAService.Contracts.v1.Test1Response> Test1(DomainServices.DocumentOnSAService.Contracts.v1.Test1Request request, ServerCallContext context)
        => await _mediator.Send(new DomainServices.DocumentOnSAService.Api.Endpoints.Test.Test1MediatrRequest(), context.CancellationToken);
    public override async Task<DomainServices.DocumentOnSAService.Contracts.v1.Test2Response> Test2(DomainServices.DocumentOnSAService.Contracts.v1.Test2Request request, ServerCallContext context)
        => await _mediator.Send(new DomainServices.DocumentOnSAService.Api.Endpoints.Test.Test2MediatrRequest() { Id = request.Id }, context.CancellationToken);
    public override async Task<DomainServices.DocumentOnSAService.Contracts.v1.Test3Response> Test3(DomainServices.DocumentOnSAService.Contracts.v1.Test3Request request, ServerCallContext context)
        => await _mediator.Send(new DomainServices.DocumentOnSAService.Api.Endpoints.Test.Test3MediatrRequest() { Id = request.Id }, context.CancellationToken);
}
