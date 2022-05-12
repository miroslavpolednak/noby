using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// Business commit podle ID (request)
    /// </summary>
    /// <value>Business commit podle ID</value>
    
    public class RiskBusinessCaseCommitRequest
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
