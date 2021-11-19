namespace CIS.Security.InternalServices
{
    public sealed class ParseResult
    {
        public bool Success { get; init; }
        public string ErrorMessage { get; init; } = "";
        public string Login { get; init; } = "";
        public string Password { get; init; } = "";
        public int ErrorCode { get; init; }

        internal ParseResult(int errorCode, string errorMessage)
        {
            Success = false;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        internal ParseResult(string login, string password)
        {
            Success = true;
            Login = login;
            Password = password;
        }
    }
}
