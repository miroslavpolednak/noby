using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.HousingConditions;

namespace DomainServices.CodebookService.Endpoints.HousingConditions;

public class HousingConditionsHandler
    : IRequestHandler<HousingConditionsRequest, List<HousingConditionItem>>
{
    public async Task<List<HousingConditionItem>> Handle(HousingConditionsRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!     
        return new List<HousingConditionItem>
        {
            new HousingConditionItem() { Id = 1, Name = "6 - vlastník domu", Code = "OW", IsValid = true },
            new HousingConditionItem() { Id = 2, Name = "5 - vlastník bytu", Code = "OW", IsValid = true },
            new HousingConditionItem() { Id = 3, Name = "3 - družstevník", Code = "GR", IsValid = true },
            new HousingConditionItem() { Id = 4, Name = "2 - nájemník", Code = "RE", IsValid = true },
            new HousingConditionItem() { Id = 5, Name = "4 - ostatní", Code = "OT", IsValid = true },
            new HousingConditionItem() { Id = 6, Name = "7 - vlastník domu/bytu", Code = "OW", IsValid = true },
            new HousingConditionItem() { Id = 8, Name = "8 - bydlení u rodičů", Code = "PA", IsValid = true },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<HousingConditionsHandler> _logger;

    public HousingConditionsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<HousingConditionsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}