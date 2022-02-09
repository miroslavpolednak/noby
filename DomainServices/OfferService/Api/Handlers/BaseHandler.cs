using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace DomainServices.OfferService.Api.Handlers;

internal class BaseHandler
{

    #region Construction

    protected readonly Repositories.OfferRepository _repository;

    private readonly ICodebookServiceAbstraction _codebookService;

    public BaseHandler(
        Repositories.OfferRepository repository,
        ICodebookServiceAbstraction codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    #endregion
        
    /// <summary>
    /// Checks if ProductTypeId matches ProductTypeCategory (Hypo, SS, ...)
    /// </summary>
    protected async Task CheckProductTypeCategory(long id, ProductTypeCategory category)
    {
        var list = await _codebookService.ProductTypes();
        var item = list.FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductTypeItem), id);
        }

        if (item.ProductCategory != category)
        {
            throw new CisArgumentException(1, $"ProductTypeId '{id}' doesn't match ProductTypeCategory '{category}'.", "ProductTypeId");
        }
    }

}
