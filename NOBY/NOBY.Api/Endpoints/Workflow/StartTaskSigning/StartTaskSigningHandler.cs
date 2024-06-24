using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using _SaDomain = DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.DocumentOnSA;
using NOBY.Api.Extensions;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed class StartTaskSigningHandler(
    ICodebookServiceClient _codebookService,
    IDocumentOnSAServiceClient _documentOnSaService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService,
    Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization) 
    : IRequestHandler<StartTaskSigningRequest, WorkflowStartTaskSigningResponse>
{
    public async Task<WorkflowStartTaskSigningResponse> Handle(StartTaskSigningRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.CaseId;
        var taskId = request.TaskId;
        var workflowTasks = await _caseService.GetTaskList(caseId, cancellationToken);
        var salesArrangement = await GetSalesArrangementId(caseId, workflowTasks, taskId, cancellationToken);

        // validace prav
        _salesArrangementAuthorization.ValidateDocumentSigningMngBySaType237And246(salesArrangement.SalesArrangementTypeId);

        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookService.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);

        var documentsOnSaListResponse = await _documentOnSaService.GetDocumentsOnSAList(salesArrangement.SalesArrangementId, cancellationToken);
        var documentOnSa = documentsOnSaListResponse.DocumentsOnSA.FirstOrDefault(d => d.TaskId == taskId && d.IsValid);

        if (documentOnSa is not null)
        {
            return Map(documentOnSa, documentTypes, eACodeMains, signatureStates, salesArrangement.SalesArrangementTypeId);
        }

        var workflowTask = workflowTasks.Find(t => t.TaskId == taskId)
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

    private async Task<_SaDomain.SalesArrangement> GetSalesArrangementId(long caseId, List<WorkflowTask> workflowTasks, long taskId, CancellationToken cancellationToken)
    {
        var wfTask = workflowTasks.Single(t => t.TaskId == taskId);

        var process = (await _caseService.GetProcessList(caseId, cancellationToken)).Single(p => p.ProcessId == wfTask.ProcessId);

        var salesArrangementsResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        if (wfTask.ProcessTypeId != 3)
        {
            return await GetSaAccordingToSaCategory(salesArrangementsResponse, cancellationToken);
        }
        else if (process.RefinancingType == (int)RefinancingTypes.MortgageRetention)
        {
            var sa = salesArrangementsResponse.SalesArrangements.SingleOrDefault(s => s.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRetention
                                                                                      && s.State != (int)SalesArrangementStates.Cancelled
                                                                                      && s.State != (int)SalesArrangementStates.Finished);

            return sa ?? await GetSaAccordingToSaCategory(salesArrangementsResponse, cancellationToken);

        }
        else if (process.RefinancingType == (int)RefinancingTypes.MortgageRefixation)
        {
            var sa = salesArrangementsResponse.SalesArrangements.SingleOrDefault(s => s.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRefixation
                                                                                     && s.State != (int)SalesArrangementStates.Cancelled
                                                                                     && s.State != (int)SalesArrangementStates.Finished);

            return sa ?? await GetSaAccordingToSaCategory(salesArrangementsResponse, cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Unsupported combination of processTypeId {wfTask.ProcessTypeId} and RefinancingType {process.RefinancingType}, cannot get SA");
        }
    }

    private async Task<_SaDomain.SalesArrangement> GetSaAccordingToSaCategory(GetSalesArrangementListResponse saList, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes
            .Single(s => s.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest);

        return saList.SalesArrangements.Single(s => s.SalesArrangementTypeId == salesArrangementType.Id);
    }

    private static WorkflowStartTaskSigningResponse Map(
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
                SignatureTypeId = documentOnSa.SignatureTypeId ?? 0,
                EaCodeMainId = documentOnSa.EACodeMainId
            },
              signatureStates),
            EaCodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
            new() { DocumentTypeId = documentOnSa.DocumentTypeId, EACodeMainId = documentOnSa.EACodeMainId },
            documentTypes,
            eACodeMains)
        };

    private static WorkflowStartTaskSigningResponse Map(
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
                SignatureTypeId = signingResponse.DocumentOnSa.SignatureTypeId ?? 0,
                EaCodeMainId = signingResponse.DocumentOnSa.EACodeMainId
            },
              signatureStates),
            EaCodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
             new() { DocumentTypeId = signingResponse.DocumentOnSa.DocumentTypeId, EACodeMainId = signingResponse.DocumentOnSa.EACodeMainId },
            documentTypes,
            eACodeMains)
        };
}