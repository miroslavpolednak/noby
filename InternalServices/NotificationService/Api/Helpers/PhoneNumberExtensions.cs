﻿using System.Text.RegularExpressions;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;

namespace CIS.InternalServices.NotificationService.Api.Helpers;

public static class PhoneNumberExtensions
{
    public static Phone? ParsePhone(this string value)
    {
        var normalizedPhoneNumber = value.NormalizePhoneNumber();
        var regex = new Regex(@"^\+(?<CountryCode>\d{1,3})(?<NationalDestinationCode>\d{2,3})(?<SubscriberNumber>\d{4,9})$");
        var match = regex.Match(normalizedPhoneNumber);

        if (!match.Success)
        {
            return null;
        }
        
        var countryCode = "+" + match.Groups["CountryCode"].Value;
        var nationalDestinationCode = match.Groups["NationalDestinationCode"].Value;
        var subscriberNumber = match.Groups["SubscriberNumber"].Value;
        
        return new Phone
        {
            CountryCode = countryCode,
            NationalNumber = nationalDestinationCode + subscriberNumber
        };
    }
    
    private static string NormalizePhoneNumber(this string value)
    {
        // odstranění mezer
        var phoneNumber =  value.Replace(" ","");
        
        // mezinárodní předvolba může začít buď + nebo 00, tak to převedeme na 00
        if(phoneNumber.StartsWith("00"))
        {  
            phoneNumber = "+" + phoneNumber.Substring(2);
        }

        return phoneNumber;
    }
    
    // private static readonly HashSet<string> _countryCallingCodes = new()
    // {
    //     "+30", "+31", "+32", "+33", "+34", "+36", "+39",
    //     "+40", "+43", "+45", "+46", "+47", "+48", "+49",
    //     "+351", "+352", "+353", "+354", "+356", "+357", "+358", "+359",
    //     "+370", "+371", "+372", "+385", "+386", "+420", "+421", "+423"
    // };
    //
    // private static string ParseCode(this string normalizedPhoneNumber)
    // {
    //     // postupně odkrajuješ od 5 do 2 a hledáš a koukám že se to děje od největšího po nejmenší
    //     for (var i  = Math.Max(normalizedPhoneNumber.Length, 5); i > 1; i--)
    //     {
    //         var code = normalizedPhoneNumber.Substring(0, i);
    //         if (_countryCallingCodes.Contains(code))
    //         {
    //             return code;
    //         }
    //     }
    //     
    //     // nevyhovuje
    //     return string.Empty;
    // }
}