using DomainServices.CodebookService.Contracts;

namespace DomainServices.CodebookService.Abstraction;

internal partial class CodebookService : ICodebookServiceAbstraction
{
    private readonly ICodebookService _codebookService;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;
    private readonly AbstractionMemoryCache _cache;

    public CodebookService(ICodebookService codebookService, CIS.Security.InternalServices.ICisUserContextHelpers userContext, AbstractionMemoryCache cache)
    {
        _cache = cache;
        _userContext = userContext;
        _codebookService = codebookService;
    }
}
