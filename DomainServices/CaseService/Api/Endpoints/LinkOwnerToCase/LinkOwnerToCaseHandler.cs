using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.LinkOwnerToCase;

internal sealed class LinkOwnerToCaseHandler
    : IRequestHandler<LinkOwnerToCaseRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkOwnerToCaseRequest request, CancellationToken cancellation)
    {
        // case entity
        var entity = await _dbContext
            .Cases
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // overit ze existuje uzivatel
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);

        // update majitele v databazi
        entity.OwnerUserId = request.CaseOwnerUserId;
        entity.OwnerUserName = userInstance.FullName;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly CaseServiceDbContext _dbContext;

    public LinkOwnerToCaseHandler(
        UserService.Clients.IUserServiceClient userService,
        CaseServiceDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }
}