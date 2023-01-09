using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateLoanAssessmentParameters;

internal sealed class UpdateLoanAssessmentParametersHandler
    : IRequestHandler<UpdateLoanAssessmentParametersRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateLoanAssessmentParametersRequest request, CancellationToken cancellation)
    {
        await _repository.UpdateLoanAssessment(request.SalesArrangementId, request.LoanApplicationAssessmentId, request.RiskSegment, request.CommandId, request.RiskBusinessCaseExpirationDate, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.SalesArrangementServiceRepository _repository;

    public UpdateLoanAssessmentParametersHandler(Database.SalesArrangementServiceRepository repository)
    {
        _repository = repository;
    }
}
