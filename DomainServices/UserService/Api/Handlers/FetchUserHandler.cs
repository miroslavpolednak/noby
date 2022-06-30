namespace DomainServices.UserService.Api.Handlers;

internal sealed class FetchUserHandler
    : IRequestHandler<Dto.FetchUserMediatrRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Dto.FetchUserMediatrRequest request, CancellationToken cancellation)
    {
        int? partyId = _currentUserAccessor.User?.Id;
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

    private readonly CIS.Core.Security.ICurrentUserAccessor _currentUserAccessor;
    private readonly Repositories.XxvRepository _repository;

    public FetchUserHandler(Repositories.XxvRepository repository, CIS.Core.Security.ICurrentUserAccessor currentUserAccessor)
    {
        _repository = repository;
        _currentUserAccessor = currentUserAccessor;
    }
}
