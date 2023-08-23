﻿using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using ExternalServices.ESignatures.V1;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListHandler : IRequestHandler<GetDocumentsToSignListRequest, GetDocumentsToSignListResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClients;
    private readonly IDocumentOnSaMapper _documentOnSaMapper;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly IMediator _mediator;

    public GetDocumentsToSignListHandler(
        DocumentOnSAServiceDbContext dbContext,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICodebookServiceClient codebookServiceClients,
        IDocumentOnSaMapper documentOnSaMapper,
        IHouseholdServiceClient householdClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        IESignaturesClient eSignaturesClient,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _documentOnSaMapper = documentOnSaMapper;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _eSignaturesClient = eSignaturesClient;
        _mediator = mediator;
    }

    public async Task<GetDocumentsToSignListResponse> Handle(GetDocumentsToSignListRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var salesArrangementType = await GetSalesArrangementType(salesArrangement, cancellationToken);

        var documentOnSaEntities = await _dbContext.DocumentOnSa
            .AsNoTracking()
            .Include(i => i.EArchivIdsLinkeds)
            .Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                              && e.IsValid
                                                              && e.IsFinal == false)
                                                  .ToListAsync(cancellationToken);


        var response = new GetDocumentsToSignListResponse();

        if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            await ComposeResultForProductRequest(request, response, documentOnSaEntities, cancellationToken);
        }
        else if (salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ServiceRequest)
        {
            await ComposeResultForServiceRequest(request, salesArrangement, response, documentOnSaEntities, cancellationToken);
        }
        else
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.SalesArrangementCategoryNotSupported, salesArrangementType.SalesArrangementCategory);
        }

        // Evaluate eletronic signature status 
        await EvaluateElectronicDocumentStatus(documentOnSaEntities, cancellationToken);

        return response;
    }

    private async Task ComposeResultForServiceRequest(
        GetDocumentsToSignListRequest request,
        SalesArrangement salesArrangement,
        GetDocumentsToSignListResponse response,
        List<DocumentOnSa> documentOnSaEntities,
        CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var documentType = documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId);
        var documentsOnSaToSignVirtual = _documentOnSaMapper.CreateDocumentOnSaToSign(documentType, request.SalesArrangementId);
        var documentOnSaReal = documentOnSaEntities.FirstOrDefault(r => r.DocumentTypeId == documentsOnSaToSignVirtual.DocumentTypeId);

        if (documentOnSaReal is not null)
        {
            // Map real service request (should by only one here)
            var documentsOnSaToSignReal = _documentOnSaMapper.MapDocumentOnSaToSign(new List<DocumentOnSa>() { documentOnSaReal });
            response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignReal);
        }
        else
        {
            // Map virtual
            response.DocumentsOnSAToSign.Add(documentsOnSaToSignVirtual);
        }

        // Map real CRS (DocumentTypeId == 13)
        var documentsOnSaReal = documentOnSaEntities.Where(r => r.DocumentTypeId == (int)DocumentTypes.DANRESID); // 13
        var documentsOnSaToSignRealCrs = _documentOnSaMapper.MapDocumentOnSaToSign(documentsOnSaReal);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignRealCrs);

        // Map virtual CRS
        if (documentsOnSaToSignVirtual.DocumentTypeId is (int)DocumentTypes.ZUSTAVSI or (int)DocumentTypes.PRISTOUP or (int)DocumentTypes.ZADOSTHD_SERVICE) // 11, 12, 16
        {
            var virtualDocumentOnCrs = await CreateVirtualDocumentOnSaCrs(request, documentOnSaEntities, cancellationToken);
            response.DocumentsOnSAToSign.AddRange(virtualDocumentOnCrs);
        }
    }

    private async Task ComposeResultForProductRequest(
        GetDocumentsToSignListRequest request,
        GetDocumentsToSignListResponse response,
        List<DocumentOnSa> documentOnSaEntities,
        CancellationToken cancellationToken)
    {
        // Real (include CRS)
        var documentsOnSaToSignReal = _documentOnSaMapper.MapDocumentOnSaToSign(documentOnSaEntities);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignReal);

        // Product virtual
        var households = await _householdClient.GetHouseholdList(request.SalesArrangementId, cancellationToken);
        var householdsWithoutdocumentOnsa = households
                                            .Where(h => !documentOnSaEntities.Select(d => d.HouseholdId)
                                            .Contains(h.HouseholdId));

        var documentsOnSaToSignVirtual = _documentOnSaMapper.CreateDocumentOnSaToSign(householdsWithoutdocumentOnsa);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignVirtual);

        // CRS virtual
        var virtualDocumentOnCrs = await CreateVirtualDocumentOnSaCrs(request, documentOnSaEntities, cancellationToken);
        response.DocumentsOnSAToSign.AddRange(virtualDocumentOnCrs);
    }

    private async Task<IEnumerable<DocumentOnSAToSign>> CreateVirtualDocumentOnSaCrs(GetDocumentsToSignListRequest request, List<DocumentOnSa> documentOnSaEntities, CancellationToken cancellationToken)
    {
        var customersChangeMetadata = await _customerOnSAServiceClient.GetCustomerChangeMetadata(request.SalesArrangementId, cancellationToken);
        var customersOnSaIdsWithCRSChange = customersChangeMetadata!.Where(r => r.CustomerChangeMetadata.WasCRSChanged).Select(r => r.CustomerOnSAId);
        var virtualDocumentsOnSaCrs = _documentOnSaMapper.CreateDocumentOnSaToSign(customersOnSaIdsWithCRSChange, request.SalesArrangementId);

        return MergeVirtualWithExistCrs(virtualDocumentsOnSaCrs, documentOnSaEntities);
    }

    /// <summary>
    /// Get only virtual CRS DocOnSa without existing DocOnSa (real) in DB 
    /// </summary>
    private static IEnumerable<DocumentOnSAToSign> MergeVirtualWithExistCrs(IEnumerable<DocumentOnSAToSign> virtualDocumentsOnSaCrs, List<DocumentOnSa> documentOnSaEntities)
    {
        foreach (var virtualCrs in virtualDocumentsOnSaCrs)
        {
            var docOnSaEntity = documentOnSaEntities.SingleOrDefault(r => r.SalesArrangementId == virtualCrs.SalesArrangementId
                                                                     && r.DocumentTypeId == virtualCrs.DocumentTypeId
                                                                     && r.CustomerOnSAId1 == virtualCrs.CustomerOnSAId);
            if (docOnSaEntity is null)
            {
                yield return virtualCrs;
            }
        }
    }

    private async Task EvaluateElectronicDocumentStatus(List<DocumentOnSa> documentsOnSaRealEntity, CancellationToken cancellationToken)
    {
        foreach (var docOnSa in documentsOnSaRealEntity)
        {
            if (docOnSa.SignatureTypeId is null or not ((int)SignatureTypes.Electronic))
                continue;

            var elDocumentStatus = await _eSignaturesClient.GetDocumentStatus(docOnSa.ExternalId!, cancellationToken);

            switch (elDocumentStatus)
            {
                case EDocumentStatuses.SIGNED or EDocumentStatuses.VERIFIED or EDocumentStatuses.SENT:
                    await _mediator.Send(new SignDocumentRequest { DocumentOnSAId = docOnSa.DocumentOnSAId, SignatureTypeId = docOnSa.SignatureTypeId }, cancellationToken);
                    break;
                case EDocumentStatuses.DELETED:
                    await _mediator.Send(new StopSigningRequest { DocumentOnSAId = docOnSa.DocumentOnSAId }, cancellationToken);
                    break;
                case EDocumentStatuses.NEW or EDocumentStatuses.IN_PROGRESS or EDocumentStatuses.APPROVED:
                    // Ignore
                    break;
                default:
                    throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedStatusReturnedFromESignature, elDocumentStatus);
            }
        }
    }

    private async Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookServiceClients.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
        return salesArrangementType;
    }
}
