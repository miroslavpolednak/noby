﻿using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.LinkOwnerToCase;

internal sealed class LinkOwnerToCaseHandler(
    UserService.Clients.IUserServiceClient _userService,
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
        var userInstance = await _userService.GetUser(request.CaseOwnerUserId, cancellation);

        // update majitele v databazi
        entity.OwnerUserId = request.CaseOwnerUserId;
        entity.OwnerUserName = userInstance.UserInfo.DisplayName;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}