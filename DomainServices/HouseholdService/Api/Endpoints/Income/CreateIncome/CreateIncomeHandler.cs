using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.CreateIncome;

internal sealed class CreateIncomeHandler
    : IRequestHandler<CreateIncomeRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _incomeMapper.MapToDocumentData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        var id = await _documentDataStorage.Add(request.CustomerOnSAId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSAIncome), id);

        return new CreateIncomeResponse
        {
            IncomeId = id
        };
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Services.IncomeToDataMapper _incomeMapper;
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        IDocumentDataStorage documentDataStorage,
        Services.IncomeToDataMapper incomeMapper,
        ILogger<CreateIncomeHandler> logger)
    {
        _incomeMapper = incomeMapper;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
    }
}