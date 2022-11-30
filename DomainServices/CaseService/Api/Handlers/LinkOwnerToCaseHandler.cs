using CIS.Core.Results;

namespace DomainServices.CaseService.Api.Handlers;

internal class LinkOwnerToCaseHandler
    : IRequestHandler<Dto.LinkOwnerToCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkOwnerToCaseMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.CaseId, cancellation);

        // overit ze existuje uzivatel
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);

        // update majitele v databazi
        await _repository.LinkOwnerToCase(request.CaseId, request.CaseOwnerUserId, userInstance.FullName, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<LinkOwnerToCaseHandler> _logger;

    public LinkOwnerToCaseHandler(
        UserService.Clients.IUserServiceClient userService,
        Repositories.CaseServiceRepository repository,
        ILogger<LinkOwnerToCaseHandler> logger)
    {
        _userService = userService;
        _repository = repository;
        _logger = logger;
    }
}