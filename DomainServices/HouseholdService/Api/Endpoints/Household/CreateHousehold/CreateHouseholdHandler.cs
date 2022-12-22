using DomainServices.HouseholdService.Contracts;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdRequest, CreateHouseholdResponse>
{
    public async Task<CreateHouseholdResponse> Handle(CreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        // check existing SalesArrangementId
        var saInstance = await __SAlesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

#pragma warning disable CA2208
        // Debtor domacnost muze byt jen jedna
        if (request.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Main && _dbContext.Households.Any(t => t.SalesArrangementId == request.SalesArrangementId))
            throw new CisArgumentException(16031, "Only one Debtor household allowed", "HouseholdTypeId");

        // check household role
        if (!(await _codebookService.HouseholdTypes(cancellationToken)).Any(t => t.Id == request.HouseholdTypeId))
            throw new CisNotFoundException(16023, $"HouseholdTypeId {request.HouseholdTypeId} does not exist.");
#pragma warning restore CA2208

        //TODO check propertySettlement?

        // ulozit customera do databaze
        var entity = new Database.Entities.Household
        {
            SalesArrangementId = request.SalesArrangementId,
            CaseId = saInstance.CaseId,
            HouseholdTypeId = (CIS.Foms.Enums.HouseholdTypes)request.HouseholdTypeId,
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

        return new CreateHouseholdResponse()
        {
            HouseholdId = entity.HouseholdId
        };
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient __SAlesArrangementService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateHouseholdHandler> _logger;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;

    public CreateHouseholdHandler(
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateHouseholdHandler> logger)
    {
        __SAlesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}