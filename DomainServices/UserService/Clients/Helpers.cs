namespace DomainServices.UserService.Clients;

internal static class Helpers
{
    public static string CreateUserCacheKey(string userId)
        => $"CisUser:{userId}";

    public static string CreateUserPermissionsCacheKey(int userId)
        => $"CisUser:{userId}:Permissions";
}
