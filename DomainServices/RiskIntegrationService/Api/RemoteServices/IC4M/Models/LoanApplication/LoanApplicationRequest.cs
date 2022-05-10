using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// LoanApplication
    /// </summary>
    public class LoanApplicationRequest
    {
        /// <summary>
        /// LoanApplication identity
        /// </summary>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// rozlišení nového obchodu a typu dodatku
        /// </summary>
        public long? AppendixCode { get; set; }

        /// <summary>
        /// distribuční kanál vzniku žádosti
        /// </summary>
        public string DistributionChannelCode { get; set; }

        /// <summary>
        /// Způsob podpisu žádosti
        /// </summary>
        public string SignatureType { get; set; }

        /// <summary>
        /// loan application verze
        /// </summary>
        public string LoanApplicationDataVersion { get; set; }

        /// <summary>
        /// pouze CREATOR
        /// </summary>
        public KBGroupPerson Person { get; set; }

        /// <summary>
        /// aplikačních data jednotlivých domácností (včetně ručitelských)
        /// </summary>
        public List<LoanApplicationHousehold> LoanApplicationHousehold { get; set; }

        /// <summary>
        /// parametrů jednotlivých účtů balíčku
        /// </summary>
        public LoanApplicationProduct LoanApplicationProduct { get; set; }

        /// <summary>
        /// konsolidované/zrušené úvěry 
        /// </summary>
        public List<LoanApplicationProductRelation> LoanApplicationProductRelation { get; set; }

        /// <summary>
        /// LoanApplicationDealer
        /// </summary>
        public LoanApplicationDealer LoanApplicationDealer { get; set; }
    }
}
