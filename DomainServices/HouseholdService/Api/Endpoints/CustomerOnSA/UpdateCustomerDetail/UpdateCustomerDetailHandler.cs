using DomainServices.HouseholdService.Contracts;
using Google.Protobuf;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomerDetail;

internal sealed class UpdateCustomerDetailHandler
    : IRequestHandler<UpdateCustomerDetailRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCustomerDetailRequest request, CancellationToken cancellation)
    {
        // vytahnout stavajici entitu z DB
        var entity = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // additional data
        entity.AdditionalData = request.CustomerAdditionalData == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(request.CustomerAdditionalData);
        entity.AdditionalDataBin = request.CustomerAdditionalData == null ? null : request.CustomerAdditionalData.ToByteArray();

        // change data
        if (request.CustomerChangeData == null)
        {
            entity.ChangeDataBin = null;
            entity.ChangeData = null;
        }
        else
        {
            entity.ChangeData = Newtonsoft.Json.JsonConvert.SerializeObject(request.CustomerChangeData);

            // musi to byt takto ob ruku, protoze RepeatedField se neda rovnou serializovat
            var list = new CustomerChangeDataItemWrapper();
            list.ChangeData.AddRange(request.CustomerChangeData);
            entity.ChangeDataBin = list.ToByteArray();
        }

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateCustomerDetailHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
