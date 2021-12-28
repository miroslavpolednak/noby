namespace CIS.Core.Results
{
    public class ErrorServiceCallResult : IServiceCallResult
    {
        public IReadOnlyCollection<(int Key, string Message)> Errors { get; init; }
        public bool Success => false;

        public ErrorServiceCallResult(int key, string message)
        {
            Errors = new List<(int Key, string Message)>(1)
            {
                (key, message)
            };
        }

        public ErrorServiceCallResult(IEnumerable<(int Key, string Message)> errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }
}
