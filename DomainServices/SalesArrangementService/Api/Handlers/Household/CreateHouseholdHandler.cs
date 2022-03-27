﻿using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class CreateHouseholdHandler
    : IRequestHandler<Dto.CreateHouseholdMediatrRequest, CreateHouseholdResponse>
{
    public async Task<CreateHouseholdResponse> Handle(Dto.CreateHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateHouseholdHandler));
        
        // check existing SalesArrangementId
        var saInstance = await _saRepository.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);

#pragma warning disable CA2208
        // Debtor domacnost muze byt jen jedna
        if (request.Request.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Main && _dbContext.Households.Any(t => t.SalesArrangementId == request.Request.SalesArrangementId))
            throw new CisArgumentException(16031, "Only one Debtor household allowed", "HouseholdTypeId");

        // check household role
        if (!(await _codebookService.HouseholdTypes(cancellation)).Any(t => t.Id == request.Request.HouseholdTypeId))
            throw new CisArgumentException(16023, $"HouseholdTypeId {request.Request.HouseholdTypeId} does not exist.", "HouseholdTypeId");
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
    
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ILogger<CreateHouseholdHandler> _logger;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    
    public CreateHouseholdHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceDbContext dbContext,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateHouseholdHandler> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _saRepository = saRepository;
        _logger = logger;
    }
}