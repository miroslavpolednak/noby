namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// Application rating podle ID (request)
    /// </summary>
    
    public class RiskBusinessCaseAssessmentRequest
    {
        /// <summary>
        /// Identifikátor obchodního případu.
        /// </summary>
        /// <value>Identifikátor obchodního případu.</value>
    
        public long? IdBusinessCase { get; set; }

        /// <summary>
        /// Jednoznačný identifikátor žádosti o založení Business Case.
        /// </summary>
        /// <value>Jednoznačný identifikátor žádosti o založení Business Case.</value>
    
        public long? IdRequest { get; set; }

    
    }
}
