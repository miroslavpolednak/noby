using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using CIS.Foms.Types.Enums;
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
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        var salesArrangementType = await GetSalesArrangementType(salesArrangement, cancellationToken);

        var documentOnSaEntities = await _dbContext.DocumentOnSa
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
        var documentsOnSaToSignVirtual = _documentOnSaMapper.CreateDocumentOnSaToSign(documentType, request.SalesArrangementId!.Value);
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
        var households = await _householdClient.GetHouseholdList(request.SalesArrangementId!.Value, cancellationToken);
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
        var customersChangeMetadata = await _customerOnSAServiceClient.GetCustomerChangeMetadata(request.SalesArrangementId!.Value, cancellationToken);
        var customersOnSaIdsWithCRSChange = customersChangeMetadata!.Where(r => r.CustomerChangeMetadata.WasCRSChanged).Select(r => r.CustomerOnSAId);
        var virtualDocumentsOnSaCrs = _documentOnSaMapper.CreateDocumentOnSaToSign(customersOnSaIdsWithCRSChange, request.SalesArrangementId!.Value);

        // Get only virtual CRS DocOnSa without existing DocOnSa (real) in DB 
        var mergedVirtualDocumentsOnSa = virtualDocumentsOnSaCrs.Where(m =>
                    !documentOnSaEntities.Select(s => s.SalesArrangementId).Contains(m.SalesArrangementId) &&
                    !documentOnSaEntities.Select(s => s.DocumentTypeId).Contains(m.DocumentTypeId) &&
                    !documentOnSaEntities.Select(s => s.CustomerOnSAId1).Contains(m.CustomerOnSAId));

        return mergedVirtualDocumentsOnSa;
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
                    docOnSa.IsValid = false;
                    break;
                case EDocumentStatuses.NEW or EDocumentStatuses.IN_PROGRESS or EDocumentStatuses.APPROVED:
                    // Ignore
                    break;
                default:
                    throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedStatusReturnedFromESignature, elDocumentStatus);
            }
        }

        if (_dbContext.ChangeTracker.HasChanges())
            await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookServiceClients.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
        return salesArrangementType;
    }
}
