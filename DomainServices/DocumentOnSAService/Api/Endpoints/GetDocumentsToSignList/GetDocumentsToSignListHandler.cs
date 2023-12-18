using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Api.Common;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListHandler : IRequestHandler<GetDocumentsToSignListRequest, GetDocumentsToSignListResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClients;
    private readonly IDocumentOnSaMapper _documentOnSaMapper;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly ICommonSigningMethods _commonSigningMethods;

    public GetDocumentsToSignListHandler(
           DocumentOnSAServiceDbContext dbContext,
           ISalesArrangementServiceClient arrangementServiceClient,
           ICodebookServiceClient codebookServiceClients,
           IDocumentOnSaMapper documentOnSaMapper,
           IHouseholdServiceClient householdClient,
           ICustomerOnSAServiceClient customerOnSAServiceClient,
           ICommonSigningMethods commonSigningMethods)
    {
        _dbContext = dbContext;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _documentOnSaMapper = documentOnSaMapper;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _commonSigningMethods = commonSigningMethods;
    }

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
        var documentType = documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId);
        var documentsOnSaToSignVirtual = _documentOnSaMapper.CreateDocumentOnSaToSign(documentType, request.SalesArrangementId);
        var documentOnSaReal = documentOnSaEntities.Find(r => r.DocumentTypeId == documentsOnSaToSignVirtual.DocumentTypeId);

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
