using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ExtendedObjects;

public abstract class ExtendedObject<TObj> where TObj : class
{
    private CodebookManager? _codebookManager;

    protected CodebookManager CodebookManager
    {
        get
        {
            if (_codebookManager is null)
                throw new InvalidOperationException($"Codebooks are not enabled for {GetType().Name} extended object");

            return _codebookManager;
        }
    }

    public TObj Source { get; set; } = null!;

    public void EnableCodebooks(CodebookManager codebookManager)
    {
        _codebookManager = codebookManager;

        ConfigureCodebooks(codebookManager);
    }

    protected virtual void ConfigureCodebooks(ICodebookManagerConfigurator configure) { }
}