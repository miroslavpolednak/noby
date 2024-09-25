using CIS.Infrastructure.Caching.Grpc;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.LinkOwnerToCase;

internal sealed class LinkOwnerToCaseHandler(
    IGrpcServerResponseCache _responseCache,
    UserService.Clients.v1.IUserServiceClient _userService,
    CaseServiceDbContext _dbContext)
        : IRequestHandler<LinkOwnerToCaseRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkOwnerToCaseRequest request, CancellationToken cancellation)
    {
        // case entity
        var entity = await _dbContext
            .Cases
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // overit ze existuje uzivatel
        var userInstance = await _userService.GetUser(new SharedTypes.Types.UserIdentity
        {
            Identity = request.CaseOwnerUserId.ToString(CultureInfo.InvariantCulture),
            Scheme = UserIdentitySchemes.V33Id
        }, cancellation);
        
        // update majitele v databazi
        entity.OwnerUserId = request.CaseOwnerUserId;
        entity.OwnerUserName = userInstance.UserInfo.DisplayName;

        await _dbContext.SaveChangesAsync(cancellation);

        await _responseCache.InvalidateEntry(nameof(ValidateCaseId), request.CaseId);
        await _responseCache.InvalidateEntry(nameof(GetCaseDetail), request.CaseId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}