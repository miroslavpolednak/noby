using System.Text;

namespace CIS.InternalServices.NotificationService.Api.Helpers;

public static class GSMExtensions
{
    //GSM 03.38 character set
    private static readonly HashSet<char> _gsmChars = new HashSet<char>
    {
        '@', '£', '$', '¥', 'è', 'é', 'ù', 'ì', 'ò', 'Ç', '\n', 'Ø', 'ø', '\r', 'Å', 'å',
        'Δ', '_', 'Φ', 'Γ', 'Λ', 'Ω', 'Π', 'Ψ', 'Σ', 'Θ', 'Ξ', 'Æ', 'æ', 'ß', 'É',
        ' ', '!', '\"', '#', '¤', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
        '¡', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ä', 'Ö', 'Ñ', 'Ü', '§',
        '¿', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
        'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ä', 'ö', 'ñ', 'ü', 'à',
        // Basic Character Set Extension
        '^', '{', '}', '\\', '[', '~', ']', '|', '€',
        // Czech characters
        'č', 'Č', 'š', 'Š', 'ř', 'Ř', 'ž', 'Ž', 'ý', 'Ý', 'á', 'Á', 'í', 'Í', 'é', 'É', 'ě', 'Ě', 'ú', 'Ú', 'ů', 'Ů',
        'ť', 'Ť', 'ď', 'Ď', 'ň', 'Ň', 'ó', 'Ó'
    };

    // non GSM 03.38 character set replacement
    private static readonly Dictionary<char, char[]> _gsmCharsReplacement = new Dictionary<char, char[]> {
        { 'à', new char[] { 'a' } }, { 'â', new char[] { 'a' } }, { 'ä', new char[] { 'a' } }, { 'À', new char[] { 'A' } }, { 'Â', new char[] { 'A' } }, { 'Ä', new char[] { 'A' } },
        { 'è', new char[] { 'e' } }, { 'ê', new char[] { 'e' } }, { 'ë', new char[] { 'e' } }, { 'È', new char[] { 'E' } }, { 'Ê', new char[] { 'E' } }, { 'Ë', new char[] { 'E' } },
        { 'ì', new char[] { 'i' } }, { 'î', new char[] { 'i' } }, { 'ï', new char[] { 'i' } }, { 'Ì', new char[] { 'I' } }, { 'Î', new char[] { 'I' } }, { 'Ï', new char[] { 'I' } },
        { 'ò', new char[] { 'o' } }, { 'ô', new char[] { 'o' } }, { 'ö', new char[] { 'o' } }, { 'Ò', new char[] { 'O' } }, { 'Ô', new char[] { 'O' } }, { 'Ö', new char[] { 'O' } },
        { 'ù', new char[] { 'u' } }, { 'û', new char[] { 'u' } }, { 'ü', new char[] { 'u' } }, { 'Ù', new char[] { 'U' } }, { 'Û', new char[] { 'U' } }, { 'Ü', new char[] { 'U' } },
        { 'ą', new char[] { 'a' } }, { 'Ą', new char[] { 'A' } }, { 'ę', new char[] { 'e' } }, { 'Ę', new char[] { 'E' } }, { 'ð', new char[] { 'd' } }, { 'Ð', new char[] { 'D' } },
        { 'ş', new char[] { 's' } }, { 'Ş', new char[] { 'S' } }, { 'ł', new char[] { 'l' } }, { 'Ł', new char[] { 'L' } }, { 'đ', new char[] { 'd' } }, { 'Đ', new char[] { 'D' } },
        { 'ņ', new char[] { 'n' } }, { 'Ņ', new char[] { 'N' } }, { 'ź', new char[] { 'z' } }, { 'ż', new char[] { 'z' } }, { 'Ź', new char[] { 'Z' } }, { 'Ż', new char[] { 'Z' } },
        { 'ç', new char[] { 'c' } }, { 'Ç', new char[] { 'C' } }, { 'ñ', new char[] { 'n' } }, { 'Ñ', new char[] { 'N' } }, { 'ğ', new char[] { 'g' } }, { 'Ğ', new char[] { 'G' } },
        { 'ĸ', new char[] { 'k' } }, { 'ļ', new char[] { 'l' } }, { 'Ļ', new char[] { 'L' } }, { 'ľ', new char[] { 'l' } }, { 'Ľ', new char[] { 'L' } }, { 'ĺ', new char[] { 'l' } },
        { 'Ĺ', new char[] { 'L' } },
        { '„', new char[] { '"' } }, { '“', new char[] { '"' } }, { '‚', new char[] { '\'' } }, { '‘', new char[] { '\'' } }, { '`', new char[] { '\'' } },
        { 'ß', new char[] { 's','s' } }, { 'ẞ', new char[] { 'S','S' } }, { 'æ', new char[] { 'a','e' } }, { 'Æ', new char[] { 'A','E' } },
        { 'œ', new char[] { 'o','e' } }, { 'Œ', new char[] { 'O','E' } }, { 'þ', new char[] { 't','h' } }, { 'Þ', new char[] { 'T','H' } }
    };

    private static readonly char _defaultEscapeChar = '?';

    public static string ToGSMString(this string input)
    {
        StringBuilder result = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (_gsmChars.Contains(c))
            {
                result.Append(c);
            }
            else if (_gsmCharsReplacement.TryGetValue(c, out var replacement))
            {
                result.Append(replacement);
            }
            else
            {
                result.Append(_defaultEscapeChar);
            }
        }
        return result.ToString();
    }
}
