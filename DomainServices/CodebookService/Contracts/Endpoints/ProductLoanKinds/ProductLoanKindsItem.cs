using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductLoanKinds
{
    [DataContract]
    public class ProductLoanKindsItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public int ProductTypeId { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public bool IsActual { get; set; }
    }
}
