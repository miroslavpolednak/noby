using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class GetIncomeHandler
    : IRequestHandler<Dto.GetIncomeMediatrRequest, Income>
{
    public async Task<Income> Handle(Dto.GetIncomeMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Income ID {request.IncomeId} does not exist.");

        var model = new Income
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
                case CIS.Foms.Enums.CustomerIncomeTypes.Employement:
                    model.Employement = IncomeDataEmployement.Parser.ParseFrom(entity.DataBin);
                    break;
                case CIS.Foms.Enums.CustomerIncomeTypes.Other:
                    model.Other = IncomeDataOther.Parser.ParseFrom(entity.DataBin);
                    break;
                case CIS.Foms.Enums.CustomerIncomeTypes.Enterprise:
                    model.Entrepreneur = IncomeDataEntrepreneur.Parser.ParseFrom(entity.DataBin);
                    break;
                default:
                    throw new NotImplementedException("This customer income type deserializer is not implemented");
            }
        }
        
        return model;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public GetIncomeHandler(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
