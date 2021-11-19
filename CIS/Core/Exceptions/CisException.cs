namespace CIS.Core.Exceptions
{
    public sealed class CisException : BaseCisException
    {
        public CisException(int exceptionCode, string message)
            : base(exceptionCode, message)
        { }
    }
}
