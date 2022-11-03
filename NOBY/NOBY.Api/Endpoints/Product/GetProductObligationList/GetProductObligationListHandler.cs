using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using NOBY.Api.SharedDto;
using Contracts = DomainServices.ProductService.Contracts;
using PaymentSymbols = NOBY.Api.SharedDto.PaymentSymbols;

namespace NOBY.Api.Endpoints.Product.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, List<Dto.ProductObligation>>
{
    private readonly IProductServiceClient _productService;

    public GetProductObligationListHandler(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public async Task<List<Dto.ProductObligation>> Handle(
        GetProductObligationListRequest request,
        CancellationToken cancellationToken)
    {
        var response = ServiceCallResult.ResolveAndThrowIfError<GetProductObligationListResponse>(
            await _productService.GetProductObligationList(
                new Contracts.GetProductObligationListRequest { ProductId =  request.ProductId }, cancellationToken));

        return response.ProductObligations
            .Select(p => new Dto.ProductObligation
            {
                ObligationTypeId = p.ObligationTypeId,
                ProductObligationId = p.ProductObligationId,
                Amount = p.Amount,
                CreditorName = p.CreditorName,
                PaymentAccount = new BankAccount
                {
                    Number = p.PaymentAccount?.Number,
                    Prefix = p.PaymentAccount?.Prefix,
                    BankCode = p.PaymentAccount?.BankCode
                },
                PaymentSymbols = new SharedDto.PaymentSymbols
                {
                    ConstantSymbol = p.PaymentSymbols?.ConstantSymbol,
                    SpecificSymbol = p.PaymentSymbols?.SpecificSymbol,
                    VariableSymbol = p.PaymentSymbols?.VariableSymbol
                },
                
            })
            .ToList();
    }
}