using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.GetIncome;

internal sealed class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, Contracts.Income>
{
    public async Task<Contracts.Income> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .CustomersIncomes
            .AsNoTracking()
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);

        var model = new Contracts.Income
        {
            IncomeId = entity.CustomerOnSAIncomeId,
            IncomeTypeId = (int)entity.IncomeTypeId,
            CustomerOnSAId = entity.CustomerOnSAId,
            BaseData = new IncomeBaseData
            {
                CurrencyCode = entity.CurrencyCode,
                Sum = entity.Sum
            }
        };

        switch (entity.IncomeTypeId)
        {
            case CustomerIncomeTypes.Employement:
                model.Employement = (await _documentDataStorage.GetDataWithMapper<Database.DocumentDataEntities.IncomeEmployement, IncomeDataEmployement>(entity.CustomerOnSAIncomeId, cancellationToken)).Data;
                break;

            case CustomerIncomeTypes.Other:
                model.Other = (await _documentDataStorage.GetDataWithMapper<Database.DocumentDataEntities.IncomeOther, IncomeDataOther>(entity.CustomerOnSAIncomeId, cancellationToken)).Data;
                break;

            case CustomerIncomeTypes.Entrepreneur:
                model.Entrepreneur = (await _documentDataStorage.GetDataWithMapper<Database.DocumentDataEntities.IncomeEntrepreneur, IncomeDataEntrepreneur>(entity.CustomerOnSAIncomeId, cancellationToken)).Data;
                break;

            case CustomerIncomeTypes.Rent:
                model.Rent = new IncomeDataRent();
                break;

            default:
                throw new NotImplementedException("This customer income type deserializer is not implemented");
        }
    
        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetIncomeHandler(Database.HouseholdServiceDbContext dbContext, IDocumentDataStorage documentDataStorage)
    {
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
    }
}