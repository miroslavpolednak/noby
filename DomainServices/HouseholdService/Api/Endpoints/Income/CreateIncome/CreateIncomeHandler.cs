using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.CreateIncome;

internal sealed class CreateIncomeHandler(
    IDocumentDataStorage _documentDataStorage,
    IncomeMapper _incomeMapper,
    ILogger<CreateIncomeHandler> _logger)
        : IRequestHandler<CreateIncomeRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _incomeMapper.MapToData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        var id = await _documentDataStorage.Add(request.CustomerOnSAId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.DocumentDataEntities.Income), id);

        return new CreateIncomeResponse
        {
            IncomeId = id
        };
    }
}