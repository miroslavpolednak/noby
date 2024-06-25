using System.Runtime.CompilerServices;
using ceTe.DynamicPDF.Text;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Extensions;

public static class FontHelper
{
    public static OpenTypeFont ParseOpenTypeFont(this Font font, [CallerMemberName] string cacheKey = "")
    {
        if (font is OpenTypeFont typeFont)
            return typeFont;

        var splitName = font.Name.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var cachedFont = splitName switch
        {
            ["Arial"] => GeneratorVariables.Arial,
            ["Arial", "Bold"] => GeneratorVariables.ArialBold,
            ["Arial", "Italic"] => GeneratorVariables.ArialItalic,
            ["Arial", "BoldItalic"] => GeneratorVariables.ArialBoldItalic,
            _ => throw new NotImplementedException($"Unknown font name - {font.Name}")
        };

        return cachedFont.GetFont(cacheKey);
    }

    public static OpenTypeFont ParseOpenTypeFont(string fontName, [CallerMemberName] string cacheKey = "")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fontName);

        var splitName = fontName.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var cachedFont = splitName switch
        {
            ["Arial"] => GeneratorVariables.Arial,
            ["Arial", "Bold"] => GeneratorVariables.ArialBold,
            ["Arial", "Italic"] => GeneratorVariables.ArialItalic,
            ["Arial", "BoldItalic"] => GeneratorVariables.ArialBoldItalic,
            _ => throw new NotImplementedException($"Unknown font name - {fontName}")
        };

        return cachedFont.GetFont(cacheKey);
    }
}