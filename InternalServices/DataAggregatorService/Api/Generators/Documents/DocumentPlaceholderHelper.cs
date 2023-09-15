using System.Text.RegularExpressions;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents;

public static partial class DocumentPlaceholderHelper
{
    public static string ReplaceNonBreakingPlaceholders(string text)
    {
        var regex = NonBreakingSpacePlaceholderRegex();

        return regex.Replace(text, "\u00A0");
    }

    [GeneratedRegex(@"(?<=[\p{L}\p{N}])~(?=[\p{L}\p{N}])")]
    private static partial Regex NonBreakingSpacePlaceholderRegex();
}