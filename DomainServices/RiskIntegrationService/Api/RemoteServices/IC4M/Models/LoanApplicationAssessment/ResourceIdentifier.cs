namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// ResourceIdentifier
    /// </summary>
    public class ResourceIdentifier
    {
        /// <summary>
        /// The resource instance code, eg. 'KBCZ'
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// The resource domain code, eg. 'CFLM'
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The resource code (the in-domain resource code, eg. 'LimitModel')
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The variant of the resource, eg. distinguishing the origin of the source
        /// </summary>
        public string Variant { get; set; }
    }
}
