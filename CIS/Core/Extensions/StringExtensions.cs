using System.Text;
using System.Text.RegularExpressions;

namespace CIS.Core;

public static class StringExtensions
{
    private static Regex _castCamelCaseToDashDelimitedRegex = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);

    public static string CastCamelCaseToDashDelimited(this string source)
        => _castCamelCaseToDashDelimitedRegex.Replace(source, "-$1").ToLower(System.Globalization.CultureInfo.InvariantCulture);

    public static string TrimUtf8String(ref string input, int maxSizeInBytes)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);

        if (bytes.Length > maxSizeInBytes)
        {
            Array.Resize(ref bytes, maxSizeInBytes);
        }

        return Encoding.UTF8.GetString(bytes);
    }
}