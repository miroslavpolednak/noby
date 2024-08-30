using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateCustomerDetail;

internal sealed class UpdateCustomerDetailHandler(
    IGrpcServerResponseCache _responseCache,
    Database.HouseholdServiceDbContext _dbContext,
    IDocumentDataStorage _documentDataStorage,
    CustomerOnSADataMapper _mapper)
        : IRequestHandler<UpdateCustomerDetailRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCustomerDetailRequest request, CancellationToken cancellationToken)
    {
        // vytahnout stavajici entitu z DB
        var entity = await _dbContext
            .Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // change data
        entity.ChangeData = request.CustomerChangeData;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var documentEntity = _mapper.MapToData(request.CustomerAdditionalData, request.CustomerChangeMetadata);

        await _documentDataStorage.UpdateByEntityId(request.CustomerOnSAId, documentEntity);

        await _responseCache.InvalidateEntry(nameof(GetCustomerList), entity.SalesArrangementId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}