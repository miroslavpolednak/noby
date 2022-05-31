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
    protected async Task CheckProductTypeCategory(int id, ProductTypeCategory category, CancellationToken cancellation)
    {
        var list = await _codebookService.ProductTypes(cancellation);
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

    /// <summary>
    /// Searchs for default 'PaymentDay' value
    /// </summary>
    protected async Task<int> GetDefaultPaymentDay(CancellationToken cancellation)
    {
        var list = await _codebookService.PaymentDays(cancellation);

        var itemDefault = list.FirstOrDefault(i=>i.IsDefault);

        if (itemDefault == null)
        {
            throw new CisNotFoundException(99999, $"Default 'PaymentDay' not found.");
        }

        return itemDefault.PaymentDay;
    }

}
