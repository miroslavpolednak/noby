namespace DomainServices.UserService.Api;

internal static class Helpers
{
    public static string CreateUserCacheKey(string userId)
    {
        return $"CisUser:{userId}";
    }

    public static string CreateUserCacheKey(int userId)
        => CreateUserCacheKey(userId.ToString(System.Globalization.CultureInfo.InvariantCulture));
}
