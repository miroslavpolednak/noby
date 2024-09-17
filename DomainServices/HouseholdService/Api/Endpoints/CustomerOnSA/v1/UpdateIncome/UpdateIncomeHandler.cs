using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Api.Extensions;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateIncome;

internal sealed class UpdateIncomeHandler(
    IncomeMapper _incomeMapper,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<Income, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Income request, CancellationToken cancellationToken)
    {
        if ((EnumIncomeTypes)request.IncomeTypeId == EnumIncomeTypes.Employement)
        {
            // pouze employement zaznamy
            var employmentIncomes = (await _documentDataStorage.GetList<Database.DocumentDataEntities.Income, int>(request.CustomerOnSAId, cancellationToken))
                .Where(t => t.Data!.IncomeTypeId == EnumIncomeTypes.Employement && t.DocumentDataStorageId != request.IncomeId)
                .ToList();

            if (IncomeHelpers.IsIncomeEmployerUnique(request.Employement?.Employer?.Cin, request.Employement?.Employer?.BirthNumber, employmentIncomes))
            {
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.TwoSameIncomes);
            }
        }

        var documentEntity = await _incomeMapper.MapToData(request.IncomeTypeId, request.BaseData, request.Employement, request.Entrepreneur, request.Other, cancellationToken);

        await _documentDataStorage.Update(request.IncomeId, documentEntity);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}