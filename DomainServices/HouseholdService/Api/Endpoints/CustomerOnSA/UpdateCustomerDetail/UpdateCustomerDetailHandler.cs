using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomerDetail;

internal sealed class UpdateCustomerDetailHandler(
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
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // change data
        entity.ChangeData = request.CustomerChangeData;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var documentEntity = _mapper.MapToData(request.CustomerAdditionalData, request.CustomerChangeMetadata);

        await _documentDataStorage.UpdateByEntityId(request.CustomerOnSAId, documentEntity);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}