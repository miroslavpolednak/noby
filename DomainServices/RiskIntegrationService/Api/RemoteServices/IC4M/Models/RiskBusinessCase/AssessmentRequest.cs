namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// AssessmentRequest
    /// </summary>
    public class AssessmentRequest
    {
        /// <summary>
        /// ID dané úvěrové žádosti.
        /// </summary>
        public ResourceIdentifier LoanApplicationId { get; set; }

        /// <summary>
        /// IT Channel.
        /// </summary>
        public string ItChannel { get; set; }

        /// <summary>
        /// IT Channel Previous.
        /// </summary>
        public string itChannelPrevious { get; set; }
        
        /// <summary>
        /// Mód vyhodnocení daného případu.
        /// </summary>
        public string AssessmentMode { get; set; }

        /// <summary>
        /// Poskytovací procedura.
        /// </summary>
        public string GrantingProcedureCode { get; set; }

        /// <summary>
        /// Příznak, zdali je požadováno samoschválení.
        /// </summary>
        public bool? SelfApprovalRequired { get; set; }

        /// <summary>
        /// Příznak, zdali je požadováno automatické schválení (systémem).
        /// </summary>
        public bool? SystemApprovalRequired { get; set; }

        /// <summary>
        /// kód risk kampaně.
        /// </summary>
        public string RiskCampaignCode { get; set; }

        /// <summary>
        /// ExceptionHighestApprovalLevel
        /// </summary>
        public string ExceptionHighestApprovalLevel { get; set; }
        
        /// <summary>
        /// Parametry metodické vyjímky.
        /// </summary>
        public List<LoanApplicationException> LoanApplicationException { get; set; }
    }
}
