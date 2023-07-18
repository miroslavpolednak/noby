using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

//TODO tady by asi mel byt nejaky rollback, kdyz se nepodari smazat customer? Co se ma mazat driv - customer nebo household?
internal sealed class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        var household = await _dbContext
            .Households
            .AsNoTracking()
            .Where(t => t.HouseholdId == request.HouseholdId)
            .Select(t => new { t.SalesArrangementId, t.CustomerOnSAId1, t.CustomerOnSAId2, t.HouseholdTypeId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);

        // kontrola ze to neni main household
        if (household.HouseholdTypeId == HouseholdTypes.Main && !request.HardDelete)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CantDeleteDebtorHousehold);

        // smazat domacnost
        await _dbContext
            .Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat customerOnSA
        if (household.CustomerOnSAId1.HasValue)
        {
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = household.CustomerOnSAId1.Value,
                HardDelete = request.HardDelete
            }, cancellationToken);
        }

        if (household.CustomerOnSAId2.HasValue)
        {
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = household.CustomerOnSAId2.Value,
                HardDelete = request.HardDelete
            }, cancellationToken);
        }
        
        // pokud se jedna o spoludluznickou, smazat flowswitch
        if (household.HouseholdTypeId == HouseholdTypes.Codebtor)
        {
            await _salesArrangementService.SetFlowSwitches(household.SalesArrangementId, new List<SalesArrangementService.Contracts.EditableFlowSwitch>
            {
                new()
                {
                    FlowSwitchId = (int)FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                    Value = null
                }
            }, cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly IMediator _mediator;
    private readonly HouseholdServiceDbContext _dbContext;

    public DeleteHouseholdHandler(HouseholdServiceDbContext dbContext, IMediator mediator, SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _dbContext = dbContext;
        _mediator = mediator;
    }
}