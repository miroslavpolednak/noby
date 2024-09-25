using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Api.Extensions;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.CreateIncome;

internal sealed class CreateIncomeHandler(
    IDocumentDataStorage _documentDataStorage,
    IncomeMapper _incomeMapper,
    ILogger<CreateIncomeHandler> _logger)
        : IRequestHandler<CreateIncomeRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        if ((EnumIncomeTypes)request.IncomeTypeId == EnumIncomeTypes.Employement)
        {
            // pouze employement zaznamy
            var employmentIncomes = (await _documentDataStorage.GetList<Database.DocumentDataEntities.Income, int>(request.CustomerOnSAId, cancellationToken))
                .Where(t => t.Data!.IncomeTypeId == EnumIncomeTypes.Employement)
                .ToList();

            if (employmentIncomes.Any() && IncomeHelpers.IsNotIncomeEmployerUnique(request.Employement?.Employer?.Cin, request.Employement?.Employer?.BirthNumber, employmentIncomes))
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.TwoSameIncomes);
            }
        }
        
        var documentEntity = await _incomeMapper.MapToData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        var id = await _documentDataStorage.Add(request.CustomerOnSAId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.DocumentDataEntities.Income), id);

        return new()
        {
            IncomeId = id
        };
    }
}