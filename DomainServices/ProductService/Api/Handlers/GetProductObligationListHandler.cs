using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetProductObligationListHandler
    : IRequestHandler<Dto.GetProductObligationListMediatrRequest, GetProductObligationListResponse>
{
    public async Task<GetProductObligationListResponse> Handle(
        Dto.GetProductObligationListMediatrRequest request,
        CancellationToken cancellation)
    {
        // todo: replace Mock with KonsDB https://jira.kb.cz/browse/HFICH-2867
        await Task.Delay(TimeSpan.FromMilliseconds(10), cancellation);

        // Mock Data
        var productObligations = Enumerable
            .Range(1, 3)
            .Select(i => new ProductObligation
            {
                ProductObligationId = i,
                ObligationTypeId = i,
                Amount = new GrpcDecimal(67000, 0),
                PaymentSymbols = new PaymentSymbols { VariableSymbol = "750504444" },
                CreditorName = "Komerční banka",
                PaymentAccount = new PaymentAccount
                {
                    Prefix = "107",
                    Number = "71286260207",
                    BankCode = "0100"
                }
            })
            .ToList();
        
        return new GetProductObligationListResponse
        {
            ProductObligations =
            {
                productObligations[0],
                productObligations[1],
                productObligations[2],
                productObligations[0],
                productObligations[1],
                productObligations[2],
            }
        };
    }
}