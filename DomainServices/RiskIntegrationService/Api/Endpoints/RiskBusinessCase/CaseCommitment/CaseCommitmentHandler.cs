namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.CaseCommitment;

//internal sealed class CaseCommitmentHandler
//    : IRequestHandler<Contracts.RiskBusinessCase.CaseCommitmentRequest, Contracts.RiskBusinessCase.CaseCommitmentResponse>
//{
//    public async Task<Contracts.RiskBusinessCase.CaseCommitmentResponse> Handle(Contracts.RiskBusinessCase.CaseCommitmentRequest request, CancellationToken cancellation)
//    {
//        var transformedRequest = new Clients.RiskBusinessCase.V1.Contracts.CommitRequest();

//        var result = await _client.CaseCommitment(request.RiskBusinessCaseId, transformedRequest, cancellation);

//        return new Contracts.RiskBusinessCase.CaseCommitmentResponse
//        {
//            OperationResult = result.OperationResult,
//            ResultReasons = result.ResultReasons is null ? null : result.ResultReasons.Select(t => new Contracts.RiskBusinessCase.CommitmentResultReason
//            {
//                Code = t.Code,
//                Desc = t.Desc
//            }).ToList(),
//            Timestamp = result.Timestamp.UtcDateTime
//        };
//    }

//    private readonly Clients.RiskBusinessCase.V1.IRiskBusinessCaseClient _client;

//    public CaseCommitmentHandler(Clients.RiskBusinessCase.V1.IRiskBusinessCaseClient client)
//    {
//        _client = client;
//    }
//}
