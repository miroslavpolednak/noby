using DomainServices.CaseService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;
using System.Globalization;

namespace NOBY.Api.Endpoints.Workflow.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, long>
{
    public async Task<long> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        // kontrola existence Case
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // validace price exception
        if (request.TaskTypeId == 2)
        {
            await validatePriceException(caseInstance.CaseId, cancellationToken);
        }

        List<string>? documentIds = new();
        var attachments = request.Attachments?
            .Select(t => new Infrastructure.Services.TempFileManager.TempDocumentInformation
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                FileName = t.FileName,
                TempGuid = t.Guid!.Value
            })
            .ToList();

        if (attachments?.Any() ?? false)
        {
            documentIds.AddRange(await _tempFileManager.UploadToArchive(caseInstance.CaseId, caseInstance.Data?.ContractNumber, attachments, cancellationToken));
        }

        var dsRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
        {
            CaseId = caseInstance.CaseId,
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            TaskRequest = request.TaskUserRequest ?? "",
            TaskSubtypeId = request.TaskSubtypeId
        };
        // pokud existuji nahrane prilohy
        if (documentIds.Any())
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
        _tempFileManager.BatchDelete(attachments);

        return result.TaskId;
    }

    private async Task updatePriceExceptionTask(DomainServices.CaseService.Contracts.CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var saId = await _salesArrangementService.GetProductSalesArrangementId(request.CaseId, cancellationToken);
        var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
        {
            throw new NobyValidationException($"OfferId is null for SalesArrangementId={saId}");
        }
        var offerInstance = await _offerService.GetMortgageOfferDetail(saInstance.OfferId.Value, cancellationToken);

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
                FeeId = t.FeeId.ToString(CultureInfo.InvariantCulture)
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
        if ((await _caseService.GetTaskList(caseId, cancellationToken)).Any(t => t.TaskTypeId == 2 && !t.Cancelled))
        {
            throw new CisAuthorizationException("PriceException already exist");
        }
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly Infrastructure.Services.TempFileManager.ITempFileManager _tempFileManager;

    public CreateTaskHandler(
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        Infrastructure.Services.TempFileManager.ITempFileManager tempFileManager)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _caseService = caseService;
        _tempFileManager = tempFileManager;
    }
}
