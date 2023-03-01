using DomainServices.HouseholdService.Contracts;
using Google.Protobuf;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomerDetail;

internal sealed class UpdateCustomerDetailHandler
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

        // additional data
        entity.AdditionalData = request.CustomerAdditionalData == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(request.CustomerAdditionalData);
        entity.AdditionalDataBin = request.CustomerAdditionalData == null ? null : request.CustomerAdditionalData.ToByteArray();
        // change data
        entity.ChangeData = request.CustomerChangeData;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateCustomerDetailHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
