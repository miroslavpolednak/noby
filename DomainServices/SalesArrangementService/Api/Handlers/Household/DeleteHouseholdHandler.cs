
namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class DeleteHouseholdHandler
    : IRequestHandler<Dto.DeleteHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteHouseholdHandler), request.HouseholdId);
        
        var householdInstance = await _repository.GetHousehold(request.HouseholdId, cancellation);
        if (householdInstance.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Debtor)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new CisArgumentException(16032, "Can't delete Debtor household", "HouseholdId");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

        await _repository.Delete(request.HouseholdId, cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.HouseholdRepository _repository;
    private readonly ILogger<DeleteHouseholdHandler> _logger;
    
    public DeleteHouseholdHandler(
        Repositories.HouseholdRepository repository,
        ILogger<DeleteHouseholdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}