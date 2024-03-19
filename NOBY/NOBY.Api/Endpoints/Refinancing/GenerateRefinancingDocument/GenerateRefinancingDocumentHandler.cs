using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;
using ExternalServices.SbWebApi.V1;
using _contract = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefinancingDocument;

public class GenerateRefinancingDocumentHandler : IRequestHandler<GenerateRefinancingDocumentRequest>
{
    private readonly TimeProvider _time;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICaseServiceClient _caseService;
    private readonly IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISbWebApiClient _sbWebApi;

    public GenerateRefinancingDocumentHandler(
        TimeProvider time,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ICaseServiceClient caseService,
        IUserServiceClient userService,
        ICurrentUserAccessor currentUser,
        ICodebookServiceClient codebookService,
        ISbWebApiClient sbWebApi
        )
    {
        _time = time;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _caseService = caseService;
        _userService = userService;
        _currentUser = currentUser;
        _codebookService = codebookService;
        _sbWebApi = sbWebApi;
    }

    public async Task Handle(GenerateRefinancingDocumentRequest request, CancellationToken cancellationToken)
    {
        // In feature here should be 2 and 3 too
        if (request.RefinancingDocumentTypeId is not 1)
            throw new NobyValidationException(90032);

        var signatureTypeDetail = (await _codebookService.SignatureTypeDetails(cancellationToken))
                               .SingleOrDefault(s => s.Id == request.SignatureTypeDetailId);

        if (signatureTypeDetail?.IsRetentionAvailable != true)
            throw new NobyValidationException(90032);

        if (request.SignatureDeadline < _time.GetLocalNow())
            throw new NobyValidationException(90032, "SignatureDeadline is lower than current time");

        var saDetail = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (saDetail.Retention?.ManagedByRC2 is null or true)
            throw new NobyValidationException(90032, "ManagedByRC2 is true or SA is not retention SA");

        if (saDetail.State is not (int)SalesArrangementStates.InProgress)
            throw new NobyValidationException(90032, "SA has to be in state InProgress(1)");

        var offer = await _offerService.GetOfferDetail(saDetail.OfferId!.Value, cancellationToken);

        if (((DateTime)offer.MortgageRetention.SimulationInputs.InterestRateValidFrom) < _time.GetLocalNow().Date)
            throw new NobyValidationException(90051);

        if (offer.MortgageRetention.SimulationInputs.InterestRateDiscount is not null || offer.MortgageRetention.BasicParameters.AmountDiscount is not null)
        {
            await ValidateTask(request, saDetail, offer, cancellationToken);
        }

        await UpdateSaParams(request, saDetail, cancellationToken);

        var user = await _userService.GetUser(_currentUser.User!.Id, cancellationToken);

        switch (request.RefinancingDocumentTypeId)
        {
            case 1:
                await GenerateRetentionDocument(user, saDetail, offer, request, cancellationToken);
                break;
            case 2:
                await GenerateIndividualAnnouncement(user, cancellationToken);
                break;
            case 3:
                await GenerateHedgeDocument(user, cancellationToken);
                break;
            default:
                throw new NobyValidationException(90032);
        }

        await _salesArrangementService.UpdateSalesArrangementState(saDetail.SalesArrangementId, (int)SalesArrangementStates.InSigning, cancellationToken);
    }

    private async Task ValidateTask(GenerateRefinancingDocumentRequest request, _contract.SalesArrangement saDetail, GetOfferDetailResponse offer, CancellationToken cancellationToken)
    {
        var taskList = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(p => p.ProcessId == saDetail.TaskProcessId && p.TaskTypeId == (int)WorkflowTaskTypes.PriceException)
            .ToList();

        if (taskList.Count == 0)
            throw new NobyValidationException(90032, "Empty collection");

        var nonCanceledTask = taskList.SingleOrDefault(t => !t.Cancelled)
            ?? throw new NobyValidationException(90050);

        if (nonCanceledTask.StateIdSb != 30)
            throw new NobyValidationException(90049);

        if (nonCanceledTask.DecisionId != 1)
            throw new NobyValidationException(90032, "Not exist DecisionId == 1");

        var taskDetail = await _caseService.GetTaskDetail(nonCanceledTask.TaskIdSb, cancellationToken);

        // ToDo refixation missing
        if (!(taskDetail.TaskDetail.PriceException.LoanInterestRate.LoanInterestRateDiscount == offer.MortgageRetention.SimulationInputs.InterestRateDiscount
                       && taskDetail.TaskDetail.PriceException.Fees[0].FinalSum == offer.MortgageRetention.BasicParameters.AmountDiscount))
        {
            throw new NobyValidationException(90048);
        }
    }

    private Task GenerateHedgeDocument(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task GenerateIndividualAnnouncement(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task GenerateRetentionDocument(User user,
        _contract.SalesArrangement sa,
        GetOfferDetailResponse offerDetail,
        GenerateRefinancingDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var simulationInputs = offerDetail.MortgageRetention.SimulationInputs;
        var simulationResults = offerDetail.MortgageRetention.SimulationResults;
        var basicParams = offerDetail.MortgageRetention.BasicParameters;
        decimal? interestRate = simulationInputs.InterestRate;
        decimal? interestRateDiscount = simulationInputs.InterestRateDiscount;
        decimal? paymentAmount = simulationResults.LoanPaymentAmountDiscounted ?? simulationResults.LoanPaymentAmount;

        await _sbWebApi.GenerateRetentionDocument(new()
        {
            CaseId = sa.CaseId,
            InterestRate = interestRate - interestRateDiscount,
            DateFrom = offerDetail.MortgageRetention.SimulationInputs.InterestRateValidFrom,
            PaymentAmount = paymentAmount,
            SignatureTypeDetailId = request.SignatureTypeDetailId,
            Cpm = user.UserInfo.Cpm,
            Icp = user.UserInfo.Icp,
            SignatureDeadline = request.SignatureDeadline,
            IndividualPricing = simulationInputs.InterestRateDiscount is not null || basicParams.AmountDiscount is not null,
            Fee = basicParams.AmountDiscount ?? basicParams.Amount
        }, cancellationToken);
    }

    private async Task UpdateSaParams(GenerateRefinancingDocumentRequest request, _contract.SalesArrangement sa, CancellationToken cancellationToken)
    {
        sa.Retention.SignatureTypeDetailId = request.SignatureTypeDetailId;
        sa.Retention.SignatureDeadline = request.SignatureDeadline;

        var saRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = sa.SalesArrangementId,
            Retention = sa.Retention
        };

        await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
    }
}
