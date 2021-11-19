namespace CIS.Core.Exceptions
{
    public sealed class ServiceUnavailableException : BaseCisException
    {
        public ServiceUnavailableException(string serviceName, string message) 
            : base(5, $"'{serviceName}' not available: {message}") 
        { }
    }
}
