using CIS.Core.Exceptions;

namespace CIS.Core.Types
{
    public record ApplicationKey
    {
        public string Key { get; init; }

        public ApplicationKey(string? key)
        {
            if (string.IsNullOrEmpty(key))
                throw new CisInvalidApplicationKeyException(key ?? "");
            if (!key.All(t => Char.IsLetterOrDigit(t) || t == ':'))
                throw new CisInvalidApplicationKeyException(key);
            else
            {
                this.Key = key;
            }
        }

        public static implicit operator string(ApplicationKey d) => d.Key;

        public override string ToString() => $"{Key}";
    }
}
