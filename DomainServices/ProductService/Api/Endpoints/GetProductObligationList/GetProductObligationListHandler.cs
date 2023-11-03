namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, GetProductObligationListResponse>
{
    private readonly LoanRepository _loanRepository;

    public GetProductObligationListHandler(LoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<GetProductObligationListResponse> Handle(GetProductObligationListRequest request, CancellationToken cancellationToken)
    {
        // check if loan exists (against KonsDB)
        if (!await _loanRepository.LoanExists(request.ProductId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        var obligations = await _loanRepository.GetObligations(request.ProductId, cancellationToken);

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

        return new GetProductObligationListResponse { ProductObligations = { responseItems } };
    }
}