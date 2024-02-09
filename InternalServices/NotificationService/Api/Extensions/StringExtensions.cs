using System.Text.RegularExpressions;

namespace CIS.InternalServices.NotificationService.Api.Extensions;

internal static class StringExtensions
{
    public static (string CountryCode, string NationalNumber) ParsePhone(this string value)
    {
        var match = _phoneRegEx.Match(value.NormalizePhoneNumber());

        if (!match.Success)
        {
            throw new CIS.Core.Exceptions.CisValidationException("Phone number is in wrong format");
        }

        var countryCode = "+" + match.Groups["CountryCode"].Value;
        var nationalDestinationCode = match.Groups["NationalDestinationCode"].Value;
        var subscriberNumber = match.Groups["SubscriberNumber"].Value;

        return (countryCode, nationalDestinationCode + subscriberNumber);
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
}
