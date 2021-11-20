using CIS.Infrastructure.gRPC;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.CaseService;

internal class GetCaseDetailHandler
    : IRequestHandler<Dto.CaseService.GetCaseDetailMediatrRequest, GetCaseDetailResponse>
{
    public async Task<GetCaseDetailResponse> Handle(Dto.CaseService.GetCaseDetailMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get detail for #{id}", request.CaseId);

        var model = await _repository.GetCaseDetail(request.CaseId);
        if (model == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);

        return model;
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public GetCaseDetailHandler(
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
