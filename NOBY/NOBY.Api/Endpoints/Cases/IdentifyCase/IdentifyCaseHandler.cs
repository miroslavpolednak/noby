using DomainServices.CaseService.Clients;
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
        var request = new GetDocumentListRequest { }; // todo
        var response = await _documentArchiveServiceClient.GetDocumentList(request, cancellationToken);

        var document = response.Metadata.FirstOrDefault(m => m.FormId == formId);
        var caseId = document?.CaseId ?? 0;
        
        await _caseServiceClient.ValidateCaseId(caseId, true, cancellationToken);

        var taskList = await _caseServiceClient.GetTaskList(caseId, cancellationToken);
        var taskSubList = taskList.Where(t => t.TaskTypeId == 6).ToList();
        
        foreach (var task in taskSubList)
        {
            var taskDetailResponse = await _caseServiceClient.GetTaskDetail(task.TaskIdSb, cancellationToken);
            
            if (taskDetailResponse.TaskDetails.Any(d => d.TaskDetail.Signing.FormId == formId))
            {
                // todo:
            }
        }

        return new IdentifyCaseResponse { CaseId = caseId };
    }
    
    private async Task<IdentifyCaseResponse> HandleByPaymentAccount(PaymentAccount account, CancellationToken cancellationToken)
    {
        var paymentAccount = new PaymentAccountObject { Prefix = account.Prefix, AccountNumber = account.Number };
        var request = new GetCaseIdRequest { PaymentAccount = paymentAccount };
        var response = await _productServiceClient.GetCaseId(request, cancellationToken);
        
        return await HandleByCaseId(response.CaseId, cancellationToken);
    }
    
    private async Task<IdentifyCaseResponse> HandleByCaseId(long caseId, CancellationToken cancellationToken)
    {
        await _caseServiceClient.ValidateCaseId(caseId, true, cancellationToken);
        return new IdentifyCaseResponse { CaseId = caseId };
    }
    
    private async Task<IdentifyCaseResponse> HandleByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
        var contract = new ContractNumberObject { ContractNumber = contractNumber };
        var request = new GetCaseIdRequest { ContractNumber = contract };
        var response = await _productServiceClient.GetCaseId(request, cancellationToken);

        return await HandleByCaseId(response.CaseId, cancellationToken);
    }

    private readonly IProductServiceClient _productServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    
    public IdentifyCaseHandler(
        IProductServiceClient productServiceClient,
        ICaseServiceClient caseServiceClient,
        IDocumentArchiveServiceClient documentArchiveServiceClient)
    {
        _productServiceClient = productServiceClient;
        _caseServiceClient = caseServiceClient;
        _documentArchiveServiceClient = documentArchiveServiceClient;
    }
}