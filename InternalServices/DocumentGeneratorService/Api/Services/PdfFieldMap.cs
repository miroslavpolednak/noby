using System.Text.Json;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

public class PdfFieldMap
{
    private readonly Dictionary<string, Dictionary<string, DocumentMapItem>> _pdfMap;

    private PdfFieldMap(Dictionary<string, Dictionary<string, DocumentMapItem>> pdfMap)
    {
        _pdfMap = pdfMap;
    }

    public static PdfFieldMap CreateInstance() => new(CreatePdfMap());

    public IReadOnlyDictionary<string, DocumentMapItem> GetPdfMap(string templateName, string templateVersion, string? templateVariant)
    {
        var pdfName = $"{templateName}_{templateVersion}{templateVariant}";

        return _pdfMap[pdfName];
    }

    private static Dictionary<string, Dictionary<string, DocumentMapItem>> CreatePdfMap()
    {
        var map = new Dictionary<string, Dictionary<string, DocumentMapItem>>();
        var jsonMapPaths = Directory.GetFiles(GeneratorVariables.StoragePath, "*.json", SearchOption.AllDirectories);

        foreach (var jsonMapPath in jsonMapPaths)
        {
            var pdfMap = JsonSerializer.Deserialize<List<DocumentMapItem>>(File.ReadAllText(jsonMapPath));

            if (pdfMap is null)
                continue;

            map.Add(Path.GetFileNameWithoutExtension(jsonMapPath), pdfMap.ToDictionary(k => k.Field.Name));
        }

        return map;
    }
}