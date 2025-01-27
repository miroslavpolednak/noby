﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.SetForeignRealEstateTypesByRealEstateValuation;

internal sealed class SetForeignRealEstateTypesByRealEstateValuationHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<SetForeignRealEstateTypesByRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(SetForeignRealEstateTypesByRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // ulozit do DB
        entity.ACVRealEstateTypeId = request.ACVRealEstateTypeId;
        entity.BagmanRealEstateTypeId = request.BagmanRealEstateTypeId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
