namespace CIS.Infrastructure.Security;

internal sealed class StaticLoginValidator : ILoginValidator
{
    private static readonly Dictionary<string, string> _logins = new()
    {
        { "a", "a" },
        { "test", "Test" },
        { "sb", "pwd" },
        { "epodpisy", "epodpisy" },
        { "insign", "insign" }
    };

    public Task<bool> Validate(string login, string password)
    {
        return Task.FromResult(_logins.ContainsKey(login) && _logins[login] == password);
    }
}
