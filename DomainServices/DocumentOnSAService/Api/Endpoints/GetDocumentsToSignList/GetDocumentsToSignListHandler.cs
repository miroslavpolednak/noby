﻿using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using _Entity = DomainServices.DocumentOnSAService.Api.Database.Entities;
namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListHandler : IRequestHandler<GetDocumentsToSignListRequest, GetDocumentsToSignListResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly IHouseholdServiceClient _householdClient;

    public GetDocumentsToSignListHandler(
        DocumentOnSAServiceDbContext dbContext,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICodebookServiceClients codebookServiceClients,
        IHouseholdServiceClient householdClient)
    {
        _dbContext = dbContext;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _householdClient = householdClient;
    }

    public async Task<GetDocumentsToSignListResponse> Handle(GetDocumentsToSignListRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId!.Value} does not exist.");
        }

        var salesArrangementType = await GetSalesArrangementType(salesArrangement, cancellationToken);

        var response = new GetDocumentsToSignListResponse();

        if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            await ComposeResultForProductRequest(request, response, cancellationToken);
        }
        else if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ServiceRequest)
        {
            await ComposeResultForServiceRequest(request, salesArrangement, response, cancellationToken);
        }
        else
        {
            throw new ArgumentException($"This kind of {nameof(SalesArrangementCategories)} {salesArrangementType.SalesArrangementCategory} is not supported");
        }

        return response;
    }

    private async Task ComposeResultForServiceRequest(GetDocumentsToSignListRequest request, SalesArrangement salesArrangement, GetDocumentsToSignListResponse response, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var documentType = documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId);
        var documentsOnSaToSignVirtual = CreateDocumentOnSaToSign(documentType, request.SalesArrangementId!.Value);
        response.DocumentsOnSAToSign.Add(documentsOnSaToSignVirtual);
    }

    private async Task ComposeResultForProductRequest(GetDocumentsToSignListRequest request, GetDocumentsToSignListResponse response, CancellationToken cancellationToken)
    {
        var documentsOnSaRealEntity = await _dbContext.DocumentOnSa
                                                    .AsNoTracking()
                                                    .Where(e => e.SalesArrangementId == request.SalesArrangementId && e.IsValid)
                                                    .ToListAsync(cancellationToken);

        var documentsOnSaToSignReal = CreateDocumentOnSaToSign(documentsOnSaRealEntity);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignReal);


        var households = await _householdClient.GetHouseholdList(request.SalesArrangementId!.Value, cancellationToken);
        var householdsWithoutdocumentOnsa = households
                                            .Where(h => !documentsOnSaRealEntity.Select(d => d.HouseholdId)
                                            .Contains(h.HouseholdId));

        var documentsOnSaToSignVirtual = CreateDocumentOnSaToSign(householdsWithoutdocumentOnsa);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignVirtual);
    }

    private IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<_Entity.DocumentOnSa> documentOnSas)
    {
        foreach (var documentOnSa in documentOnSas)
        {
            yield return new DocumentOnSAToSign
            {
                DocumentOnSAId = documentOnSa.DocumentOnSAId,
                DocumentTypeId = documentOnSa.DocumentTypeId,
                DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId,
                FormId = documentOnSa.FormId ?? string.Empty,
                EArchivId = documentOnSa.EArchivId ?? string.Empty,
                DmsxId = documentOnSa.DmsxId ?? string.Empty,
                SalesArrangementId = documentOnSa.SalesArrangementId,
                HouseholdId = documentOnSa.HouseholdId,
                IsValid = documentOnSa.IsValid,
                IsSigned = documentOnSa.IsSigned,
                IsDocumentArchived = documentOnSa.IsDocumentArchived,
                SignatureMethodCode = documentOnSa.SignatureMethodCode ?? string.Empty,
                SignatureDateTime = documentOnSa.SignatureDateTime is not null ? Timestamp.FromDateTime(documentOnSa.SignatureDateTime.Value) : null,
                SignatureConfirmedBy = documentOnSa.SignatureConfirmedBy
            };
        }
    }

    private DocumentOnSAToSign CreateDocumentOnSaToSign(DocumentTypeItem documentTypeItem, int salesArrangementId)
    {
        return new DocumentOnSAToSign
        {
            DocumentTypeId = documentTypeItem.Id,
            SalesArrangementId = salesArrangementId,
            IsValid = true,
            IsSigned = false,
            IsDocumentArchived = false
        };

    }

    private IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<Household> households)
    {
        foreach (var household in households)
        {
            yield return new DocumentOnSAToSign
            {
                DocumentTypeId = GetDocumentTypeId(household.HouseholdTypeId),
                SalesArrangementId = household.SalesArrangementId,
                HouseholdId = household.HouseholdId,
                IsValid = true,
                IsSigned = false,
                IsDocumentArchived = false
            };
        }
    }

    private async Task<SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookServiceClients.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
        return salesArrangementType;
    }

    private int GetDocumentTypeId(int householdTypeId) => householdTypeId switch
    {
        1 => 4,
        2 => 5,
        _ => throw new ArgumentException($"HouseholdTypeId {householdTypeId} does not exist.", nameof(householdTypeId))
    };
}