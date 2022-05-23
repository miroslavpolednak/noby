namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement;

internal class UpdateLoanAssessmentParametersHandler
    : IRequestHandler<Dto.UpdateLoanAssessmentParametersMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateLoanAssessmentParametersMediatrRequest request, CancellationToken cancellation)
    {
        await _repository.UpdateLoanAssessment(request.Request.SalesArrangementId, request.Request.LoanApplicationAssessmentId, request.Request.RiskSegment, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementDataHandler> _logger;

    public UpdateLoanAssessmentParametersHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementDataHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
