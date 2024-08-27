﻿using System.Diagnostics.CodeAnalysis;

namespace DomainServices.UserService.Clients;

public static class UserExtensions
{
    public static bool HasPermission([NotNull] this Contracts.User user, Authorization.UserPermissions permission)
        => user.UserPermissions?.Contains((int)permission) ?? false;

    public static bool HasPermission([NotNull] this Contracts.User user, int permission)
        => user.UserPermissions?.Contains(permission) ?? false;
}
