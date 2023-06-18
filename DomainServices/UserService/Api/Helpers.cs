namespace DomainServices.UserService.Api;

internal static class Helpers
{
    public static string CreateUserCacheKey(string userId)
        => $"CisUser:{userId}";

    public static string CreateUserCacheKey(int userId)
        => CreateUserCacheKey(userId.ToString(System.Globalization.CultureInfo.InvariantCulture));

    public static string CreateUserPermissionsCacheKey(int userId)
        => $"CisUser:{userId}:Permissions";
}
