﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateValuationTypeByRealEstateValuation;

internal sealed class UpdateValuationTypeByRealEstateValuationHandler
    : IRequestHandler<UpdateValuationTypeByRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateValuationTypeByRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // ulozit do DB
        entity.ValuationTypeId = request.ValuationTypeId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public UpdateValuationTypeByRealEstateValuationHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}