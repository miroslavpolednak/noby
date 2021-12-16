using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateSalesArrangementHandler
    : IRequestHandler<Dto.CreateSalesArrangementMediatrRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(Dto.CreateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Create SA {type} for #{caseId}", request.SalesArrangementType, request.CaseId);

        var caseInstance = await _caseService.GetCaseDetail(request.CaseId);
        //TODO nejaka validace na case?

        var salesArrangementId = await _repository.CreateSalesArrangement(new()
        {
            CaseId = request.CaseId,
            SalesArrangementType = request.SalesArrangementType,
            InsertUserId = request.UserId
        });

        return new CreateSalesArrangementResponse { SalesArrangementId = salesArrangementId };
    }

    private readonly CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateSalesArrangementHandler> _logger;

    public CreateSalesArrangementHandler(
        CaseService.Abstraction.ICaseServiceAbstraction caseService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateSalesArrangementHandler> logger)
    {
        _caseService = caseService;
        _repository = repository;
        _logger = logger;
    }
}
