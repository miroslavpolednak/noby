namespace DomainServices.UserService.Contracts;

public static class Helpers
{
    public static string GetUserCacheKey(string login)
        => $"u:{login}";
}
