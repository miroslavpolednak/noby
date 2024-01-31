using System.Globalization;
using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using FastEnumUtility;
using Google.Protobuf;
using NOBY.Api.Endpoints.Document.GeneralDocument;
using NOBY.Api.Endpoints.Document.SharedDto;
using NOBY.Services.DocumentHelper;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

internal sealed class CancelCaseHandler : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    const DocumentTypes _documentType = DocumentTypes.ODSTOUP;
    
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellationToken)
    {
        var documentTypeItem = await getDocumentType(_documentType, cancellationToken);
        
        var salesArrangement = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First();
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var customerOnSas = await _customerOnSaService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(s => s.Id == caseDetail.State);

        var responseModel = new CancelCaseResponse
        {
            State = (CaseStates)caseDetail.State,
            StateName = caseState.Name,
            CustomersOnSa = new List<CustomerOnSAItem>(customerOnSas.Count)
        };

        foreach (var customerOnSa in customerOnSas.Where(t => (t.CustomerIdentifiers?.Any() ?? false)))
        {
            var getGeneralDocumentRequest = new GetGeneralDocumentRequest
            {
                DocumentType = _documentType,
                ForPreview = false,
                InputParameters = new InputParameters
                {
                    SalesArrangementId = salesArrangement.SalesArrangementId,
                    CustomerOnSaId = customerOnSa.CustomerOnSAId
                }
            };
            
            var generateDocumentRequest = await _documentGenerator.CreateRequest(getGeneralDocumentRequest, cancellationToken);
            var documentData = await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);
            var documentId = await _documentArchiveService.GenerateDocumentId(new (), cancellationToken);

            var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
            var authorUserLogin = _documentHelper.GetAuthorUserLoginForDocumentUpload(user);

            var uploadRequest = new UploadDocumentRequest
            {
                BinaryData = ByteString.CopyFrom(documentData.ToArray()),
                Metadata = new DocumentMetadata
                {
                    AuthorUserLogin = authorUserLogin,
                    CaseId = request.CaseId,
                    ContractNumber = caseDetail.Data.ContractNumber,
                    CreatedOn = _dateTime.GetLocalNow().Date,
                    DocumentId = documentId,
                    Description = documentTypeItem.Name,
                    EaCodeMainId = documentTypeItem.EACodeMainId,
                    Filename = $"{documentTypeItem.FileName}_{caseDetail.CaseId}_{_dateTime.GetLocalNow().ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
                }
            };

            await _documentArchiveService.UploadDocument(uploadRequest, cancellationToken);

            // pridat do resonse modelu
            responseModel.CustomersOnSa.Add(new CustomerOnSAItem
            {
                CustomerOnSAId = customerOnSa.CustomerOnSAId,
                BirthDate = customerOnSa.DateOfBirthNaturalPerson,
                FirstName = customerOnSa.FirstNameNaturalPerson,
                LastName = customerOnSa.Name,
                DocumentId = documentId
            });
        }
        
        await _caseService.CancelCase(request.CaseId, true, cancellationToken);
        
        return responseModel;
    }

    private async Task<DocumentTypesResponse.Types.DocumentTypeItem> getDocumentType(DocumentTypes documentType, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        return documentTypes.First(t => t.Id == documentType.ToByte());
    }

    private readonly TimeProvider _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly DocumentGenerator _documentGenerator;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDocumentHelperService _documentHelper;
    private readonly IUserServiceClient _userService;

    public CancelCaseHandler(
        TimeProvider dateTime,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService,
        ICustomerOnSAServiceClient customerOnSaService,
        DocumentGenerator documentGenerator,
        IDocumentArchiveServiceClient documentArchiveService,
        ICurrentUserAccessor currentUserAccessor,
        IDocumentHelperService documentHelper,
        IUserServiceClient userService)
    {
        _dateTime = dateTime;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _customerOnSaService = customerOnSaService;
        _documentGenerator = documentGenerator;
        _documentArchiveService = documentArchiveService;
        _currentUserAccessor = currentUserAccessor;
        _documentHelper = documentHelper;
        _userService = userService;
    }
}