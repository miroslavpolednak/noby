namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationListHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetProductObligationListRequest, GetProductObligationListResponse>
{
    public async Task<GetProductObligationListResponse> Handle(GetProductObligationListRequest request, CancellationToken cancellationToken)
    {
        var product = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken);

        if (product.Inactive)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001);
        }

        var responseItems = product?
            .Obligations?
            .Where(obligation => obligation.ObligationType != 0 && obligation.Amount > 0 && !string.IsNullOrWhiteSpace(obligation.Creditor))
            .Select(obligation =>
            {
                var item = new GetProductObligationItem
                {
                    ObligationTypeId = obligation.ObligationType,
                    Amount = Convert.ToDecimal(obligation.Amount),
                    CreditorName = obligation.Creditor
                };

                if (!string.IsNullOrWhiteSpace(obligation.AccountNumber))
                {
                    item.PaymentAccount = new PaymentAccount
                    {
                        Prefix = obligation.AccountPrefix ?? string.Empty,
                        Number = obligation.AccountNumber ?? string.Empty,
                        BankCode = obligation.BankCode ?? string.Empty
                    };
                }

                if (!string.IsNullOrWhiteSpace(obligation.VariableSymbol))
                {
					item.PaymentSymbols = new PaymentSymbols 
                    { 
                        VariableSymbol = obligation.VariableSymbol 
                    };
				}
                
                return item;
            });

        return new GetProductObligationListResponse 
        { 
            ProductObligations = { responseItems } 
        };
    }
}