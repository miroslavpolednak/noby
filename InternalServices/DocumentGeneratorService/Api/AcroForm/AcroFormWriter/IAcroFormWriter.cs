using CIS.InternalServices.DocumentGeneratorService.Api.Storage;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

public interface IAcroFormWriter
{
    MergeDocument Write(TemplateLoader templateLoader, string? templateNameModifier = default);
}