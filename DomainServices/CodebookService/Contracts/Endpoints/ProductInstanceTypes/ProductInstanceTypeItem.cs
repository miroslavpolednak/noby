using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        public string Description { get; set; }

        [DataMember(Order = 4)]
        [DefaultValue(CIS.Core.Mandants.Unknown)]
        public CIS.Core.Mandants Mandant { get; set; }

        [JsonIgnore]
        [DataMember(Order = 5)]
        [DefaultValue(ProductInstanceTypeCategory.Unknown)]
        public ProductInstanceTypeCategory ProductCategory { get; set; }

        [JsonIgnore]
        [DataMember(Order = 6)]
        public string MpHomeLoanType { get; set; }
    }
}
