using DomainServices.ProductService.Clients;
using NOBY.Api.Endpoints.Product.GetProductObligationList.Dto;
using NOBY.Api.SharedDto;
using Contracts = DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.Product.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, List<ProductObligation>>
{
    private readonly IProductServiceClient _productService;

    public GetProductObligationListHandler(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public async Task<List<ProductObligation>> Handle(GetProductObligationListRequest request, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductObligationList(new Contracts.GetProductObligationListRequest { ProductId = request.ProductId }, cancellationToken);

        return response.ProductObligations
                       .Select(p => new ProductObligation
                       {
                           ObligationTypeId = p.ObligationTypeId,
                           ProductObligationId = new ProductObligationId
                           {
                               LoanId = p.ProductObligationId.LoanId,
                               LoanPurpose = p.ProductObligationId.LoanPurpose
                           },
                           Amount = p.Amount,
                           CreditorName = p.CreditorName,
                           PaymentAccount = new BankAccount
                           {
                               Number = p.PaymentAccount?.Number,
                               Prefix = p.PaymentAccount?.Prefix,
                               BankCode = p.PaymentAccount?.BankCode
                           },
                           PaymentSymbols = new PaymentSymbols
                           {
                               VariableSymbol = p.PaymentSymbols?.VariableSymbol
                           },

                       })
                       .ToList();
    }
}