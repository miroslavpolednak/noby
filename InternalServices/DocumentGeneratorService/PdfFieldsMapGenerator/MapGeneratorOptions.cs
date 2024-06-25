using CommandLine;

namespace CIS.InternalServices.DocumentGeneratorService.PdfFieldsMapGenerator;

[Verb("generate")]
public class MapGeneratorOptions
{
    [Option('s', "source", Required = true, HelpText = "Source path to a directory with PDFs")]
    public string? SourcePath { get; set; }
}