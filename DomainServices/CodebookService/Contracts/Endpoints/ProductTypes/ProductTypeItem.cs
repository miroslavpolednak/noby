using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductTypes
{
    [DataContract]
    public class ProductTypeItem
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
        [DefaultValue(ProductTypeCategory.Unknown)]
        public ProductTypeCategory ProductCategory { get; set; }

        [JsonIgnore]
        [DataMember(Order = 6)]
        public int Order { get; set; }

        [DataMember(Order = 7)]
        public int? LoanAmountInt { get; set; }

        [DataMember(Order = 8)]
        public int? LoanAmountMax { get; set; }

        [DataMember(Order = 9)]
        public int? LoanDurationMin { get; set; }

        [DataMember(Order = 10)]
        public int? LoanDurationMax { get; set; }

        [DataMember(Order = 11)]
        public int? LtvMin { get; set; }

        [DataMember(Order = 12)]
        public int? LtvMax { get; set; }

        [JsonIgnore]
        [DataMember(Order = 13)]
        public string MpHomeApiLoanType { get; set; }

        [JsonIgnore]
        [DataMember(Order = 14)]
        public bool IsValid { get; set; }
    }
}
