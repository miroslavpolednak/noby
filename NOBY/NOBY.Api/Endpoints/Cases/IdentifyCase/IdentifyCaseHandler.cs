using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using SharedTypes.GrpcTypes;
using System.Text.RegularExpressions;

namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

internal sealed partial class IdentifyCaseHandler(
    Services.CreateCaseFromExternalSources.CreateCaseFromExternalSourcesService _createCaseFromExternalSources,
    ICurrentUserAccessor _currentUser,
    IMediator _mediator,
    IProductServiceClient _productServiceClient,
    ICaseServiceClient _caseServiceClient,
    IDocumentArchiveServiceClient _documentArchiveServiceClient,
    IDocumentOnSAServiceClient _documentOnSAService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService) 
    : IRequestHandler<CasesIdentifyCaseRequest, CasesIdentifyCaseResponse>
{
    public async Task<CasesIdentifyCaseResponse> Handle(CasesIdentifyCaseRequest request, CancellationToken cancellationToken)
    {
        return request.Criterion switch
        {
            CasesIdentifyCaseRequestCriterion.FormId => await handleByFormId(request.FormId!.Trim(), cancellationToken),
            CasesIdentifyCaseRequestCriterion.PaymentAccount => await handleByPaymentAccount(request.Account!, cancellationToken),
            CasesIdentifyCaseRequestCriterion.CaseId => await handleByCaseId(request.CaseId!.Value, cancellationToken),
            CasesIdentifyCaseRequestCriterion.ContractNumber => await handleByContractNumber(request.ContractNumber!.Trim(), cancellationToken),
            CasesIdentifyCaseRequestCriterion.CustomerIdentity => await handleByCustomerIdentity(request.CustomerIdentity!, cancellationToken),
            _ => throw new NobyValidationException("Criterion unknown")
        };
    }

    private async Task<CasesIdentifyCaseResponse> handleByCustomerIdentity(Identity? identity, CancellationToken cancellationToken)
    {
        var result = await _productServiceClient.SearchProducts(identity, cancellationToken);
        List<long> idsToRemove = [];

        // projit a dozalozit pripadne chybejici produkty
        foreach (var item in result)
        {
            try
            {
                await handleByCaseId(item.CaseId, cancellationToken);
            }
            // vime ze to v nekterych pripadech muze spadnout na chybu (vetsinou na pravech) - takovy case pak nezobrazime uzivateli
            catch (CisAuthorizationException)
            {
                idsToRemove.Add(item.CaseId);
            }
            catch (NobyValidationException ex) when (ex.Errors[0].ErrorCode == 90032)
            {
                idsToRemove.Add(item.CaseId);
            }
        }

        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var caseStates = await _codebookService.CaseStates(cancellationToken);

        CasesIdentifyCaseResponse response = new() { Cases = new(result.Count - idsToRemove.Count) };

        foreach (var product in result.Where(t => !idsToRemove.Contains(t.CaseId)))
        {
            var caseInstance = await _caseServiceClient.GetCaseDetail(product.CaseId, cancellationToken);

            response.Cases.Add(new CasesIdentifyCaseResponseItem
            {
                CaseId = product.CaseId,
                ContractRelationshipTypeId = product.ContractRelationshipTypeId,
                ContractNumber = caseInstance.Data.ContractNumber,
                State = (EnumCaseStates)caseInstance.State,
                TargetAmount = caseInstance.Data.TargetAmount,
                StateName = caseStates.First(x => x.Id == caseInstance.State).Name,
                ProductName = productTypes.First(x => x.Id == caseInstance.Data.ProductTypeId).Name,
                CaseOwnerName = caseInstance.CaseOwner.UserName,
                CreatedOn = caseInstance.Created.DateTime,
                StateUpdatedOn = caseInstance.StateUpdatedOn,
                Customer = new()
                {
                    FirstName = caseInstance.Customer.FirstNameNaturalPerson,
                    LastName = caseInstance.Customer.Name,
                    DateOfBirth = caseInstance.Customer.DateOfBirthNaturalPerson,
                    Identity = new SharedTypesCustomerIdentity
                    {
                        Scheme = (SharedTypesCustomerIdentityScheme)caseInstance.Customer.Identity.IdentityScheme,
                        Id = caseInstance.Customer.Identity.IdentityId
                    }
                },
                ActiveTasks = caseInstance.Tasks?
                    .GroupBy(t => t.TaskTypeId)
                    .Select(t => new CasesIdentifyCaseResponseItemTask
                    {
                        CategoryId = t.Key,
                        TaskCount = t.Count()
                    })
                    .ToList()
            });
        }

        return response;
    }

    private async Task<CasesIdentifyCaseResponse> handleByFormId(string formId, CancellationToken cancellationToken)
    {
        var documentListRequest = new GetDocumentListRequest { FormId = formId };
        var documentListResponse = await _documentArchiveServiceClient.GetDocumentList(documentListRequest, cancellationToken);

        var document = documentListResponse
            .Metadata
            .Where(t => handlerByFormIdRegex().IsMatch(t.DocumentId[7..]))
            .FirstOrDefault();
        var caseId = document?.CaseId;

        if (caseId == null)
        {
            try
            {
                var documentOnSa = (await _documentOnSAService.GetDocumentOnSAByFormId(formId, cancellationToken)).DocumentOnSa;

                var validationResponse = await _salesArrangementService.ValidateSalesArrangementId(documentOnSa.SalesArrangementId, false, cancellationToken);

                if (!validationResponse.Exists || validationResponse.CaseId is null)
                    return new CasesIdentifyCaseResponse();

                return await handleByCaseId(validationResponse.CaseId.Value, cancellationToken);
            }
            catch (CisNotFoundException)
            {
                return new CasesIdentifyCaseResponse();
            }
        }

        var caseInstance = await _caseServiceClient.ValidateCaseId(caseId.Value, false, cancellationToken);
        if (!caseInstance.Exists)
        {
            await _createCaseFromExternalSources.CreateCase(caseId.Value, cancellationToken);
        }
        else
        {
            SecurityHelpers.CheckCaseOwnerAndState(_currentUser, caseInstance.OwnerUserId!.Value, (EnumCaseStates)caseInstance.State!.Value, caseInstance.StateUpdatedOn);
        }

        var taskList = await _caseServiceClient.GetTaskList(caseId.Value, cancellationToken);
        var taskSubList = taskList.Where(t => t.TaskTypeId == 6).ToList();

        if (taskSubList.Count == 0)
        {
            return new CasesIdentifyCaseResponse 
            { 
                Cases = 
                [ 
                    new() { CaseId = caseId.Value } 
                ] 
            };
        }

        var taskDetails = new Dictionary<long, List<TaskDetailItem>>();
        
        foreach (var task in taskSubList)
        {
            var response = await _caseServiceClient.GetTaskDetail(task.TaskIdSb, cancellationToken);

            if (response.TaskDetail?.Signing?.FormId == formId)
            {
                taskDetails.Add(task.TaskId, [response.TaskDetail]);
            }
        }

        if (taskDetails.SelectMany(d => d.Value).Count() != 1)
        {
            return new CasesIdentifyCaseResponse
            {
                Cases =
                [
                    new() { CaseId = caseId.Value }
                ]
            };
        }

        var taskId = taskDetails.First().Key;
        var taskDetailRequest = new Workflow.GetTaskDetail.GetTaskDetailRequest(caseId.Value, taskId);
        var taskDetailResponse = await _mediator.Send(taskDetailRequest, cancellationToken);

        return new CasesIdentifyCaseResponse
        {
            Cases =
            [
                new() { CaseId = caseId.Value }
            ],
            Task = taskDetailResponse.Task,
            TaskDetail = taskDetailResponse.TaskDetail,
            Documents = taskDetailResponse.Documents
        };
    }
    
    private async Task<CasesIdentifyCaseResponse> handleByPaymentAccount(CasesIdentifyCasePaymentAccount account, CancellationToken cancellationToken)
    {
        var request = new GetCaseIdRequest 
        { 
            PaymentAccount = new()
            { 
                Prefix = account.Prefix?.Trim(), 
                AccountNumber = account.Number.Trim() 
            }
        };
        return await callProductService(request, cancellationToken);
    }
    
    private async Task<CasesIdentifyCaseResponse> handleByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
        var request = new GetCaseIdRequest 
        { 
            ContractNumber = new()
            {
                ContractNumber = contractNumber
            }
        };
        return await callProductService(request, cancellationToken);
    }

    private async Task<CasesIdentifyCaseResponse> handleByCaseId(long caseId, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseServiceClient.ValidateCaseId(caseId, false, cancellationToken);

        if (!caseInstance.Exists)
        {
            // osetrena vyjimka - spoustime logiku na vytvoreni case z konsDB
            try
            {
                await _createCaseFromExternalSources.CreateCase(caseId, cancellationToken);
            }
            catch (CisNotFoundException)
            {
                return new CasesIdentifyCaseResponse();
            }
        }
        else
        {
            SecurityHelpers.CheckCaseOwnerAndState(_currentUser, caseInstance.OwnerUserId!.Value, (EnumCaseStates)caseInstance.State!.Value, caseInstance.StateUpdatedOn);
        }

        return new CasesIdentifyCaseResponse
        {
            Cases =
                [
                    new() { CaseId = caseId }
                ]
        };
    }

    private async Task<CasesIdentifyCaseResponse> callProductService(GetCaseIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _productServiceClient.GetCaseId(request, cancellationToken);
            return await handleByCaseId(response.CaseId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            return new CasesIdentifyCaseResponse();
        }
    }

    [GeneratedRegex(@"^[0-9]+$")]
    private static partial Regex handlerByFormIdRegex();
}