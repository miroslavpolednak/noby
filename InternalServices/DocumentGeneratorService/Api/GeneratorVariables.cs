using ceTe.DynamicPDF.Text;

namespace CIS.InternalServices.DocumentGeneratorService.Api;

public static class GeneratorVariables
{
    private const string FontsFolder = "Fonts";

    public static string StoragePath { get; private set; } = null!;

    public static OutputIntent ColorScheme { get; private set; } = null!;

    public static OpenTypeFont Arial { get; private set; } = null!;

    public static OpenTypeFont ArialBold { get; private set; } = null!;

    public static OpenTypeFont ArialItalic { get; private set; } = null!;

    public static OpenTypeFont ArialBoldItalic { get; private set; } = null!;

    public static void Init(GeneratorConfiguration config)
    {
        StoragePath = config.StoragePath;

        SetColorScheme(StoragePath);

        Arial = GetOpenTypeFont("arial");
        ArialBold = GetOpenTypeFont("arial_bold");
        ArialItalic = GetOpenTypeFont("arial_italic");
        ArialBoldItalic = GetOpenTypeFont("arial_bold_italic");
    }

    private static void SetColorScheme(string storagePath)
    {
        var iccProfile = new IccProfile(Path.Combine(storagePath, "ICC\\sRGB_IEC61966-2-1.icc"));

        ColorScheme = new OutputIntent("", "sRGB_IEC61966-2-1.icc", "https://www.adobe.com/", "RGB", iccProfile)
        {
            Version = OutputIntentVersion.PDF_A
        };
    }

    private static OpenTypeFont GetOpenTypeFont(string name) =>
        new(Path.Combine(StoragePath, FontsFolder, $"{name}.ttf"))
        {
            Embed = true,
            Subset = false
        };
}