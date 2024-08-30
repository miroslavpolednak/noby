using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Api.Common;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListHandler(
       DocumentOnSAServiceDbContext _dbContext,
       ISalesArrangementServiceClient _arrangementServiceClient,
       ICodebookServiceClient _codebookServiceClients,
       IDocumentOnSaMapper _documentOnSaMapper,
       IHouseholdServiceClient _householdClient,
       ICustomerOnSAServiceClient _customerOnSAServiceClient,
       ICommonSigningMethods _commonSigningMethods) 
    : IRequestHandler<GetDocumentsToSignListRequest, GetDocumentsToSignListResponse>
{
    public async Task<GetDocumentsToSignListResponse> Handle(GetDocumentsToSignListRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var salesArrangementType = await _commonSigningMethods.GetSalesArrangementType(salesArrangement, cancellationToken);

        var documentOnSaEntities = await _dbContext.DocumentOnSa
            .AsNoTracking()
            .Include(i => i.EArchivIdsLinkeds)
            .Include(s => s.SigningIdentities)
            .Where(e => e.SalesArrangementId == request.SalesArrangementId
                                                              && e.IsValid
                                                              && !e.IsFinal)
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
        var documentTypeId = documentTypes.SingleOrDefault(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId)?.Id;
        var documentsOnSaToSignVirtual = _documentOnSaMapper.CreateDocumentOnSaToSign(documentTypeId, request.SalesArrangementId);
        
        // Map real service request (should by only one here for non worflow SA, for WF documentTypeId gonna be null and many docsOnSa are possible)
        var documentsOnSasReal = documentOnSaEntities.Where(r => r.DocumentTypeId == documentsOnSaToSignVirtual.DocumentTypeId);

        if (documentsOnSasReal.Any())
        {
            foreach (var documentOnSaReal in documentsOnSasReal)
            {
                var documentsOnSaToSignReal = _documentOnSaMapper.MapDocumentOnSaToSign([documentOnSaReal]);
                response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignReal);
            }
        }
        else if (documentTypeId is not null) // For WF virtual documentOnSa doesn't make sense (documentTypeId is null). 
        {
            // Map virtual
            response.DocumentsOnSAToSign.Add(documentsOnSaToSignVirtual);
        }

        // Map real CRS (DocumentTypeId == 13)
        var documentsOnSaRealCrs = documentOnSaEntities.Where(r => r.DocumentTypeId == (int)DocumentTypes.DANRESID); // 13
        var documentsOnSaToSignRealCrs = _documentOnSaMapper.MapDocumentOnSaToSign(documentsOnSaRealCrs);
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
        var virtualDocumentsOnSaCrs = await _documentOnSaMapper.CreateDocumentOnSaToSign(customersOnSaIdsWithCRSChange, request.SalesArrangementId, cancellationToken);

        return MergeVirtualWithExistCrs(virtualDocumentsOnSaCrs, documentOnSaEntities);
    }

    /// <summary>
    /// Get only virtual CRS DocOnSa without existing DocOnSa (real) in DB 
    /// </summary>
    private static IEnumerable<DocumentOnSAToSign> MergeVirtualWithExistCrs(IReadOnlyCollection<DocumentOnSAToSign> virtualDocumentsOnSaCrs, List<DocumentOnSa> documentOnSaEntities)
    {
        foreach (var virtualCrs in virtualDocumentsOnSaCrs)
        {
            var docOnSaEntity = documentOnSaEntities.SingleOrDefault(r => r.SalesArrangementId == virtualCrs.SalesArrangementId
                                                                     && r.DocumentTypeId == virtualCrs.DocumentTypeId
                                                                     && r.CustomerOnSAId1 == virtualCrs.CustomerOnSA.CustomerOnSAId);
            if (docOnSaEntity is null)
            {
                yield return virtualCrs;
            }
        }
    }
}
