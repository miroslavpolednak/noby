using CIS.Infrastructure.gRPC;
using DomainServices.CaseService.Contracts;
using Grpc.Core;

namespace DomainServices.CaseService.Api.Handlers.SalesArrangement;

internal class GetSalesArrangementsByCaseIdHandler
    : IRequestHandler<Dto.SalesArrangement.GetSalesArrangementsByCaseIdMediatrRequest, GetSalesArrangementsByCaseIdResponse>
{
    public async Task<GetSalesArrangementsByCaseIdResponse> Handle(Dto.SalesArrangement.GetSalesArrangementsByCaseIdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get list for CaseId #{id}", request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        var model = new GetSalesArrangementsByCaseIdResponse();
        var listData = await _repository.GetSalesArrangementsByCaseId(request.CaseId);
        model.SalesArrangements.AddRange(listData);

        return model;
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<GetSalesArrangementsByCaseIdHandler> _logger;

    public GetSalesArrangementsByCaseIdHandler(
        Repositories.NobyDbRepository repository,
        ILogger<GetSalesArrangementsByCaseIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
