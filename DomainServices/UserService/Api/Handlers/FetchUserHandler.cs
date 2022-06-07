using System.Diagnostics;

namespace DomainServices.UserService.Api.Handlers;

internal sealed class FetchUserHandler
    : IRequestHandler<Dto.FetchUserMediatrRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Dto.FetchUserMediatrRequest request, CancellationToken cancellation)
    {
        int? partyId = null;
        if (int.TryParse(Activity.Current?.Baggage.FirstOrDefault(b => b.Key == "MpPartyId").Value, out int i))
            partyId = i;

        if (partyId is null)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.NotFound, $"User not logged in", 1);

        // vytahnout info o uzivateli z DB
        var userInstance = await _repository.GetUser(partyId.Value);
        if (userInstance is null)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.NotFound, $"User #{partyId.Value} not found", 1);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = userInstance.v33id,
            CPM = userInstance.v33cpm ?? "",
            ICP = userInstance.v33icp ?? "",
            FullName = $"{userInstance.v33jmeno} {userInstance.v33prijmeni}".Trim(),
            Email = "",
            Phone = ""
        };
        model.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity(string.IsNullOrEmpty(model.ICP) ? model.CPM : $"{model.CPM}_{model.ICP}", CIS.Foms.Enums.UserIdentitySchemes.Mpad));

        return model;
    }

    private readonly Repositories.XxvRepository _repository;

    public FetchUserHandler(Repositories.XxvRepository repository)
    {
        _repository = repository;
    }
}
