using CIS.Infrastructure.gRPC;
using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Create SA {type} for #{caseId}, #{productId}", request.SalesArrangementType, request.CaseId, request.ProductInstanceId);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        if (caseInstance == null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "CaseId does not exist.", 13000);
        //TODO nejaka validace na case?

        var salesArrangementId = await _repository.CreateSalesArrangement(new()
        {
            CaseId = request.CaseId,
            SalesArrangementType = request.SalesArrangementType,
            ProductInstanceId = request.ProductInstanceId,
            InsertTime = _dateTime.Now,
            InsertUserId = 1//TODO pridat userid
        });

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;
    private readonly CIS.Core.IDateTime _dateTime;

    public CreateSalesArrangementHandler(
        CIS.Core.IDateTime dateTime,
        Repositories.NobyDbRepository repository,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _dateTime = dateTime;
        _repository = repository;
        _logger = logger;
    }
}
