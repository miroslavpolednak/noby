using DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp.Dto;

internal class CustomersData
{
    public required ICollection<CustomerOnSaExtended> CustomersOnSa { get; init; }

    public required ICollection<GetCustomersOnProductResponseItem> RedundantCustomersOnProduct { get; init; }
}