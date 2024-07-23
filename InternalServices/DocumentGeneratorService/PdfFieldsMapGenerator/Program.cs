using System.Text.Json;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.Merger;
using CIS.InternalServices.DocumentGeneratorService.PdfFieldsMapGenerator;
using CommandLine;

Document.AddLicense("DPS10NEDLDCHHBkifnavglbvMUnz6cOsK3rihyH8moPETXqm86GidIy9yKvju+7UztxVoPJRLgKM5MmmDgsKwmDSRjs5hznpB2Lw");

return (int)Parser.Default
      .ParseArguments<MapGeneratorOptions>(args)
      .MapResult(options =>
      {
          try
          {
              return RunGenerator(options);
          }
          catch (Exception)
          {
              return ExitCode.UnknownError;
          }
      }, _ => ExitCode.UnknownError);

static ExitCode RunGenerator(MapGeneratorOptions opts)
{
    ArgumentException.ThrowIfNullOrEmpty(opts.SourcePath);

    if (!Directory.Exists(opts.SourcePath))
        return ExitCode.DirectoryNotExist;

    var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

    foreach (var pdfFilePath in Directory.GetFiles(opts.SourcePath, "*.pdf", SearchOption.AllDirectories))
    {
        var pdfDocument = new PdfDocument(pdfFilePath);
        var mergeDocument = new MergeDocument(pdfDocument);

        var fields = PdfFieldsLoader.Instance.LoadDocumentMap(pdfDocument, mergeDocument).ToList();

        var path = Path.Combine(Path.GetDirectoryName(pdfFilePath)!, $"{Path.GetFileNameWithoutExtension(pdfFilePath)}.json");
        var json = JsonSerializer.Serialize(fields, jsonSerializerOptions);

        File.WriteAllText(path, json);
    }

    return ExitCode.Success;
}