using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.GetIncome;

internal sealed class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, Contracts.Income>
{
    public async Task<Contracts.Income> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CustomersIncomes
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

        if (entity.DataBin is not null)
        {
            switch (entity.IncomeTypeId)
            {
                case CustomerIncomeTypes.Employement:
                    model.Employement = IncomeDataEmployement.Parser.ParseFrom(entity.DataBin);
                    break;

                case CustomerIncomeTypes.Other:
                    model.Other = IncomeDataOther.Parser.ParseFrom(entity.DataBin);
                    break;

                case CustomerIncomeTypes.Enterprise:
                    model.Entrepreneur = IncomeDataEntrepreneur.Parser.ParseFrom(entity.DataBin);
                    break;

                case CustomerIncomeTypes.Rent:
                    model.Rent = IncomeDataRent.Parser.ParseFrom(entity.DataBin);
                    break;

                default:
                    throw new NotImplementedException("This customer income type deserializer is not implemented");
            }
        }

        return model;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetIncomeHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}