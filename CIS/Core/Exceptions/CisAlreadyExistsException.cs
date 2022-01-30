namespace CIS.Core.Exceptions
{
    public sealed class CisAlreadyExistsException : BaseCisException
    {
        public CisAlreadyExistsException(int exceptionCode, string message) 
            : base(exceptionCode, message) 
        { }
    }
}
