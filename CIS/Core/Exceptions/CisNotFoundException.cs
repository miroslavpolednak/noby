namespace CIS.Core.Exceptions
{
    public sealed class CisNotFoundException : BaseCisException
    {
        public CisNotFoundException(int exceptionCode, string message) 
            : base(exceptionCode, message) 
        { }
    }
}
