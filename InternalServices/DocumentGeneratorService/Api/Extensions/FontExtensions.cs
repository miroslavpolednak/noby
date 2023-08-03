using ceTe.DynamicPDF.Text;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Extensions;

public static class FontExtensions
{
    public static OpenTypeFont ParseOpenTypeFont(this Font font)
    {
        if (font is OpenTypeFont typeFont)
            return typeFont;

        var splitName = font.Name.Split(',', StringSplitOptions.RemoveEmptyEntries);

        return splitName switch
        {
            ["Arial"] => GeneratorVariables.Arial,
            ["Arial", "Bold"] => GeneratorVariables.ArialBold,
            ["Arial", "Italic"] => GeneratorVariables.ArialItalic,
            ["Arial", "BoldItalic"] => GeneratorVariables.ArialBoldItalic,
            _ => throw new NotImplementedException($"Unknown font name - {font.Name}")
        };
    }
}