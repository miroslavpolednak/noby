﻿using Google.Protobuf;

namespace DomainServices.UserService.Api.Endpoints.GetUserPermissions;

internal sealed class GetUserPermissionsHandler
    : IRequestHandler<Contracts.GetUserPermissionsRequest, Contracts.GetUserPermissionsResponse>
{
    public async Task<Contracts.GetUserPermissionsResponse> Handle(Contracts.GetUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        // zkusit cache
        string cacheKey = Helpers.CreateUserPermissionsCacheKey(request.UserId);
        var cachedBytes = await _distributedCache.GetAsync(cacheKey, cancellationToken);
        if (cachedBytes != null)
        {
            return Contracts.GetUserPermissionsResponse.Parser.ParseFrom(cachedBytes);
        }

        var dbPermissions = await _db.ExecuteDapperStoredProcedureSqlToListAsync<dynamic>(
            "[dbo].[getPermissions]",
            new { ApplicationCode = "NOBY", v33id = request.UserId },
            cancellationToken);

        // vytvorit finalni model
        var model = new Contracts.GetUserPermissionsResponse();
        dbPermissions.ForEach(t =>
        {
            if (int.TryParse(t.PermissionCode, out int p))
            {
                model.UserPermissions.Add(p);
            }
        });

        await _distributedCache.SetAsync(Helpers.CreateUserPermissionsCacheKey(request.UserId), model.ToByteArray(), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_minutesInCache),
        },
        cancellationToken);

        return model;
    }

    private const int _minutesInCache = 30;
    private readonly IConnectionProvider _db;
    private readonly IDistributedCache _distributedCache;

    public GetUserPermissionsHandler(
        IConnectionProvider db,
        IDistributedCache distributedCache)
    {
        _db = db;
        _distributedCache = distributedCache;
    }
}
