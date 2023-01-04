namespace DomainServices.CustomerService.ExternalServices.Common;

public static class Extensions
{
    public static string ToQuery(this bool? value)
        => !value.GetValueOrDefault(false) ? "false" : "true";

    public static string ToQuery(this bool value)
        => value ? "true" : "false";
}
