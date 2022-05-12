namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// SemanticVersion
    /// </summary>
    public class SemanticVersion
    {
        /// <summary>
        /// Major part of a version
        /// </summary>
        public int? Major { get; set; }

        /// <summary>
        /// Minor part of a version
        /// </summary>
        public int? Minor { get; set; }

        /// <summary>
        /// Bugfix part of a version
        /// </summary>
        public int? Bugfix { get; set; }

        /// <summary>
        /// A non-semantic part of a version
        /// </summary>
        public string NonSemanticPart { get; set; }
    }
}
