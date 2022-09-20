namespace DomainServices.CustomerService.Api.Extensions;

public static class CMExtensions
{
    public static string? ToCMString(this string str)
    {
        return string.IsNullOrWhiteSpace(str) ? null : str;
    }
}