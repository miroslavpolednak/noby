using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.CaseService;

internal class UpdateCaseStateHandler
    : IRequestHandler<Dto.CaseService.UpdateCaseStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.CaseService.UpdateCaseStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update case state for #{caseId} to {state}", request.CaseId, request.State);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        await _repository.UpdateCaseState(request.CaseId, request.State);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseStateHandler(
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

