namespace DomainServices.ProductService.Api.Dto;

internal sealed class ProductInstanceListModel
{
    public long ProductInstanceId { get; set; }
    public int ProductInstanceTypeId { get; set; }
    public int State { get; set; }

    public Contracts.ProductInstanceListItem CreateContractItem(List<DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem> productTypes)
    {
        return new Contracts.ProductInstanceListItem
        {
            ProductInstanceId = this.ProductInstanceTypeId,
            ProductInstanceTypeId = this.ProductInstanceTypeId,
            ProductInstanceState = this.State,
            ProductInstanceStateName = "",
            ProductInstanceTypeName = productTypes.First(t => t.Id == this.ProductInstanceTypeId).Name
        };
    }
}
