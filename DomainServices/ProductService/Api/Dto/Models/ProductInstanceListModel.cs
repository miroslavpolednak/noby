namespace DomainServices.ProductService.Api.Dto;

internal sealed class ProductInstanceListModel
{
    public long ProductInstanceId { get; set; }
    public int ProductInstanceType { get; set; }
    public int State { get; set; }

    public Contracts.ProductInstanceListItem CreateContractItem(List<DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeItem> productTypes)
    {
        return new Contracts.ProductInstanceListItem
        {
            ProductInstanceId = this.ProductInstanceType,
            ProductInstanceType = this.ProductInstanceType,
            ProductInstanceState = this.State,
            ProductInstanceStateName = "",
            ProductInstanceTypeName = productTypes.First(t => t.Id == this.ProductInstanceType).Name
        };
    }
}
