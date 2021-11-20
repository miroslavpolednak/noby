using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.SalesArrangement;

internal class UpdateSalesArrangementDataHandler
    : IRequestHandler<Dto.SalesArrangement.UpdateSalesArrangementDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.SalesArrangement.UpdateSalesArrangementDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update #{id}", request.SalesArrangementId);

        var arrangement = await _repository.GetSalesArrangement(request.SalesArrangementId);
        if (arrangement == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "SalesArrangementId does not exist.", 13000);
        //TODO nejaka validace na case?

        //await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<UpdateSalesArrangementDataHandler> _logger;

    public UpdateSalesArrangementDataHandler(
        Repositories.NobyDbRepository repository,
        ILogger<UpdateSalesArrangementDataHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
