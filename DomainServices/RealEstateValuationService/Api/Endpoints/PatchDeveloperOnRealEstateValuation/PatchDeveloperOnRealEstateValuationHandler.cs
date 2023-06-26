using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PatchDeveloperOnRealEstateValuation;

internal sealed class PatchDeveloperOnRealEstateValuationHandler
    : IRequestHandler<PatchDeveloperOnRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(PatchDeveloperOnRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.RealEstateValuations.FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // ulozit do DB
        entity.ValuationStateId = request.ValuationStateId;
        entity.DeveloperApplied = request.DeveloperApplied;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public PatchDeveloperOnRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
