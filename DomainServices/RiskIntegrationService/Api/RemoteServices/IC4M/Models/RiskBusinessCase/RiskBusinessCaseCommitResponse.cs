using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// Business commit podle ID (response)
    /// </summary>
    [DataContract]
    public class RiskBusinessCaseCommitResponse
    {
        /// <summary>
        /// identifikátor obchodního případu v C4M
        /// </summary>
        /// <value>identifikátor obchodního případu v C4M</value>
        [DataMember(Name = "idBusinessCase", EmitDefaultValue = false)]
        //[JsonProperty(PropertyName = "idBusinessCase")]
        public long? IdBusinessCase { get; set; }

        /// <summary>
        /// Datum a čas odpovědi
        /// </summary>
        /// <value>Datum a čas odpovědi</value>
        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        //[JsonProperty(PropertyName = "timestamp")]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value>Status</value>
        [DataMember(Name = "operationResult", EmitDefaultValue = false)]
        //[JsonProperty(PropertyName = "operationResult")]
        public string OperationResult { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("class CommitResponse {\n");
            sb.Append("  IdBusinessCase: ").Append(IdBusinessCase).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  OperationResult: ").Append(OperationResult).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
