namespace FOMS.Api.Endpoints.Customer.Dto;

internal sealed class SearchResponse
{
    public CIS.Core.Types.PaginableResponse? Pagination { get; set; }
    public List<Customer>? Rows { get; set; }

    public class Customer
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ContractNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
