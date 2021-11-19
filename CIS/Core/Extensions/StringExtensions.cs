using System;
using System.Text.RegularExpressions;

namespace CIS.Core
{
    public static class StringExtensions
    {
        private static Regex _castCamelCaseToDashDelimitedRegex = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);

        public static string CastCamelCaseToDashDelimited(this string source)
            =>  _castCamelCaseToDashDelimitedRegex.Replace(source, "-$1").ToLower();
    }
}
