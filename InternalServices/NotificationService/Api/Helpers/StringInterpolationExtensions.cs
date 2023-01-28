using System.Text;
using System.Text.RegularExpressions;
using CIS.Core.Exceptions;

namespace CIS.InternalServices.NotificationService.Api.Helpers;

public static class StringInterpolationExtensions
{
    private static readonly string _pattern = @"[\{]{2}[a-z0-9_-]{3,15}[\}]{2}";
    
    public static void Validate(this string template, IEnumerable<string> keyNames)
    {
        var keys = keyNames.ToHashSet();
        var extracted = Regex.Matches(template, _pattern)
            .Select(m => m.Value.Trim('{', '}'))
            .ToHashSet();

        foreach (var k in keys)
        {
            if (!extracted.Contains(k))
            {
                throw new CisValidationException($"Key '{k}' is not expected.");
            }
        }

        foreach (var e in extracted)
        {
            if (!keys.Contains(e))
            {
                throw new CisValidationException($"Missing key '{e}'.");
            }
        }
    }

    public static string Interpolate(this string template, Dictionary<string, string> keyValues)
    {
        var builder = new StringBuilder(template);
        
        foreach (var pair in keyValues)
        {
            builder.Replace($"{{{{{pair.Key}}}}}", pair.Value);
        }
        
        return builder.ToString();
    }
}