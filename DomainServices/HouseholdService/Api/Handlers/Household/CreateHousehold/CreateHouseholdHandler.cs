using DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.Household.CreateHousehold;

internal sealed class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdMediatrRequest, CreateHouseholdResponse>
{
    public async Task<CreateHouseholdResponse> Handle(CreateHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        // check existing SalesArrangementId
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.Request.SalesArrangementId, cancellation));

#pragma warning disable CA2208
        // Debtor domacnost muze byt jen jedna
        if (request.Request.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Main && _dbContext.Households.Any(t => t.SalesArrangementId == request.Request.SalesArrangementId))
            throw new CisArgumentException(16031, "Only one Debtor household allowed", "HouseholdTypeId");

        // check household role
        if (!(await _codebookService.HouseholdTypes(cancellation)).Any(t => t.Id == request.Request.HouseholdTypeId))
            throw new CisNotFoundException(16023, $"HouseholdTypeId {request.Request.HouseholdTypeId} does not exist.");
#pragma warning restore CA2208

        //TODO check propertySettlement?

        // ulozit customera do databaze
        var entity = new Repositories.Entities.Household
        {
            SalesArrangementId = request.Request.SalesArrangementId,
            CaseId = saInstance.CaseId,
            HouseholdTypeId = (CIS.Foms.Enums.HouseholdTypes)request.Request.HouseholdTypeId,
            CustomerOnSAId1 = request.Request.CustomerOnSAId1,
            CustomerOnSAId2 = request.Request.CustomerOnSAId2,
            ChildrenOverTenYearsCount = request.Request.Data?.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = request.Request.Data?.ChildrenUpToTenYearsCount,
            PropertySettlementId = request.Request.Data?.PropertySettlementId,
            AreBothPartnersDeptors = request.Request.Data?.AreBothPartnersDeptors,
            AreCustomersPartners = request.Request.Data?.AreCustomersPartners,
            HousingExpenseAmount = request.Request.Expenses?.HousingExpenseAmount,
            SavingExpenseAmount = request.Request.Expenses?.SavingExpenseAmount,
            InsuranceExpenseAmount = request.Request.Expenses?.InsuranceExpenseAmount,
            OtherExpenseAmount = request.Request.Expenses?.OtherExpenseAmount
        };

        _dbContext.Households.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.Household), entity.HouseholdId);

        return new CreateHouseholdResponse()
        {
            HouseholdId = entity.HouseholdId
        };
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateHouseholdHandler> _logger;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CreateHouseholdHandler(
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.HouseholdServiceDbContext dbContext,
        ILogger<CreateHouseholdHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}