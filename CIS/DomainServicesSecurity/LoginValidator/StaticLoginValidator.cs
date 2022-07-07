namespace CIS.DomainServicesSecurity;

internal class StaticLoginValidator : ILoginValidator
{
    private static readonly Dictionary<string, string> _logins = new()
    {
        { "a", "a" },
        { "b", "b" },
        { "sb", "pwd" }
    };

    public Task<bool> Validate(string login, string password)
    {
        return Task.FromResult(_logins.ContainsKey(login) && _logins[login] == password);
    }
}
