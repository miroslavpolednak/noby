using System.Text.Json.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// Create request
    /// </summary>
    public class CreateRequest
    {
        /// <summary>
        /// Identifikátor LoanApplication.
        /// </summary>
        public ResourceIdentifier LoanApplicationId { get; set; }

        /// <summary>
        /// ID business procesu v rámci kterého Risk Business Case vzniká.
        /// </summary>
        //[JsonIgnore]
        public ResourceIdentifier ResourceProcessId { get; set; }
        
        /// <summary>
        /// Typ zdrojové aplikace (např. NOBY)
        /// </summary>
        /// <value>Typ zdrojové aplikace (např. NOBY)</value>
        public string ItChannel { get; set; }
    }
}
