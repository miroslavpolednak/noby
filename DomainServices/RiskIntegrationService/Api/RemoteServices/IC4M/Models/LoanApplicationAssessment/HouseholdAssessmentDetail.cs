namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// Household Assessment detail
    /// </summary>
    public class HouseholdAssessmentDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Household id
        /// </summary>
        public long? HouseholdId { get; set; }

        /// <summary>
        /// Household index
        /// </summary>
        public long? HouseholdIndex { get; set; }

        /// <summary>
        /// Assessment Detail
        /// </summary>
        public AssessmentDetail AssessmentDetail { get; set; }
    }
}
