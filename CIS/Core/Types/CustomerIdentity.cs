namespace CIS.Core.Types;

public record CustomerIdentity
{
    public int Id { get; }
    public Enums.IdentitySchemes Scheme { get; }

    public CustomerIdentity(int id, Enums.IdentitySchemes scheme)
    {
        Id = id;
        Scheme = scheme;
    }

    public CustomerIdentity(int? id, string? scheme) 
        : this(id.GetValueOrDefault(), scheme) { }

    public CustomerIdentity(int id, string? scheme)
    {
        Id = id;
        if (!Enum.TryParse(scheme, out Enums.IdentitySchemes parsedScheme))
            throw new Exceptions.CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(scheme));
        Scheme = parsedScheme;
    }

    public CustomerIdentity(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new Exceptions.CisArgumentNullException(1, "CustomerIdentity token is null or empty", nameof(token));

        int idx = token.IndexOf(':');
        if (idx < 1)
            throw new Exceptions.CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!int.TryParse(token.AsSpan(idx + 1), out int id))
            throw new Exceptions.CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!Enum.TryParse(token.Substring(0, idx), out Enums.IdentitySchemes parsedScheme))
            throw new Exceptions.CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(token));

        Id = id;
        Scheme = parsedScheme;
    }
}
