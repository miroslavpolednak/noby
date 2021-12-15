using CIS.Infrastructure.gRPC;
using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementDetailHandler
    : IRequestHandler<Dto.GetSalesArrangementDetailMediatrRequest, GetSalesArrangementDetailResponse>
{
    public async Task<GetSalesArrangementDetailResponse> Handle(Dto.GetSalesArrangementDetailMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get detail for #{id}", request.SalesArrangementId);

        var model = await _repository.GetSalesArrangementDetail(request.SalesArrangementId);
        if (model == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);

        return model;
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<GetSalesArrangementDetailHandler> _logger;

    public GetSalesArrangementDetailHandler(
        Repositories.NobyDbRepository repository,
        ILogger<GetSalesArrangementDetailHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
