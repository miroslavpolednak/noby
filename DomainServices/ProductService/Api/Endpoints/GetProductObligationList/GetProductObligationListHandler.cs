using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, GetProductObligationListResponse>
{
    private readonly LoanRepository _loanRepository;
    private readonly ProductServiceDbContext _dbContext;

    public GetProductObligationListHandler(LoanRepository loanRepository, ProductServiceDbContext dbContext)
    {
        _loanRepository = loanRepository;
        _dbContext = dbContext;
    }

    public async Task<GetProductObligationListResponse> Handle(GetProductObligationListRequest request, CancellationToken cancellation)
    {
        // check if loan exists (against KonsDB)
        if (!await _loanRepository.ExistsLoan(request.ProductId, cancellation))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);
        }
        
        var obligations = await _dbContext.Obligations.Where(o => o.LoanId == request.ProductId && o.ObligationTypeId != 0 && o.Amount > 0 && o.CreditorName != null).ToListAsync(cancellation);

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