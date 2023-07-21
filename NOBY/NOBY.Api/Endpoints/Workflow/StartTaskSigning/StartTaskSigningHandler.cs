using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed class StartTaskSigningHandler : IRequestHandler<StartTaskSigningRequest, StartTaskSigningResponse>
{
    public async Task<StartTaskSigningResponse> Handle(StartTaskSigningRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.CaseId;
        var taskId = request.TaskId;
        var salesArrangementsResponse = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes
            .Single(s => s.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest);

        var salesArrangement = salesArrangementsResponse.SalesArrangements
            .Single(s => s.SalesArrangementTypeId != salesArrangementType.Id);

        var salesArrangementId = salesArrangement.SalesArrangementId;
        var documentsOnSaListResponse = await _documentOnSaService.GetDocumentsOnSAList(salesArrangement.SalesArrangementId, cancellationToken);
        var documentOnSa = documentsOnSaListResponse.DocumentsOnSA.FirstOrDefault(d => d.TaskId == taskId && d.IsValid);
        
        if (documentOnSa is not null)
        {
            // return documentOnSa
            return new StartTaskSigningResponse();
        }
        
        var workflowTasks = await _caseService.GetTaskList(caseId, cancellationToken);
        var workflowTask = workflowTasks.FirstOrDefault(t => t.TaskId == taskId)
            ?? throw new CisNotFoundException(90001, $"TaskId '{taskId}' is not present in workflow task list.");

        var signingRequest = new StartSigningRequest
        {
            CaseId = caseId,
            TaskId = (int)taskId,
            SalesArrangementId = salesArrangementId,
            // SignatureTypeId = workflowTask.SignatureType todo: HH
        };

        var signingResponse = await _documentOnSaService.StartSigning(signingRequest, cancellationToken);
        
        return new StartTaskSigningResponse();
    }

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