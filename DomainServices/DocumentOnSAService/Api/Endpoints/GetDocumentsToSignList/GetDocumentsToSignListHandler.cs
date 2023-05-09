using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Mappers;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListHandler : IRequestHandler<GetDocumentsToSignListRequest, GetDocumentsToSignListResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClients _codebookServiceClients;
    private readonly IDocumentOnSaMapper _documentOnSaMapper;
    private readonly IHouseholdServiceClient _householdClient;

    public GetDocumentsToSignListHandler(
        DocumentOnSAServiceDbContext dbContext,
        ISalesArrangementServiceClient arrangementServiceClient,
        ICodebookServiceClients codebookServiceClients,
        IDocumentOnSaMapper documentOnSaMapper,
        IHouseholdServiceClient householdClient)
    {
        _dbContext = dbContext;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _documentOnSaMapper = documentOnSaMapper;
        _householdClient = householdClient;
    }

    public async Task<GetDocumentsToSignListResponse> Handle(GetDocumentsToSignListRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);
           
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
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.SalesArrangementCategoryNotSupported, salesArrangementType.SalesArrangementCategory);
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

        var documentsOnSaToSignReal = _documentOnSaMapper.MapDocumentOnSaToSign(documentsOnSaRealEntity);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignReal);


        var households = await _householdClient.GetHouseholdList(request.SalesArrangementId!.Value, cancellationToken);
        var householdsWithoutdocumentOnsa = households
                                            .Where(h => !documentsOnSaRealEntity.Select(d => d.HouseholdId)
                                            .Contains(h.HouseholdId));

        var documentsOnSaToSignVirtual = CreateDocumentOnSaToSign(householdsWithoutdocumentOnsa);
        response.DocumentsOnSAToSign.AddRange(documentsOnSaToSignVirtual);
    }

    private static DocumentOnSAToSign CreateDocumentOnSaToSign(DocumentTypeItem documentTypeItem, int salesArrangementId)
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

    private static IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<Household> households)
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

    private static int GetDocumentTypeId(int householdTypeId) => householdTypeId switch
    {
        1 => 4,
        2 => 5,
        _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.HouseholdTypeIdNotExist, householdTypeId)
    };
}
