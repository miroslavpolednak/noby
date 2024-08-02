using System.Text.RegularExpressions;

namespace CIS.InternalServices.NotificationService.Api;

internal static partial class StringExtensions
{
    public static string GetDomainFromEmail(this string email)
    {
        return email[(email.IndexOf('@') + 1)..];
    }

    public static bool TryParsePhone(this string value, out string? countryCode, out string? nationalNumber)
    {
        var match = _phoneRegEx.Match(value.NormalizePhoneNumber());

        if (!match.Success)
        {
            countryCode = null;
            nationalNumber = null;
            return false;
        }

        countryCode = "+" + match.Groups["CountryCode"].Value;
        nationalNumber = match.Groups["NationalDestinationCode"].Value + match.Groups["SubscriberNumber"].Value;

        return true;
    }

    private static Regex _phoneRegEx = new Regex(@"^\+(?<CountryCode>\d{1,3})(?<NationalDestinationCode>\d{2,3})(?<SubscriberNumber>\d{4,9})$", RegexOptions.Singleline | RegexOptions.Compiled);

    private static string NormalizePhoneNumber(this string value)
    {
        // odstranění mezer
        var phoneNumber = value.Replace(" ", "");

        // mezinárodní předvolba může začít buď + nebo 00, tak to převedeme na 00
        if (phoneNumber.StartsWith("00", StringComparison.OrdinalIgnoreCase))
        {
            phoneNumber = string.Concat("+", phoneNumber.Substring(2));
        }

        return phoneNumber;
    }

    public static string ResolveSenderEmail(this string originalSenderEmail, Dictionary<string, string> _emailSenderMapping)
    {
        if (_emailSenderMapping.TryGetValue(originalSenderEmail, out string? translatedSenderEmail))
        {
            return translatedSenderEmail;
        }
        return originalSenderEmail;
    }
}
