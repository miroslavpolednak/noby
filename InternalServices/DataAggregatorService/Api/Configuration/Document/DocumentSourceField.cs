using System.Text.RegularExpressions;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal partial class DocumentSourceField : SourceFieldBase
{
    private readonly string? _stringFormat;
    private readonly string? _defaultTextIfNull;

    public string AcroFieldName { get; init; } = null!;

    public string? StringFormat
    {
        get => _stringFormat;
        init => ReplaceNonBreakingPlaceholders(ref _stringFormat, value);
    }

    public byte? TextAlign { get; init; }

    public byte? VAlign { get; init; }

    public string? DefaultTextIfNull
    {
        get => _defaultTextIfNull;
        init => ReplaceNonBreakingPlaceholders(ref _defaultTextIfNull, value);
    }

    private static void ReplaceNonBreakingPlaceholders(ref string? variable, string? text)
    {
        if (text is null)
        {
            variable = default;

            return;
        }

        var regex = NonBreakingSpacePlaceholderRegex();

        variable = regex.Replace(text, "\u00A0");
    }

    [GeneratedRegex(@"(?<=[\p{L}\p{N}])~(?=[\p{L}\p{N}])")]
    private static partial Regex NonBreakingSpacePlaceholderRegex();
}