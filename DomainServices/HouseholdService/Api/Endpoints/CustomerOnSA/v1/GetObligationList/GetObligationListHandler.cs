using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.GetIncomeList;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.GetObligationList;

internal sealed class GetObligationListHandler(
    IDocumentDataStorage _documentDataStorage,
    ObligationMapper _mapper,
    HouseholdServiceDbContext _dbContext,
    ILogger<GetIncomeListHandler> _logger)
        : IRequestHandler<GetObligationListRequest, GetObligationListResponse>
{
    public async Task<GetObligationListResponse> Handle(GetObligationListRequest request, CancellationToken cancellationToken)
    {
        var list = await _documentDataStorage.GetList<Database.DocumentDataEntities.Obligation, int>(request.CustomerOnSAId, cancellationToken);

        if (list.Count == 0 && !_dbContext.Customers.Any(t => t.CustomerOnSAId == request.CustomerOnSAId))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);
        }

        _logger.FoundItems(list.Count, nameof(Database.DocumentDataEntities.Obligation));

        var obligations = list.Select(t => _mapper.MapFromDataToList(t));

        var response = new GetObligationListResponse();
        response.Obligations.AddRange(obligations);
        return response;
    }
}