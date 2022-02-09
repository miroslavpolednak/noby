using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductLoanPurposes
{
    [DataContract]
    public sealed class ProductLoanPurposesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public CIS.Core.Enums.Mandants Mandant { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}
