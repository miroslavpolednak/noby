using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// HouseholdCreditLiabilitiesSummaryOutHomeCompany.
    /// </summary>
    public class CreditLiabilitiesSummary
    {
        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        [DataMember(Name = "productGroup", EmitDefaultValue = false)]
        [JsonPropertyName("productGroup")]
        public string ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public decimal? AmountConsolidated { get; set; }
    }
}
