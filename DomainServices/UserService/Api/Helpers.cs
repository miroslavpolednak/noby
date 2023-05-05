namespace DomainServices.UserService.Api;

internal static class Helpers
{
    public static string CreateUserCacheKey(int userId)
    {
        return $"CisUser:{userId}";
    }
}
