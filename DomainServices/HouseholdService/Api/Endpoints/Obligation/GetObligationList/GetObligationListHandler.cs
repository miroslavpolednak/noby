using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Api.Endpoints.Income.GetIncomeList;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.GetObligationList;

internal sealed class GetObligationListHandler
    : IRequestHandler<GetObligationListRequest, GetObligationListResponse>
{
    public async Task<GetObligationListResponse> Handle(GetObligationListRequest request, CancellationToken cancellationToken)
    {
        var list = await _documentDataStorage.GetList<Database.DocumentDataEntities.Obligation>(request.CustomerOnSAId, cancellationToken);

        if (list.Count == 0 && !_dbContext.Customers.Any(t => t.CustomerOnSAId == request.CustomerOnSAId))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);
        }

        _logger.FoundItems(list.Count, nameof(Database.Entities.CustomerOnSAObligation));

        var obligations = list.Select(t => _mapper.MapFromDataToList(t));

        var response = new GetObligationListResponse();
        response.Obligations.AddRange(obligations);
        return response;
    }

    private readonly HouseholdServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ObligationMapper _mapper;
    private readonly ILogger<GetIncomeListHandler> _logger;

    public GetObligationListHandler(
        IDocumentDataStorage documentDataStorage,
        ObligationMapper mapper,
        HouseholdServiceDbContext dbContext,
        ILogger<GetIncomeListHandler> logger)
    {
        _documentDataStorage = documentDataStorage;
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }
}