using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.SetACVRealEstateTypeByRealEstateValuation;

internal sealed class SetACVRealEstateTypeByRealEstateValuationHandler
    : IRequestHandler<SetACVRealEstateTypeByRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(SetACVRealEstateTypeByRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuationDetails
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // ulozit do DB
        entity.ACVRealEstateTypeId = request.ACVRealEstateType;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public SetACVRealEstateTypeByRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
