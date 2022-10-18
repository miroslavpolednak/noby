using DomainServices.ProductService.Abstraction;
using DomainServices.ProductService.Contracts;
using FOMS.Api.SharedDto;
using Contracts = DomainServices.ProductService.Contracts;
using PaymentSymbols = FOMS.Api.SharedDto.PaymentSymbols;

namespace FOMS.Api.Endpoints.Product.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, List<Dto.ProductObligation>>
{
    private readonly IProductServiceAbstraction _productService;

    public GetProductObligationListHandler(IProductServiceAbstraction productService)
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
                PaymentSymbols = new PaymentSymbols
                {
                    ConstantSymbol = p.PaymentSymbols?.ConstantSymbol,
                    SpecificSymbol = p.PaymentSymbols?.SpecificSymbol,
                    VariableSymbol = p.PaymentSymbols?.VariableSymbol
                },
                
            })
            .ToList();
    }
}