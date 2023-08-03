using ceTe.DynamicPDF.Text;

namespace CIS.InternalServices.DocumentGeneratorService.Api;

public static class GeneratorVariables
{
    private const string FontsFolder = "Fonts";

    public static string StoragePath { get; private set; } = null!;

    public static OutputIntent ColorScheme { get; private set; } = null!;

    public static OpenTypeFont Helvetica { get; private set; } = null!;

    public static OpenTypeFont HelveticaBold { get; private set; } = null!;

    public static void Init(GeneratorConfiguration config)
    {
        StoragePath = config.StoragePath;

        SetColorScheme(StoragePath);

        Helvetica = new OpenTypeFont(Path.Combine(StoragePath, FontsFolder, "Helvetica.ttf"));
        HelveticaBold = new OpenTypeFont(Path.Combine(StoragePath, FontsFolder, "Helvetica-Bold.ttf"));
    }

    private static void SetColorScheme(string storagePath)
    {
        var iccProfile = new IccProfile(Path.Combine(storagePath, "ICC\\sRGB_IEC61966-2-1.icc"));

        ColorScheme = new OutputIntent("", "sRGB_IEC61966-2-1.icc", "https://www.adobe.com/", "RGB", iccProfile)
        {
            Version = OutputIntentVersion.PDF_A
        };
    }
}