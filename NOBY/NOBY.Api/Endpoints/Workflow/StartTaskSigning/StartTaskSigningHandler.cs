using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.DocumentOnSA;

namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed class StartTaskSigningHandler : IRequestHandler<StartTaskSigningRequest, StartTaskSigningResponse>
{
    public async Task<StartTaskSigningResponse> Handle(StartTaskSigningRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.CaseId;
        var taskId = request.TaskId;
        var salesArrangementId = await GetSalesArrangementId(caseId, cancellationToken);
        
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);
        
        var documentsOnSaListResponse = await _documentOnSaService.GetDocumentsOnSAList(salesArrangementId, cancellationToken);
        var documentOnSa = documentsOnSaListResponse.DocumentsOnSA.FirstOrDefault(d => d.TaskId == taskId && d.IsValid);
        
        if (documentOnSa is not null)
        {
            return Map(documentOnSa, documentTypes, eACodeMains, signatureStates);
        }
        
        var workflowTasks = await _caseService.GetTaskList(caseId, cancellationToken);
        var workflowTask = workflowTasks.FirstOrDefault(t => t.TaskId == taskId)
            ?? throw new NobyValidationException($"TaskId '{taskId}' is not present in workflow task list.");
        
        var signingRequest = new StartSigningRequest
        {
            CaseId = caseId,
            TaskId = Convert.ToInt32(taskId),
            SalesArrangementId = salesArrangementId,
            SignatureTypeId = workflowTask.SignatureTypeId
        };

        var signingResponse = await _documentOnSaService.StartSigning(signingRequest, cancellationToken);

        return Map(signingResponse, documentTypes, eACodeMains, signatureStates);
    }

    private async Task<int> GetSalesArrangementId(long caseId, CancellationToken cancellationToken)
    {
        var salesArrangementsResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes
            .Single(s => s.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest);

        var salesArrangement = salesArrangementsResponse.SalesArrangements
            .Single(s => s.SalesArrangementTypeId != salesArrangementType.Id);

        return salesArrangement.SalesArrangementId;
    }

    private static StartTaskSigningResponse Map(
        DocumentOnSAToSign documentOnSa,
        List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes,
        List<EaCodesMainResponse.Types.EaCodesMainItem> eACodeMains,
        List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => new()
    {
        DocumentOnSAId = documentOnSa.DocumentOnSAId,
        DocumentTypeId = documentOnSa.DocumentTypeId,
        FormId = documentOnSa.FormId,
        IsSigned = documentOnSa.IsSigned,
        SignatureTypeId = documentOnSa.SignatureTypeId,
        SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                DocumentOnSAId = documentOnSa.DocumentOnSAId,
                EArchivId = documentOnSa.EArchivId,
                IsSigned = documentOnSa.IsSigned
            },
            signatureStates
        ),
        EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
            documentOnSa.DocumentTypeId.GetValueOrDefault(),
            documentTypes,
            eACodeMains)
    };
    
    private static StartTaskSigningResponse Map(
        StartSigningResponse signingResponse,
        List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes,
        List<EaCodesMainResponse.Types.EaCodesMainItem> eACodeMains,
        List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => new()
    {
        DocumentOnSAId = signingResponse.DocumentOnSa.DocumentOnSAId,
        DocumentTypeId = signingResponse.DocumentOnSa.DocumentTypeId,
        FormId = signingResponse.DocumentOnSa.FormId,
        IsSigned = signingResponse.DocumentOnSa.IsSigned,
        SignatureTypeId = signingResponse.DocumentOnSa.SignatureTypeId,
        SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                DocumentOnSAId = signingResponse.DocumentOnSa.DocumentOnSAId,
                EArchivId = signingResponse.DocumentOnSa.EArchivId,
                IsSigned = signingResponse.DocumentOnSa.IsSigned
            },
            signatureStates
        ),
        EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
            signingResponse.DocumentOnSa.DocumentTypeId.GetValueOrDefault(),
            documentTypes,
            eACodeMains)
    };
    
    private readonly ICodebookServiceClient _codebookService;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    
    public StartTaskSigningHandler(
        ICodebookServiceClient codebookService,
        IDocumentOnSAServiceClient documentOnSaService,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService)
    {
        _codebookService = codebookService;
        _documentOnSaService = documentOnSaService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
    }
}