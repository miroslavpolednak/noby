using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler
    : IRequestHandler<DeleteRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.RealEstateValuations.FirstOrDefaultAsync(t => t.RealEstateValuationId == request.NobyOrderId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.NobyOrderId);

        // ulozit do DB
        _dbContext.RealEstateValuations.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public DeleteRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
