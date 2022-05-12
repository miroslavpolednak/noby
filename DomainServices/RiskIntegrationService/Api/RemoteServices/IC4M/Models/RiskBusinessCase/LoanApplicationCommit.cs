using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// Response
    /// </summary>
    [DataContract]
    public class LoanApplicationCommit
    {
        /// <summary>
        /// identifikátor obchodního případu v C4M
        /// </summary>
        /// <value>identifikátor obchodního případu v C4M</value>
        public long? RiskBusinessCaseId { get; set; }

        /// <summary>
        /// Datum a čas odpovědi
        /// </summary>
        /// <value>Datum a čas odpovědi</value>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value>Status</value>
        public string OperationResult { get; set; }

        /// <summary>
        /// Důvod(y) nespočetní výsledků bonity
        /// </summary>
        /// <value>Důvod(y) nespočetní výsledků bonity</value>
        public List<ResultReason> ResultReasons { get; set; }
    }
}
