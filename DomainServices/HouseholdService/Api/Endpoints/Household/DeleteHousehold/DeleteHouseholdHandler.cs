using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteHouseholdRequest request, CancellationToken cancellation)
    {
        var householdInstance = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.HouseholdId} does not exist.");

        if (householdInstance.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main && !request.HardDelete)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new CisArgumentException(16032, "Can't delete Debtor household", "HouseholdId");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

        // smazat domacnost
        _dbContext.Households.Remove(householdInstance);

        await _dbContext.SaveChangesAsync(cancellation);

        // smazat customerOnSA
        if (householdInstance.CustomerOnSAId1.HasValue)
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = householdInstance.CustomerOnSAId1.Value,
                HardDelete = request.HardDelete
            }, cancellation);

        if (householdInstance.CustomerOnSAId2.HasValue)
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = householdInstance.CustomerOnSAId2.Value,
                HardDelete = request.HardDelete
            }, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public DeleteHouseholdHandler(Database.HouseholdServiceDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
}