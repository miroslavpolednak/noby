namespace CIS.Core.Exceptions
{
    public sealed class CisInvalidEnvironmentNameException : BaseCisArgumentException
    {
        private const int _exceptionCode = 4;

        public CisInvalidEnvironmentNameException(string name)
            : base(_exceptionCode, $"Environment Name '{name}' is invalid", "key")
        { }

        public CisInvalidEnvironmentNameException(string name, string paramName)
            : base(_exceptionCode, $"Environment Name '{name}' is invalid", paramName)
        { }
    }
}
