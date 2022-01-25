﻿namespace DomainServices.UserService.Api.Handlers;

internal class GetUserHandler
    : IRequestHandler<Dto.GetUserMediatrRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Dto.GetUserMediatrRequest request, CancellationToken cancellation)
    {
        // vytahnout info o uzivateli z DB
        var userInstance = await _repository.GetUser(request.UserId);
        if (userInstance is null)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.NotFound, $"User #{request.UserId} not found", 1);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            Id = userInstance.v33id,
            CPM = userInstance.v33cpm ?? "",
            ICP = userInstance.v33icp ?? "",
            FullName = $"{userInstance.v33jmeno} {userInstance.v33prijmeni}".Trim(),
            Login = "",
            Email = "",
            Phone = ""
        };

        return model;
    }

    private readonly Repositories.XxvRepository _repository;
    private readonly ILogger<GetUserByLoginHandler> _logger;

    public GetUserHandler(
        Repositories.XxvRepository repository,
        ILogger<GetUserByLoginHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
