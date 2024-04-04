using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.InterestRatesValidFrom;
using NOBY.Services.WorkflowTask;
using _Ca = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Offer.LinkModelation;

internal sealed class LinkModelationHandler
    : IRequestHandler<LinkModelationRequest>
{
    private static readonly SalesArrangementStates[] _allowedStates = [
        SalesArrangementStates.InProgress,
        SalesArrangementStates.NewArrangement,
        SalesArrangementStates.InSigning,
        SalesArrangementStates.ToSend
    ];

    public async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        // get SA data
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);
        
        // validace prav
        _salesArrangementAuthorization.ValidateSaAccessBySaType213And248(saInstance.SalesArrangementTypeId);
        
        if ((offer.Data.CaseId.HasValue && saInstance.CaseId != offer.Data.CaseId) || !_allowedStates.Contains((SalesArrangementStates)saInstance.State))
            throw new NobyValidationException(90032);
        
        switch ((SalesArrangementTypes)saInstance.SalesArrangementTypeId)
        {
            case SalesArrangementTypes.Mortgage:
                await updateMortgage(request, saInstance, cancellationToken);
                break;

            case SalesArrangementTypes.MortgageRefixation:
                ValidateRefixation(saInstance);
                await UpdateRefinancing(request, saInstance, offer, cancellationToken);
                break;

            case SalesArrangementTypes.MortgageRetention:
                await ValidateRetention(saInstance, offer, cancellationToken);
                await UpdateRefinancing(request, saInstance, offer, cancellationToken);
                break;

            default:
                throw new NobyValidationException(90032);
        }

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
    }

    private async Task updateMortgage(LinkModelationRequest request, DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        // get case instance
        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);

        // update kontaktu
        var offerContacts = new _Ca.OfferContacts
        {
            EmailForOffer = request.OfferContacts?.EmailAddress?.EmailAddress ?? "",
            PhoneNumberForOffer = new _Ca.Phone
            {
                PhoneNumber = request.OfferContacts?.MobilePhone?.PhoneNumber ?? "",
                PhoneIDC = request.OfferContacts?.MobilePhone?.PhoneIDC ?? ""
            }
        };
        await _caseService.UpdateOfferContacts(saInstance.CaseId, offerContacts, cancellationToken);

        // update customer
        if (caseInstance.Customer?.Identity is null || caseInstance.Customer.Identity.IdentityId == 0)
        {
            await _caseService.UpdateCustomerData(saInstance.CaseId, new _Ca.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName ?? "",
                Name = request.LastName ?? "",
            }, cancellationToken);
        }
    }

    private async Task UpdateRefinancing(LinkModelationRequest request, DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        var taskResult = await _caseService.GetTaskByTaskId(salesArrangement.CaseId, salesArrangement.TaskProcessId!.Value, cancellationToken);

        var taskUpdateRequest = new _Ca.UpdateTaskRequest
        {
            CaseId = salesArrangement.CaseId,
            TaskIdSb = taskResult.TaskIdSb,
            Retention = new _Ca.Retention
            {
                InterestRateValidFrom = (DateTime)offer.MortgageRetention.SimulationInputs.InterestRateValidFrom,
                LoanInterestRate = offer.MortgageRetention.SimulationInputs.InterestRate,
                LoanInterestRateProvided = ((decimal?)offer.MortgageRetention.SimulationInputs.InterestRate?? 0) - ((decimal?)offer.MortgageRetention.SimulationInputs.InterestRateDiscount ?? 0),
                LoanPaymentAmount = (decimal)offer.MortgageRetention.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountFinal = (decimal?)offer.MortgageRetention.SimulationResults.LoanPaymentAmountDiscounted,
            }
        };

        if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRetention)
        {
            taskUpdateRequest.Retention.FeeSum = (decimal)offer.MortgageRetention.BasicParameters.FeeAmount;
            taskUpdateRequest.Retention.FeeFinalSum = (decimal?)offer.MortgageRetention.BasicParameters.FeeAmountDiscounted;
        } 
        else if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRefixation)
        {
            //taskUpdateRequest.Retention.FixedRatePeriod = //Offer FixedRatePeriod
        }

        await _caseService.UpdateTask(taskUpdateRequest, cancellationToken);

        var tasks = await _caseService.GetTaskList(salesArrangement.CaseId, cancellationToken);

        if (tasks.Where(t => t.ProcessId != salesArrangement.TaskProcessId && t.TaskTypeId != (int)WorkflowTaskTypes.PriceException).Any(t => !t.Cancelled))
        {
            var oldOffer = await _offerService.GetOffer(salesArrangement.OfferId!.Value, cancellationToken);

            if ((decimal?)offer.MortgageRetention.SimulationInputs.InterestRateDiscount == oldOffer.MortgageRetention.SimulationInputs.InterestRateDiscount &&
                (decimal?)offer.MortgageRetention.BasicParameters.FeeAmountDiscounted == oldOffer.MortgageRetention.BasicParameters.FeeAmountDiscounted)
                return;

            _salesArrangementAuthorization.ValidateRefinancing241Permission();

            await _caseService.CancelTask(salesArrangement.CaseId, taskResult.TaskIdSb, cancellationToken);
        }

        if (offer.MortgageRetention.SimulationInputs.InterestRateDiscount is null || offer.MortgageRetention.BasicParameters.FeeAmountDiscounted is null)
            return;

        _salesArrangementAuthorization.ValidateRefinancing241Permission();

        salesArrangement.Retention.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        await _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Retention = salesArrangement.Retention
        }, cancellationToken);

        var createTaskRequest = new _Ca.CreateTaskRequest
        {
            CaseId = salesArrangement.CaseId,
            TaskTypeId = (int)WorkflowTaskTypes.PriceException,
            ProcessId = salesArrangement.TaskProcessId,
            TaskRequest = salesArrangement.Retention?.IndividualPriceCommentLastVersion,
            PriceException = new _Ca.TaskPriceException
            {
                LoanInterestRate = new _Ca.PriceExceptionLoanInterestRateItem
                {
                    LoanInterestRate = (decimal?)offer.MortgageRetention.SimulationInputs.InterestRate ?? 0m,
                    LoanInterestRateDiscount = offer.MortgageRetention.SimulationInputs.InterestRateDiscount,
                    LoanInterestRateProvided = ((decimal?)offer.MortgageRetention.SimulationInputs.InterestRate ?? 0m) - (offer.MortgageRetention.SimulationInputs.InterestRateDiscount ?? 0m)
                },
                Fees =
                {
                    new _Ca.PriceExceptionFeesItem
                    {
                        TariffSum = offer.MortgageRetention.BasicParameters.FeeAmount,
                        FinalSum = (decimal?)offer.MortgageRetention.BasicParameters.FeeAmountDiscounted ?? 0m,
                        DiscountPercentage = 100 * (offer.MortgageRetention.BasicParameters.FeeAmount - (offer.MortgageRetention.BasicParameters.FeeAmountDiscounted ?? 0m)) / offer.MortgageRetention.BasicParameters.FeeAmount
                    }
                }
            }
        };

        await _caseService.CreateTask(createTaskRequest, cancellationToken);
    }

    private static void ValidateRefixation(DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement)
    {
        if (salesArrangement.Refixation.ManagedByRC2 == true)
            throw new NobyValidationException(90032);
    }

    private async Task ValidateRetention(DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        if (salesArrangement.Retention.ManagedByRC2 == true)
            throw new NobyValidationException(90032);

        var interestRateValidFrom = (DateTime)offer.MortgageRetention.SimulationInputs.InterestRateValidFrom;
        var (date1, date2) = await _interestRatesValidFromService.GetValidityDates(salesArrangement.CaseId, cancellationToken);

        if (date1 == interestRateValidFrom || date2 == interestRateValidFrom)
            return;

        throw new NobyValidationException(90032);
    }

    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly IWorkflowTaskService _workflowTaskService;
    private readonly InterestRatesValidFromService _interestRatesValidFromService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService;

    public LinkModelationHandler(
        DomainServices.CaseService.Clients.v1.ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization,
        IWorkflowTaskService workflowTaskService,
        InterestRatesValidFromService interestRatesValidFromService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _salesArrangementAuthorization = salesArrangementAuthorization;
        _workflowTaskService = workflowTaskService;
        _interestRatesValidFromService = interestRatesValidFromService;
    }
}
