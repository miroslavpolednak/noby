using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Api.Database;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        var householdInstance = await _dbContext
            .Households
            .FirstOrDefaultAsync(t => t.HouseholdId == request.HouseholdId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);

        // kontrola ze to neni main household
        if (householdInstance.HouseholdTypeId == HouseholdTypes.Main && !request.HardDelete)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CantDeleteDebtorHousehold);

        // smazat domacnost
        _dbContext.Households.Remove(householdInstance!);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // smazat customerOnSA
        if (householdInstance!.CustomerOnSAId1.HasValue)
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = householdInstance.CustomerOnSAId1.Value,
                HardDelete = request.HardDelete
            }, cancellationToken);

        if (householdInstance.CustomerOnSAId2.HasValue)
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = householdInstance.CustomerOnSAId2.Value,
                HardDelete = request.HardDelete
            }, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly HouseholdServiceDbContext _dbContext;

    public DeleteHouseholdHandler(HouseholdServiceDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
}