using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Product.GetProductObligations;

internal sealed class GetProductObligationsHandler(IProductServiceClient _productService) 
    : IRequestHandler<GetProductObligationsRequest, List<ProductGetProductObligationListObligation>>
{
    public async Task<List<ProductGetProductObligationListObligation>> Handle(GetProductObligationsRequest request, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductObligationList(new() { ProductId = request.ProductId }, cancellationToken);

        return response.ProductObligations
                       .Select(p => new ProductGetProductObligationListObligation
                       {
                           ObligationTypeId = p.ObligationTypeId,
                           Amount = p.Amount,
                           CreditorName = p.CreditorName,
                           PaymentAccount = new()
                           {
                               AccountNumber = p.PaymentAccount?.Number,
                               AccountPrefix = p.PaymentAccount?.Prefix,
                               AccountBankCode = p.PaymentAccount?.BankCode
                           },
                           PaymentSymbols = new()
                           {
                               VariableSymbol = p.PaymentSymbols?.VariableSymbol
                           },

                       })
                       .ToList();
    }
}