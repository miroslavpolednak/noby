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
