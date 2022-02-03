using CIS.Core.Exceptions;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes;


namespace DomainServices.OfferService.Api.Handlers;

internal class BaseHandler
{

    #region Construction

    protected readonly Repositories.OfferInstanceRepository _repository;
    
    private readonly ICodebookServiceAbstraction _codebookService;

    public BaseHandler(
        Repositories.OfferInstanceRepository repository,
        ICodebookServiceAbstraction codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    #endregion


    /// <summary>
    /// Checks if ProductInstanceTypeId matches ProductInstanceTypeCategory (Hypo, SS, ...)
    /// </summary>
    protected async Task checkProductInstanceTypeCategory(long id, ProductInstanceTypeCategory category)
    {
        var list = await _codebookService.ProductInstanceTypes();
        var item = list.FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductInstanceTypeItem), id);
        }

        if (item.ProductCategory != category)
        {
            throw new CisArgumentException(1, $"ProductInstanceTypeId '{id}' doesn't match ProductInstanceTypeCategory '{category}'.", "ProductInstanceTypeId");
        }
    }

}
