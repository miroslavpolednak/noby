namespace CIS.Core.Exceptions
{
    public sealed class CisInvalidApplicationKeyException : BaseCisArgumentException
    {
        public const int _exceptionCode = 3;

        public CisInvalidApplicationKeyException(string key) 
            : base(_exceptionCode, $"Application key '{key}' is invalid", "key")
        { }

        public CisInvalidApplicationKeyException(string key, string paramName)
            : base(_exceptionCode, $"Application key '{key}' is invalid", paramName)
        { }
    }
}
