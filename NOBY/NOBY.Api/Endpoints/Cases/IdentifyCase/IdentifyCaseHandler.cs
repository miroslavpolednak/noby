using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using PaymentAccount = NOBY.Api.Endpoints.Cases.IdentifyCase.Dto.PaymentAccount;

namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

internal sealed class IdentifyCaseHandler : IRequestHandler<IdentifyCaseRequest, IdentifyCaseResponse>
{
    public async Task<IdentifyCaseResponse> Handle(IdentifyCaseRequest request, CancellationToken cancellationToken)
    {
        return request.Criterion switch
        {
            Criterion.FormId => await HandleByFormId(request.FormId!, cancellationToken),
            Criterion.PaymentAccount => await HandleByPaymentAccount(request.Account!, cancellationToken),
            Criterion.CaseId => await HandleByCaseId(request.CaseId!.Value, cancellationToken),
            Criterion.ContractNumber => await HandleByContractNumber(request.ContractNumber!, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private async Task<IdentifyCaseResponse> HandleByFormId(string formId, CancellationToken cancellationToken)
    {
        var documentListRequest = new GetDocumentListRequest { FormId = formId };
        var documentListResponse = await _documentArchiveServiceClient.GetDocumentList(documentListRequest, cancellationToken);

        var document = documentListResponse.Metadata.FirstOrDefault();
        var caseId = document?.CaseId;

        if (caseId == null)
        {
            return new IdentifyCaseResponse();
        }

        if (!await _caseServiceClient.ValidateCaseId(caseId.Value, false, cancellationToken))
        {
            return new IdentifyCaseResponse { CaseId = null };
        }

        var taskList = await _caseServiceClient.GetTaskList(caseId.Value, cancellationToken);
        var taskSubList = taskList.Where(t => t.TaskTypeId == 6).ToList();

        if (!taskSubList.Any())
        {
            return new IdentifyCaseResponse { CaseId = caseId };
        }

        var taskDetails = new Dictionary<long, List<TaskDetailItem>>();
        
        foreach (var task in taskSubList)
        {
            var response = await _caseServiceClient.GetTaskDetail(task.TaskIdSb, cancellationToken);

            if (response.TaskDetail?.Signing?.FormId == formId)
            {
                taskDetails.Add(task.TaskId, new() { response.TaskDetail });
            }
        }

        if (taskDetails.SelectMany(d => d.Value).Count() != 1)
        {
            return new IdentifyCaseResponse { CaseId = caseId };
        }

        var taskId = taskDetails.First().Key;
        var taskDetailRequest = new Workflow.GetTaskDetail.GetTaskDetailRequest(caseId.Value, taskId);
        var taskDetailResponse = await _mediator.Send(taskDetailRequest, cancellationToken);

        return new IdentifyCaseResponse
        {
            Task = taskDetailResponse.Task,
            TaskDetail = taskDetailResponse.TaskDetail,
            Documents = taskDetailResponse.Documents
        };
    }
    
    private async Task<IdentifyCaseResponse> HandleByPaymentAccount(PaymentAccount account, CancellationToken cancellationToken)
    {
        var paymentAccount = new PaymentAccountObject { Prefix = account.Prefix, AccountNumber = account.Number };
        var request = new GetCaseIdRequest { PaymentAccount = paymentAccount };
        
        try
        {
            var response = await _productServiceClient.GetCaseId(request, cancellationToken);
            return await HandleByCaseId(response.CaseId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            return new IdentifyCaseResponse();
        }
    }
    
    private async Task<IdentifyCaseResponse> HandleByCaseId(long caseId, CancellationToken cancellationToken)
    {
        var found = await _caseServiceClient.ValidateCaseId(caseId, false, cancellationToken);
        return new IdentifyCaseResponse { CaseId = found ? caseId : null };
    }
    
    private async Task<IdentifyCaseResponse> HandleByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
        var contract = new ContractNumberObject { ContractNumber = contractNumber };
        var request = new GetCaseIdRequest { ContractNumber = contract };
        try
        {
            var response = await _productServiceClient.GetCaseId(request, cancellationToken);
            return await HandleByCaseId(response.CaseId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            return new IdentifyCaseResponse();
        }
    }

    private readonly IMediator _mediator;
    private readonly IProductServiceClient _productServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    
    public IdentifyCaseHandler(
        IMediator mediator,
        IProductServiceClient productServiceClient,
        ICaseServiceClient caseServiceClient,
        IDocumentArchiveServiceClient documentArchiveServiceClient)
    {
        _mediator = mediator;
        _productServiceClient = productServiceClient;
        _caseServiceClient = caseServiceClient;
        _documentArchiveServiceClient = documentArchiveServiceClient;
    }
}