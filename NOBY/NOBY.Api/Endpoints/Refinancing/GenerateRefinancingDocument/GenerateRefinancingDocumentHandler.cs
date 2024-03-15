using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefinancingDocument;

public class GenerateRefinancingDocumentHandler : IRequestHandler<GenerateRefinancingDocumentRequest>
{
    private readonly TimeProvider _time;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICaseServiceClient _caseService;

    public GenerateRefinancingDocumentHandler(
        TimeProvider time,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ICaseServiceClient caseService
        )
    {
        _time = time;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _caseService = caseService;
    }

    public async Task Handle(GenerateRefinancingDocumentRequest request, CancellationToken cancellationToken)
    {
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
            var taskList = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
                .Where(p => p.ProcessId == saDetail.TaskProcessId && p.TaskTypeId == (int)WorkflowTaskTypes.PriceException)
                .ToList();

            if (taskList.Count == 0)
                throw new NobyValidationException(90032, "Empty collection");

            if (taskList.All(t => t.Cancelled))
                throw new NobyValidationException(90050);

            if (!taskList.Any(t => t.StateIdSb == 30))
                throw new NobyValidationException(90049);

            if (!taskList.Any(t => t.DecisionId == 1))
                throw new NobyValidationException(90032, "Not exist DecisionId == 1");
        }
        else
        {

        }

        //ToDO interestRateDiscount and FeeAmoun... missing

        // ToDo Task merge
        // ToDo SalesArrangementParametersRetention misisng

    }
}
