﻿using System.Globalization;
using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
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
using Google.Protobuf;
using NOBY.Api.Endpoints.Document.GeneralDocument;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

internal sealed class CancelCaseHandler : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    const DocumentTypes _documentType = DocumentTypes.ODSTOUP;
    
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellationToken)
    {
        var documentTypeItem = await GetDocumentType(_documentType, cancellationToken);
        
        var salesArrangement = await GetProductSalesArrangement(request.CaseId, cancellationToken);
        var caseDetail = await _caseService.GetCaseDetail(salesArrangement.CaseId, cancellationToken);
        var customerOnSas = await _customerOnSaService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        
        foreach (var customerOnSa in customerOnSas)
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
            
            var uploadRequest = new UploadDocumentRequest
            {
                BinaryData = ByteString.CopyFrom(documentData.ToArray()),
                Metadata = new DocumentMetadata
                {
                    AuthorUserLogin = _currentUserAccessor.User?.Login,
                    CaseId = salesArrangement.CaseId,
                    ContractNumber = caseDetail.Data.ContractNumber,
                    CreatedOn = _dateTime.Now.Date,
                    DocumentId = documentId,
                    Description = documentTypeItem.Name,
                    EaCodeMainId = documentTypeItem.EACodeMainId,
                    Filename = $"{documentTypeItem.FileName}_{caseDetail.CaseId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
                }
            };

            await _documentArchiveService.UploadDocument(uploadRequest, cancellationToken);
        }

        await _caseService.CancelCase(request.CaseId, cancellationToken: cancellationToken);
        return await CreateCancelCaseResponse(salesArrangement.CaseId, customerOnSas, cancellationToken);
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

    private async Task<CancelCaseResponse> CreateCancelCaseResponse(long caseId, List<CustomerOnSA> customerOnSas, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);
        var caseStates = await _codebookService.CaseStates(cancellationToken);
        var caseState = caseStates.First(s => s.Id == caseDetail.State);
        
        return new CancelCaseResponse
        {
            State = (CaseStates) caseState.Id,
            StateName = caseState.Name,
            CustomersOnSa = customerOnSas.Select(c => new CustomerOnSAItem
            {
                CustomerOnSAId = c.CustomerOnSAId,
                BirthDate = c.DateOfBirthNaturalPerson,
                FirstName = c.FirstNameNaturalPerson,
                LastName = c.Name
            }).ToList()
        };
    }
    
    private readonly IDateTime _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly DocumentGenerator _documentGenerator;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CancelCaseHandler(
        IDateTime dateTime,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService,
        ICustomerOnSAServiceClient customerOnSaService,
        DocumentGenerator documentGenerator,
        IDocumentArchiveServiceClient documentArchiveService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _dateTime = dateTime;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _customerOnSaService = customerOnSaService;
        _documentGenerator = documentGenerator;
        _documentArchiveService = documentArchiveService;
        _currentUserAccessor = currentUserAccessor;
    }
}