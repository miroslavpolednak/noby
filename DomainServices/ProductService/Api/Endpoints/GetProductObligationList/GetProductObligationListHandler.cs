namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationListHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetProductObligationListRequest, GetProductObligationListResponse>
{
    public async Task<GetProductObligationListResponse> Handle(GetProductObligationListRequest request, CancellationToken cancellationToken)
    {
        var product = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken);

        var responseItems = obligations.Select(obligation =>
        {
            var item = new GetProductObligationItem
            {
                ObligationTypeId = obligation.ObligationTypeId,
                Amount = obligation.Amount,
                CreditorName = obligation.CreditorName
            };

            if (!string.IsNullOrWhiteSpace(obligation.AccountNumber))
            {
                item.PaymentAccount = new PaymentAccount
                {
                    Prefix = obligation.AccountNumberPrefix ?? string.Empty,
                    Number = obligation.AccountNumber ?? string.Empty,
                    BankCode = obligation.BankCode ?? string.Empty
                };
            }

            if (!string.IsNullOrWhiteSpace(obligation.VariableSymbol))
                item.PaymentSymbols = new PaymentSymbols { VariableSymbol = obligation.VariableSymbol };

            return item;
        });

        return new GetProductObligationListResponse 
        { 
            ProductObligations = { responseItems } 
        };
    }
}