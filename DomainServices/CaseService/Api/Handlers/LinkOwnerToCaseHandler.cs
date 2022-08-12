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
        var userInstance = resolveUserResult(await _userService.GetUser(request.CaseOwnerUserId, cancellation));

        // update majitele v databazi
        await _repository.LinkOwnerToCase(request.CaseId, request.CaseOwnerUserId, userInstance.FullName, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static UserService.Contracts.User resolveUserResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<UserService.Contracts.User> r => r.Model,
            ErrorServiceCallResult err => throw new CisNotFoundException(13022, $"User not found: {err.Errors[0].Message}"),
            _ => throw new NotImplementedException()
        };

    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<LinkOwnerToCaseHandler> _logger;

    public LinkOwnerToCaseHandler(
        UserService.Abstraction.IUserServiceAbstraction userService,
        Repositories.CaseServiceRepository repository,
        ILogger<LinkOwnerToCaseHandler> logger)
    {
        _userService = userService;
        _repository = repository;
        _logger = logger;
    }
}