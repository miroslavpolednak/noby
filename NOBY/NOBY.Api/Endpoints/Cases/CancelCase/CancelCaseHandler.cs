using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

internal sealed class CancelCaseHandler
    : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await GetProductSalesArrangement(request.CaseId, cancellationToken);
        var documentType = await GetDocumentType(DocumentTypes.ODSTOUP, cancellationToken);
        var customerOnSas = await _customerOnSaService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        
        foreach (var customerOnSa in customerOnSas)
        {
            var getDocumentRequest = CreateGetDocumentDataRequest(salesArrangement, customerOnSa, documentType);
            var getDocumentResponse = await _dataAggregatorService.GetDocumentData(getDocumentRequest, cancellationToken);

            var generateDocumentRequest = CreateGenerateDocumentRequest(documentType);
            var generateDocumentResponse = await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
        
            var uploadRequest = new UploadDocumentRequest
            {
                BinaryData = generateDocumentResponse.Data,
                Metadata = new DocumentMetadata
                {
                    EaCodeMainId = documentType.EACodeMainId,
                    // CaseId = salesArrangement.CaseId,
                    //
                }
            };

            await _documentArchiveService.UploadDocument(uploadRequest, cancellationToken);
        }

        await _caseService.CancelCase(request.CaseId, cancellationToken: cancellationToken);
        
        return new CancelCaseResponse
        {
            // State = State.Cancelled,
            // StateName = "",
            CustomersOnSa = customerOnSas.Select(c => new CustomerOnSAItem
            {
                CustomerOnSAId = c.CustomerOnSAId,
                BirthDate = c.DateOfBirthNaturalPerson,
                FirstName = c.FirstNameNaturalPerson,
                LastName = c.Name
            }).ToList()
        };
    }

    private async Task<DomainServices.SalesArrangementService.Contracts.SalesArrangement> GetProductSalesArrangement(long caseId, CancellationToken cancellationToken)
    {
        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
        return salesArrangementResponse.SalesArrangements.First(s => s.IsProductSalesArrangement());
    }

    private async Task<DocumentTypesResponse.Types.DocumentTypeItem> GetDocumentType(DocumentTypes documentType, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        return documentTypes.First(t => t.Id == documentType.ToByte());
    }

    private static GetDocumentDataRequest CreateGetDocumentDataRequest(
        DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement,
        CustomerOnSA customerOnSa,
        DocumentTypesResponse.Types.DocumentTypeItem documentType) => new()
    {
        DocumentTypeId = documentType.Id,
        InputParameters = new InputParameters
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            CaseId = salesArrangement.CaseId,
            CustomerOnSaId = customerOnSa.CustomerOnSAId,
            // CustomerIdentity = 
        }
    };

    private static GenerateDocumentRequest CreateGenerateDocumentRequest(
        DocumentTypesResponse.Types.DocumentTypeItem documentType) => new()
    {
        DocumentTypeId = documentType.Id
    };
    
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly IDataAggregatorServiceClient _dataAggregatorService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICaseServiceClient _caseService;

    public CancelCaseHandler(
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSaService,
        IDataAggregatorServiceClient dataAggregatorService,
        IDocumentGeneratorServiceClient documentGeneratorService,
        IDocumentArchiveServiceClient documentArchiveService,
        ICaseServiceClient caseService)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSaService = customerOnSaService;
        _dataAggregatorService = dataAggregatorService;
        _documentGeneratorService = documentGeneratorService;
        _documentArchiveService = documentArchiveService;
        _caseService = caseService;
    }
}