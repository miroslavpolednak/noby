using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseDataHandler
    : IRequestHandler<Dto.UpdateCaseDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update case data for #{caseId}", request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        await _repository.UpdateCaseData(request.CaseId, request.ContractNumber);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseDataHandler(
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
