﻿using DomainServices.DocumentOnSAService.Contracts;
using static DomainServices.DocumentOnSAService.Contracts.v1.DocumentOnSAService;

namespace DomainServices.DocumentOnSAService.Clients.Services;
public class DocumentOnSAService : IDocumentOnSAServiceClient
{
    private readonly DocumentOnSAServiceClient _client;

    public DocumentOnSAService(DocumentOnSAServiceClient client)
    {
        _client = client;
    }

    public async Task<string> GenerateFormId(GenerateFormIdRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _client.GenerateFormIdAsync(request, cancellationToken: cancellationToken);
        return result.FormId;
    }

    public async Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentsToSignListAsync(new GetDocumentsToSignListRequest
        {
            SalesArrangementId = salesArrangementId
        }, cancellationToken: cancellationToken);
    }

    public async Task<StartSigningResponse> StartSigning(StartSigningRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.StartSigningAsync(request, cancellationToken: cancellationToken);
    }

    public async Task StopSigning(StopSigningRequest request, CancellationToken cancellationToken = default)
    {
        await _client.StopSigningAsync(new StopSigningRequest
        {
            DocumentOnSAId = request.DocumentOnSAId,
            NotifyESignatures = request.NotifyESignatures
        }, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentOnSADataAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task SignDocument(int documentOnSAId, int signatureTypeId, CancellationToken cancellationToken = default)
    {
        await _client.SignDocumentAsync(new() { DocumentOnSAId = documentOnSAId, SignatureTypeId = signatureTypeId }, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentsOnSAListResponse> GetDocumentsOnSAList(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentsOnSAListAsync(new GetDocumentsOnSAListRequest { SalesArrangementId = salesArrangementId }, cancellationToken: cancellationToken);
    }

    public async Task<CreateDocumentOnSAResponse> CreateDocumentOnSA(CreateDocumentOnSARequest request, CancellationToken cancellationToken = default)
    {
        return await _client.CreateDocumentOnSAAsync(request, cancellationToken: cancellationToken);
    }

    public async Task LinkEArchivIdToDocumentOnSA(LinkEArchivIdToDocumentOnSARequest request, CancellationToken cancellationToken = default)
    {
        await _client.LinkEArchivIdToDocumentOnSAAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetElectronicDocumentFromQueueResponse> GetElectronicDocumentFromQueue(GetElectronicDocumentFromQueueRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.GetElectronicDocumentFromQueueAsync(request, cancellationToken: cancellationToken);
    }

    public async Task SetDocumentOnSAArchived(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        await _client.SetDocumentOnSAArchivedAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task<GetElectronicDocumentPreviewResponse> GetElectronicDocumentPreview(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        return await _client.GetElectronicDocumentPreviewAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task SendDocumentPreview(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        await _client.SendDocumentPreviewAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task RefreshElectronicDocument(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        await _client.RefreshElectronicDocumentAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentOnSAByFormIdResponse> GetDocumentOnSAByFormId(string formId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentOnSAByFormIdAsync(new() { FormId = formId }, cancellationToken: cancellationToken);
    }
}
