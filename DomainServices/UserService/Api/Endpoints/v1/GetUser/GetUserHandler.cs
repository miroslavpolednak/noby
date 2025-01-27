﻿using DomainServices.UserService.Api.Dto;

namespace DomainServices.UserService.Api.Endpoints.v1.GetUser;

internal sealed class GetUserHandler(IConnectionProvider _db)
        : IRequestHandler<Contracts.GetUserRequest, Contracts.User>
{
    public async Task<Contracts.User> Handle(Contracts.GetUserRequest request, CancellationToken cancellationToken)
    {
        // vytahnout info o uzivateli z DB
        var dbIdentities = await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<Dto.DbUserIdentity>(
            "[dbo].[getUserIdentities]",
            new { identitySchema = request.Identity.IdentityScheme.ToString(), identityValue = request.Identity.Identity },
            cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.Identity.IdentityScheme}={request.Identity.Identity}");

        // dotahnout atributy
        var dbAttributes = await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<GetUserAttributesDto>(
            "[dbo].[getUserAttributes]",
            new { dbIdentities.v33id },
            cancellationToken);

        var dbPermissions = await _db.ExecuteDapperStoredProcedureSqlToListAsync<GetPermissionsDto>(
            "[dbo].[getPermissions]",
            new { ApplicationCode = "NOBY", dbIdentities.v33id },
            cancellationToken);

        // vytvorit finalni model
        var model = new Contracts.User
        {
            UserId = dbIdentities.v33id,
            UserInfo = new Contracts.UserInfoObject
            {
                FirstName = dbIdentities.firstname ?? "",
                LastName = dbIdentities.surname ?? "",
                Cin = string.IsNullOrWhiteSpace(dbAttributes?.companyCin) ? getDefaultCustomerIdentificationNumber(dbIdentities) : dbAttributes?.companyCin,
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
        model.UserInfo.IsInternal = !model.UserIdentifiers.Any(t => t.IdentityScheme == SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId) && model.UserIdentifiers.Count != 0;

        // perms
        dbPermissions.ForEach(t =>
        {
            if (int.TryParse(t.PermissionCode, out int p))
            {
                model.UserPermissions.Add(p);
            }
        });

        return model;
    }

    private static string getDefaultCustomerIdentificationNumber(Dto.DbUserIdentity dbIdentities)
    {
        if (!string.IsNullOrWhiteSpace(dbIdentities.kbad))
            return "45317054";

        if (!string.IsNullOrWhiteSpace(dbIdentities.mpad))
            return "63998017";

        return string.Empty;
    }

    private static void fillIdentities(Dto.DbUserIdentity dbIdentities, Contracts.User user)
    {
        if (dbIdentities.brokerId.HasValue)
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.brokerId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId
            });

        if (!string.IsNullOrEmpty(dbIdentities.kbuid))
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.kbuid,
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.KbUid
            });

        if (!string.IsNullOrEmpty(dbIdentities.mpad))
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.mpad,
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Mpad
            });

        if (dbIdentities.m04id.HasValue)
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.m04id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.M04Id
            });

        if (dbIdentities.m17id.HasValue)
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.m17id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.M17Id
            });

        if (dbIdentities.oscis.HasValue)
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.oscis.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.OsCis
            });

        if (!string.IsNullOrEmpty(dbIdentities.kbad))
            user.UserIdentifiers.Add(new SharedTypes.GrpcTypes.UserIdentity
            {
                Identity = dbIdentities.kbad,
                IdentityScheme = SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.Kbad
            });
    }
}
