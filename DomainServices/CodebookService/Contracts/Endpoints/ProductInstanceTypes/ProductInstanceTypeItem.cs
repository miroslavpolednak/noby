using System.Runtime.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes
{
    [DataContract]
    public class ProductInstanceTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public ProductInstanceTypeCategory ProductCategory { get; set; }
    }

    public enum ProductInstanceTypeCategory
    {
        [DataMember(Order = 1)]
        BuildingSavings = 1,

        [DataMember(Order = 2)]
        BuildingSavingsLoan = 2,
        
        [DataMember(Order = 3)]
        Morgage = 3
    }
}
