﻿using DomainServices.DocumentOnSAService.Contracts;
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

    public override async Task<Empty> SignDocument(SignDocumentRequest request, ServerCallContext context)
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

    public override async Task<Empty> RefreshElectronicDocument(RefreshElectronicDocumentRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentOnSAByFormIdResponse> GetDocumentOnSAByFormId(GetDocumentOnSAByFormIdRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> RefreshSalesArrangementState(RefreshSalesArrangementStateRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SetProcessingDateInSbQueues(SetProcessingDateInSbQueuesRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentOnSAStatusResponse> GetDocumentOnSAStatus(GetDocumentOnSAStatusRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);
}
