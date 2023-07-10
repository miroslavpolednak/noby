using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdRequest, CreateHouseholdResponse>
{
    public async Task<CreateHouseholdResponse> Handle(CreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        // check existing SalesArrangementId
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // ulozit customera do databaze
        var entity = new Database.Entities.Household
        {
            SalesArrangementId = request.SalesArrangementId,
            CaseId = saInstance.CaseId,
            HouseholdTypeId = (HouseholdTypes)request.HouseholdTypeId,
            CustomerOnSAId1 = request.CustomerOnSAId1,
            CustomerOnSAId2 = request.CustomerOnSAId2,
            ChildrenOverTenYearsCount = request.Data?.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = request.Data?.ChildrenUpToTenYearsCount,
            PropertySettlementId = request.Data?.PropertySettlementId,
            AreBothPartnersDeptors = request.Data?.AreBothPartnersDeptors,
            HousingExpenseAmount = request.Expenses?.HousingExpenseAmount,
            SavingExpenseAmount = request.Expenses?.SavingExpenseAmount,
            InsuranceExpenseAmount = request.Expenses?.InsuranceExpenseAmount,
            OtherExpenseAmount = request.Expenses?.OtherExpenseAmount
        };

        _dbContext.Households.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Household), entity.HouseholdId);

        // pokud se jedna o spoludluznickou, nastavujeme flowswitch
        if (request.HouseholdTypeId == (int)HouseholdTypes.Codebtor)
        {
            await _salesArrangementService.SetFlowSwitches(request.SalesArrangementId, new()
            {
                new()
                {
                    FlowSwitchId = (int)FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                    Value = false
                }
            }, cancellationToken);
        }

        return new CreateHouseholdResponse()
        {
            HouseholdId = entity.HouseholdId
        };
    }

    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateHouseholdHandler> _logger;

    public CreateHouseholdHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateHouseholdHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _dbContext = dbContext;
        _logger = logger;
    }
}