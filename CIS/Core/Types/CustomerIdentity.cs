namespace CIS.Core.Types
{
    public record CustomerIdentity
    {
        public int Id { get; init; }
        public IdentitySchemes Scheme { get; init; }

        public CustomerIdentity(int id, IdentitySchemes scheme)
        {
            Id = id;
            Scheme = scheme;
        }

        public CustomerIdentity(int id, string scheme)
        {
            Id = id;
            if (!Enum.TryParse<IdentitySchemes>(scheme, out IdentitySchemes parsedScheme))
                throw new Exceptions.CisArgumentException(1, "CustomerIdentity scheme is not in valid format", "scheme");
            Scheme = parsedScheme;
        }

        public CustomerIdentity(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exceptions.CisArgumentNullException(1, "CustomerIdentity token is null or empty", "token");

            int idx = token.IndexOf(':');
            if (idx < 1)
                throw new Exceptions.CisArgumentException(1, "CustomerIdentity token is not in valid format", "token");
            if (!int.TryParse(token.Substring(idx + 1), out int id))
                throw new Exceptions.CisArgumentException(1, "CustomerIdentity token is not in valid format", "token");
            if (!Enum.TryParse<IdentitySchemes>(token.Substring(0, idx), out IdentitySchemes parsedScheme))
                throw new Exceptions.CisArgumentException(1, "CustomerIdentity scheme is not in valid format", "scheme");

            Id = id;
            Scheme = parsedScheme;
        }
    }
}
