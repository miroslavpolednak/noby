namespace ExternalServices.Eas;

public static class EasHelpers
{
    public static string CreateEasDate(DateTime d) => d.ToString("dd.MM.yyyy");
    public static string CreateEasDate() => DateTime.Now.ToString("dd.MM.yyyy");

    public static DateTime? CreateDateFromEas(string? d)
    {
        if (string.IsNullOrEmpty(d)) return null;
        if (DateTime.TryParse(d, out DateTime d2))
            return d2;
        else return null;
    }
}
