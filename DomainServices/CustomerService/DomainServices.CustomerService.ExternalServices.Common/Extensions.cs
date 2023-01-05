namespace DomainServices.CustomerService.ExternalServices.Common;

public static class Extensions
{
    public static string ToQuery(this bool value)
        => value ? "true" : "false";
}
