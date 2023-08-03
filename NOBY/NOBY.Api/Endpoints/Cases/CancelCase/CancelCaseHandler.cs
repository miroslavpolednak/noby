using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

internal sealed class CancelCaseHandler
    : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellationToken)
    {
        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var salesArrangement = salesArrangementResponse.SalesArrangements.FirstOrDefault(s => s.IsProductSalesArrangement())
            ?? throw new NobyValidationException("");

        var customerOnSas = await _customerOnSaService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var documentTypeId = DocumentTypes.ODSTOUP.ToByte();
        var documentType = documentTypes.FirstOrDefault(t => t.Id == documentTypeId);
        
        foreach (var customerOnSa in customerOnSas)
        {
            var getDocumentRequest = new GetDocumentDataRequest
            {
                DocumentTypeId = documentTypeId,
                InputParameters = new InputParameters
                {
                    SalesArrangementId = salesArrangement.SalesArrangementId,
                    CaseId = salesArrangement.CaseId,
                    CustomerOnSaId = customerOnSa.CustomerOnSAId,
                    // CustomerIdentity = 
                }
            };
            
            var documentResponse = await _dataAggregatorService.GetDocumentData(getDocumentRequest, cancellationToken);
            
            var generateDocumentRequest = new GenerateDocumentRequest
            {
                DocumentTypeId = documentTypeId,
            };

            var generateResponse = await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
            
            var uploadRequest = new UploadDocumentRequest
            {
                BinaryData = generateResponse.Data,
                Metadata = new DocumentMetadata
                {
                    EaCodeMainId = documentType?.EACodeMainId,
                    CaseId = salesArrangement.CaseId,
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