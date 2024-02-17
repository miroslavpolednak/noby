using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.CreateTask;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, long>
{
    public async Task<long> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
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
        await validateProcess(Convert.ToInt32(request.ProcessId), cancellationToken);

        // validace price exception
        if (request.TaskTypeId == 2)
        {
            await validatePriceException(caseInstance.CaseId, cancellationToken);
        }

        List<string>? documentIds = new();
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
        if (request.TaskTypeId == 2)
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

    private async Task validateProcess(int processId, CancellationToken cancellationToken)
    {
        var processInstance = await _caseService.GetTaskDetail(processId, cancellationToken);

        if (processInstance.TaskObject?.ProcessTypeId is not 1 or 2 && processInstance.TaskObject?.TaskTypeId != 3)
        {
            throw new NobyValidationException(90032, "validateProcess");
        }

        WorkflowHelpers.ValidateRefinancing(processInstance.TaskObject?.ProcessTypeId, _currentUserAccessor, UserPermissions.WFL_TASK_DETAIL_OtherManage, UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage);
    }

    private async Task updatePriceExceptionTask(DomainServices.CaseService.Contracts.CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var saId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First().SalesArrangementId;
        var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
        {
            throw new NobyValidationException($"OfferId is null for SalesArrangementId={saId}");
        }
        var offerInstance = await _offerService.GetOfferDetail(saInstance.OfferId.Value, cancellationToken);

        request.PriceException = new()
        {
            ProductTypeId = offerInstance.MortgageOffer.SimulationInputs.ProductTypeId,
            FixedRatePeriod = offerInstance.MortgageOffer.SimulationInputs.FixedRatePeriod.GetValueOrDefault(),
            LoanAmount = Convert.ToInt32(offerInstance.MortgageOffer.SimulationResults.LoanAmount),
            LoanDuration = offerInstance.MortgageOffer.SimulationResults.LoanDuration,
            LoanToValue = Convert.ToInt32(offerInstance.MortgageOffer.SimulationResults.LoanToValue),
            Expiration = ((DateTime?)offerInstance.MortgageOffer.BasicParameters.GuaranteeDateTo ?? DateTime.Now), // nikdo nerekl co delat, pokud datum bude null...
            LoanInterestRate = new()
            {
                LoanInterestRate = offerInstance.MortgageOffer.SimulationResults.LoanInterestRate,
                LoanInterestRateProvided = offerInstance.MortgageOffer.SimulationResults.LoanInterestRateProvided,
                LoanInterestRateAnnouncedType = offerInstance.MortgageOffer.SimulationResults.LoanInterestRateAnnouncedType,
                LoanInterestRateDiscount = offerInstance.MortgageOffer.SimulationInputs.InterestRateDiscount
            }
        };

        if (offerInstance.MortgageOffer.AdditionalSimulationResults.Fees is not null)
        {
            request.PriceException.Fees.AddRange(offerInstance.MortgageOffer.AdditionalSimulationResults.Fees.Select(t => new DomainServices.CaseService.Contracts.PriceExceptionFeesItem
            {
                FinalSum = (decimal?)t.FinalSum ?? 0,
                TariffSum = (decimal?)t.TariffSum ?? 0,
                DiscountPercentage = t.DiscountPercentage,
                FeeId = t.FeeId
            }));
        }

        if (offerInstance.MortgageOffer.AdditionalSimulationResults.MarketingActions is not null)
        {
            request.PriceException.AppliedMarketingActionsCodes.AddRange(
                offerInstance.MortgageOffer.AdditionalSimulationResults.MarketingActions
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
        if ((await _caseService.GetTaskList(caseId, cancellationToken)).Any(t => t.TaskTypeId == 2 && !t.Cancelled))
        {
            throw new NobyValidationException(90032, "ValidatePriceException failed");
        }
    }

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly Services.UploadDocumentToArchive.IUploadDocumentToArchiveService _uploadDocumentToArchive;
    private readonly SharedComponents.Storage.ITempStorage _tempFileManager;

    public CreateTaskHandler(
        ICurrentUserAccessor currentUserAccessor,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        SharedComponents.Storage.ITempStorage tempFileManager,
        Services.UploadDocumentToArchive.IUploadDocumentToArchiveService uploadDocumentToArchive)
    {
        _currentUserAccessor = currentUserAccessor;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _caseService = caseService;
        _tempFileManager = tempFileManager;
        _uploadDocumentToArchive = uploadDocumentToArchive;
    }
}
