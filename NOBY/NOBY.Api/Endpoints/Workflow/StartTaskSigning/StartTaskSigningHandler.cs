using SharedTypes.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using _SaDomain = DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.DocumentOnSA;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed class StartTaskSigningHandler : IRequestHandler<StartTaskSigningRequest, StartTaskSigningResponse>
{
    public async Task<StartTaskSigningResponse> Handle(StartTaskSigningRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.CaseId;
        var taskId = request.TaskId;
        var salesArrangement = await GetSalesArrangementId(caseId, cancellationToken);

        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);

        var documentsOnSaListResponse = await _documentOnSaService.GetDocumentsOnSAList(salesArrangement.SalesArrangementId, cancellationToken);
        var documentOnSa = documentsOnSaListResponse.DocumentsOnSA.FirstOrDefault(d => d.TaskId == taskId && d.IsValid);

        if (documentOnSa is not null)
        {
            return Map(documentOnSa, documentTypes, eACodeMains, signatureStates, salesArrangement.SalesArrangementTypeId);
        }

        var workflowTasks = await _caseService.GetTaskList(caseId, cancellationToken);
        var workflowTask = workflowTasks.FirstOrDefault(t => t.TaskId == taskId)
            ?? throw new NobyValidationException($"TaskId '{taskId}' is not present in workflow task list.");

        var signingRequest = new StartSigningRequest
        {
            CaseId = caseId,
            TaskId = Convert.ToInt32(taskId),
            TaskIdSb = workflowTask.TaskIdSb,
            SalesArrangementId = salesArrangement.SalesArrangementId,
            SignatureTypeId = workflowTask.SignatureTypeId
        };

        var signingResponse = await _documentOnSaService.StartSigning(signingRequest, cancellationToken);

        return Map(signingResponse, documentTypes, eACodeMains, signatureStates, salesArrangement.SalesArrangementTypeId);
    }

    private async Task<_SaDomain.SalesArrangement> GetSalesArrangementId(long caseId, CancellationToken cancellationToken)
    {
        var salesArrangementsResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes
            .Single(s => s.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest);

        var salesArrangement = salesArrangementsResponse.SalesArrangements
            .Single(s => s.SalesArrangementTypeId == salesArrangementType.Id);

        return salesArrangement;
    }

    private static StartTaskSigningResponse Map(
        DocumentOnSAToSign documentOnSa,
        List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes,
        List<EaCodesMainResponse.Types.EaCodesMainItem> eACodeMains,
        List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates,
        int salesArrangementTypeId) => new()
        {
            DocumentOnSAId = documentOnSa.DocumentOnSAId,
            DocumentTypeId = documentOnSa.DocumentTypeId,
            FormId = documentOnSa.FormId,
            IsSigned = documentOnSa.IsSigned,
            SignatureTypeId = documentOnSa.SignatureTypeId,
            SalesArrangementId = documentOnSa.SalesArrangementId,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                IsValid = documentOnSa.IsValid,
                DocumentOnSAId = documentOnSa.DocumentOnSAId,
                IsSigned = documentOnSa.IsSigned,
                Source = documentOnSa.Source.MapToCisEnum(),
                SalesArrangementTypeId = salesArrangementTypeId,
                EArchivIdsLinked = documentOnSa.EArchivIdsLinked,
            },
              signatureStates),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
            new() { DocumentTypeId = documentOnSa.DocumentTypeId, EACodeMainId = documentOnSa.EACodeMainId },
            documentTypes,
            eACodeMains)
        };

    private static StartTaskSigningResponse Map(
        StartSigningResponse signingResponse,
        List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes,
        List<EaCodesMainResponse.Types.EaCodesMainItem> eACodeMains,
        List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates,
        int salesArrangementTypeId) => new()
        {
            DocumentOnSAId = signingResponse.DocumentOnSa.DocumentOnSAId,
            DocumentTypeId = signingResponse.DocumentOnSa.DocumentTypeId,
            FormId = signingResponse.DocumentOnSa.FormId,
            IsSigned = signingResponse.DocumentOnSa.IsSigned,
            SignatureTypeId = signingResponse.DocumentOnSa.SignatureTypeId,
            SalesArrangementId = signingResponse.DocumentOnSa.SalesArrangementId,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                IsValid = signingResponse.DocumentOnSa.IsValid,
                DocumentOnSAId = signingResponse.DocumentOnSa.DocumentOnSAId,
                IsSigned = signingResponse.DocumentOnSa.IsSigned,
                Source = signingResponse.DocumentOnSa.Source.MapToCisEnum(),
                SalesArrangementTypeId = salesArrangementTypeId,
                EArchivIdsLinked = Array.Empty<string>(),
            },
              signatureStates),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
             new() { DocumentTypeId = signingResponse.DocumentOnSa.DocumentTypeId, EACodeMainId = signingResponse.DocumentOnSa.EACodeMainId },
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