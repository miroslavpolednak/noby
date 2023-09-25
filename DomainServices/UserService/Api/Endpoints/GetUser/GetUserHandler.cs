using DomainServices.UserService.Api.Database.Entities;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Endpoints.GetUser;

internal sealed class GetUserHandler
    : IRequestHandler<Contracts.GetUserRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Contracts.GetUserRequest request, CancellationToken cancellationToken)
    {
        if (request.Identity.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.V33Id)
        {
            // zkusit cache
            string cacheKey = Helpers.CreateUserCacheKey(request.Identity.Identity);
            var cachedBytes = await _distributedCache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes != null)
            {
                return Contracts.User.Parser.ParseFrom(cachedBytes);
            }
        }

        // vytahnout info o uzivateli z DB
        var dbIdentities = (await _dbContext.UserIdentities
            .FromSqlInterpolated($"EXECUTE [dbo].[getUserIdentities] @identitySchema={request.Identity.IdentityScheme.ToString()}, @identityValue={request.Identity.Identity}")
            .ToListAsync(cancellationToken)
            ).FirstOrDefault()
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.Identity.IdentityScheme}={request.Identity.Identity}");

        // dotahnout atributy
        var dbAttributes = (await _dbContext.DbUserAttributes
            .FromSqlInterpolated($"EXECUTE [dbo].[getUserAttributes] @v33id={dbIdentities.v33id}")
            .ToListAsync(cancellationToken)
            ).FirstOrDefault();

        var dbPermissions = await _dbContext.DbUserPermissions
            .FromSqlInterpolated($"EXECUTE [dbo].[getPermissions] @ApplicationCode='NOBY', @v33id={dbIdentities.v33id}")
            .ToListAsync(cancellationToken);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            UserId = dbIdentities.v33id,
            UserInfo = new Contracts.UserInfoObject
            {
                FirstName = dbIdentities.firstname ?? "",
                LastName = dbIdentities.surname ?? "",
                Cin = string.IsNullOrWhiteSpace(dbAttributes?.companyCin) ? GetDefaultCustomerIdentificationNumber(dbIdentities) : dbAttributes.companyCin,
                Cpm = dbIdentities.cpm,
                Icp = dbIdentities.icp,
                DisplayName = $"{dbIdentities.firstname} {dbIdentities.surname}".Trim(), //Trim because some users have full name only in the Surname field
                Email = dbAttributes?.email,
                PhoneNumber = dbAttributes?.phone,
                IsUserVIP = !string.IsNullOrEmpty(dbAttributes?.VIPFlag),
                ChannelId = dbAttributes?.distributionChannelId ?? 4,
                PersonOrgUnitName = dbAttributes?.personOrgUnitName,
                DealerCompanyName = dbAttributes?.dealerCompanyName
            }
        };

        // identity
        fillIdentities(dbIdentities, model);

        // set is internal
        model.UserInfo.IsInternal = !model.UserIdentifiers.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId) && model.UserIdentifiers.Any();

        // perms
        dbPermissions.ForEach(t =>
        {
            if (int.TryParse(t.PermissionCode, out int p))
            {
                model.UserPermissions.Add(p);
            }
        });

        await _distributedCache.SetAsync(Helpers.CreateUserCacheKey(model.UserId), model.ToByteArray(), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_minutesInCache),
        },
        cancellationToken);

        return model;
    }

    private static void fillIdentities(DbUserIdentity dbIdentities, Contracts.User user)
    {
        if (dbIdentities.brokerId.HasValue)
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.brokerId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId
            });

        if (!string.IsNullOrEmpty(dbIdentities.kbuid))
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.kbuid,
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.KbUid
            });

        if (!string.IsNullOrEmpty(dbIdentities.mpad))
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.mpad,
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.Mpad
            });

        if (dbIdentities.m04id.HasValue)
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.m04id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.M04Id
            });

        if (dbIdentities.m17id.HasValue)
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.m17id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.M17Id
            });

        if (dbIdentities.oscis.HasValue)
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.oscis.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.OsCis
            });

        if (!string.IsNullOrEmpty(dbIdentities.kbad))
            user.UserIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.UserIdentity
            {
                Identity = dbIdentities.kbad,
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes.Kbad
            });
    }

    private static string GetDefaultCustomerIdentificationNumber(DbUserIdentity dbIdentities)
    {
        if (!string.IsNullOrWhiteSpace(dbIdentities.kbad))
            return "45317054";

        if (!string.IsNullOrWhiteSpace(dbIdentities.mpad))
            return "63998017";

        return string.Empty;
    }

    private const int _minutesInCache = 30;
    private readonly Database.UserServiceDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;

    public GetUserHandler(
        Database.UserServiceDbContext dbContext,
        IDistributedCache distributedCache)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
    }
}
