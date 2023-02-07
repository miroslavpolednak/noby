using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationListHandler
    : IRequestHandler<Contracts.GetProductObligationListRequest, GetProductObligationListResponse>
{
    public async Task<GetProductObligationListResponse> Handle(
        Contracts.GetProductObligationListRequest request,
        CancellationToken cancellation)
    {
        // todo: replace Mock with KonsDB https://jira.kb.cz/browse/HFICH-2867

        await Task.Delay(TimeSpan.FromMilliseconds(10), cancellation);

        // todo: do not return obligations where:
        // todo: ObligationTypeId is null or
        // todo: Amount is null or
        // todo: CreditorName is null
        // todo: log those obligations
        // Mock Data
        var productObligations = Enumerable
            .Range(1, 3)
            .Select(i => new GetProductObligationItem
            {
                ProductObligationId = i,
                ObligationTypeId = i,
                Amount = new GrpcDecimal(67000, 0),
                CreditorName = "Komerční banka",
                PaymentSymbols = new PaymentSymbols { VariableSymbol = "750504444" },
                PaymentAccount = new PaymentAccount
                {
                    Prefix = "115",
                    Number = "8912354748",
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