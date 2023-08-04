using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ceTe.DynamicPDF.Text;

namespace CIS.InternalServices.DocumentGeneratorService.Api;

public static class GeneratorVariables
{
    private const string FontsFolder = "Fonts";

    public static string StoragePath { get; private set; } = null!;

    public static OutputIntent ColorScheme { get; private set; } = null!;

    public static CachedFont Arial { get; private set; } = null!;

    public static CachedFont ArialBold { get; private set; } = null!;

    public static CachedFont ArialItalic { get; private set; } = null!;

    public static CachedFont ArialBoldItalic { get; private set; } = null!;

    public static void Init(GeneratorConfiguration config)
    {
        StoragePath = config.StoragePath;

        SetColorScheme(StoragePath);

        Arial = new CachedFont(() => GetOpenTypeFont("arial"));
        ArialBold = new CachedFont(() => GetOpenTypeFont("arial_bold"));
        ArialItalic = new CachedFont(() => GetOpenTypeFont("arial_italic"));
        ArialBoldItalic = new CachedFont(() => GetOpenTypeFont("arial_bold_italic"));
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

    public class CachedFont
    {
        private readonly Func<OpenTypeFont> _fontFactory;
        private readonly ConcurrentDictionary<string, OpenTypeFont> _fonts = new(Environment.ProcessorCount, 2);

        public CachedFont(Func<OpenTypeFont> fontFactory)
        {
            _fontFactory = fontFactory;
        }

        public OpenTypeFont GetFont([CallerMemberName] string cacheKey = "") => _fonts.GetOrAdd(cacheKey, _ => _fontFactory());
    }
}