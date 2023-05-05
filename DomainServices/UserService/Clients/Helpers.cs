namespace DomainServices.UserService.Clients;

internal static class Helpers
{
    public static string CreateUserCacheKey(int userId)
    {
        return $"CisUser:{userId}";
    }
}
