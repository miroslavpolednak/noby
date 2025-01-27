﻿using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.CreateTask;

internal sealed class CreateTaskHandler(
    ICurrentUserAccessor _currentUserAccessor,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService,
    SharedComponents.Storage.ITempStorage _tempFileManager,
    Services.UploadDocumentToArchive.IUploadDocumentToArchiveService _uploadDocumentToArchive)
        : IRequestHandler<WorkflowCreateTaskRequest, long>
{
    public async Task<long> Handle(WorkflowCreateTaskRequest request, CancellationToken cancellationToken)
    {
        // kontrola existence Case
        DomainServices.CaseService.Contracts.Case caseInstance;
        try
        {
            caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        }
        catch (CisValidationException ex) when (ex.Errors.Any(x => x.ExceptionCode == "13029"))
        {
            throw new NobyValidationException(90032, "DS error 13029");
        }

        // validace procesu
        await validateProcess(caseInstance.CaseId, request.ProcessId, request.TaskTypeId, cancellationToken);

        // validace price exception
        if (request.TaskTypeId == (int)WorkflowTaskTypes.PriceException)
        {
            await validatePriceException(caseInstance.CaseId, cancellationToken);
        }

        List<string>? documentIds = [];
        var attachments = request
            .Attachments?
            .Select(t => new Services.UploadDocumentToArchive.DocumentMetadata
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                TempFileId = t.Guid!.Value
            })
            .ToList();

        if (attachments?.Any() ?? false)
        {
            var uploadResult = await _uploadDocumentToArchive.Upload(caseInstance.CaseId, caseInstance.Data?.ContractNumber, attachments, cancellationToken);
            documentIds.AddRange(uploadResult);
        }

        var dsRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
        {
            CaseId = caseInstance.CaseId,
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            TaskRequest = request.TaskUserRequest ?? "",
            TaskSubtypeId = request.TaskSubtypeId,
            OrderId = request.OrderId
        };
        // pokud existuji nahrane prilohy
        if (documentIds.Count != 0)
        {
            dsRequest.TaskDocumentsId.AddRange(documentIds);
        }

        // price exception
        if (request.TaskTypeId == (int)WorkflowTaskTypes.PriceException)
        {
            await updatePriceExceptionTask(dsRequest, cancellationToken);
        }

        var result = await _caseService.CreateTask(dsRequest, cancellationToken);

        // smazat prilohy z tempu
        if (attachments?.Any() ?? false)
        {
            await _tempFileManager.Delete(attachments.Select(t => t.TempFileId), cancellationToken);
        }

        return result.TaskId;
    }

    private async Task validateProcess(long caseId, long processId, int taskTypeId, CancellationToken cancellationToken)
    {
        var allProcesses = await _caseService.GetProcessList(caseId, cancellationToken);
        var processInstance = allProcesses.FirstOrDefault(t => t.ProcessId == processId)
            ?? throw new NobyValidationException($"Workflow process {processId} for Case {caseId} not found");
        
        if (!_allowedProcessTypes.Contains(processInstance.ProcessTypeId))
        {
            throw new NobyValidationException(90032, "validateProcess #1");
        }
        else if (processInstance.ProcessTypeId == (int)WorkflowProcesses.Refinancing && taskTypeId != (int)WorkflowTaskTypes.Consultation)
        {
            throw new NobyValidationException(90032, "validateProcess #2");
        }

        WorkflowHelpers.ValidateRefinancing(processInstance.ProcessTypeId, _currentUserAccessor, UserPermissions.WFL_TASK_DETAIL_OtherManage, UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage);
    }

    private async Task updatePriceExceptionTask(DomainServices.CaseService.Contracts.CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var saId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First().SalesArrangementId;
        var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
        {
            throw new NobyValidationException($"OfferId is null for SalesArrangementId={saId}");
        }
        var offerInstance = await _offerService.GetMortgageDetail(saInstance.OfferId.Value, cancellationToken);

        request.PriceException = new()
        {
            ProductTypeId = offerInstance.SimulationInputs.ProductTypeId,
            FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod.GetValueOrDefault(),
            LoanAmount = Convert.ToInt32(offerInstance.SimulationResults.LoanAmount),
            LoanDuration = offerInstance.SimulationResults.LoanDuration,
            LoanToValue = Convert.ToInt32(offerInstance.SimulationResults.LoanToValue),
            Expiration = ((DateTime?)offerInstance.BasicParameters.GuaranteeDateTo ?? DateTime.Now), // nikdo nerekl co delat, pokud datum bude null...
            LoanInterestRate = new()
            {
                LoanInterestRate = offerInstance.SimulationResults.LoanInterestRate,
                LoanInterestRateProvided = offerInstance.SimulationResults.LoanInterestRateProvided,
                LoanInterestRateAnnouncedType = offerInstance.SimulationResults.LoanInterestRateAnnouncedType,
                LoanInterestRateDiscount = offerInstance.SimulationInputs.InterestRateDiscount
            }
        };

        if (offerInstance.AdditionalSimulationResults.Fees is not null)
        {
            request.PriceException.Fees.AddRange(offerInstance.AdditionalSimulationResults.Fees.Select(t => new DomainServices.CaseService.Contracts.PriceExceptionFeesItem
            {
                FinalSum = (decimal?)t.FinalSum ?? 0,
                TariffSum = (decimal?)t.TariffSum ?? 0,
                DiscountPercentage = t.DiscountPercentage,
                FeeId = t.FeeId
            }));
        }

        if (offerInstance.AdditionalSimulationResults.MarketingActions is not null)
        {
            request.PriceException.AppliedMarketingActionsCodes.AddRange(
                offerInstance.AdditionalSimulationResults.MarketingActions
                    .Where(t => t.Applied.GetValueOrDefault() == 1)
                    .Select(t => t.Code)
            );
        }
    }

    /// <summary>
    /// Validace zda jiz neexistuje aktivni price exception
    /// </summary>
    private async Task validatePriceException(long caseId, CancellationToken cancellationToken)
    {
        if ((await _caseService.GetTaskList(caseId, cancellationToken)).Any(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled))
        {
            throw new NobyValidationException(90032, "ValidatePriceException failed");
        }
    }

    private static readonly int[] _allowedProcessTypes =
    [
        (int)WorkflowProcesses.Main,
        (int)WorkflowProcesses.Change,
        (int)WorkflowProcesses.Refinancing
    ];
}
