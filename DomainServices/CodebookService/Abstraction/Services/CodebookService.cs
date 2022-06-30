using DomainServices.CodebookService.Contracts;

namespace DomainServices.CodebookService.Abstraction;

internal partial class CodebookService : ICodebookServiceAbstraction
{
    private readonly ICodebookService _codebookService;
    private readonly AbstractionMemoryCache _cache;

    public CodebookService(ICodebookService codebookService, AbstractionMemoryCache cache)
    {
        _cache = cache;
        _codebookService = codebookService;
    }
}
