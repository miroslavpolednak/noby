namespace DomainServices.UserService.Clients;

internal static class Helpers
{
    public static string CreateUserCacheKey(string userId)
    {
        return $"CisUser:{userId}";
    }
}
