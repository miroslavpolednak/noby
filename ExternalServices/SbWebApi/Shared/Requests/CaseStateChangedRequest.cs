namespace ExternalServices.SbWebApi.Shared.Requests;

[DataContract]
internal sealed class CaseStateChangedRequest
{
    [DataMember]
    public HeaderModel header { get; set; }

    [DataMember]
    public MessageModel message { get; set; }

    [DataContract]
    public class HeaderModel
    {
        [DataMember]
        public string system { get; set; } = "NOBY";

        [DataMember]
        public string login { get; set; }
    }

    [DataContract]
    public class MessageModel
    {
        [DataMember]
        public long case_id { get; set; }

        [DataMember]
        public long uver_id { get; set; }

        [DataMember]
        public string? contract_no { get; set; }

        [DataMember]
        public string? jmeno_prijmeni { get; set; }

        [DataMember]
        public string case_state { get; set; } = "";

        [DataMember]
        public int product_type { get; set; }

        [DataMember]
        public string owner_cpm { get; set; } = "";

        [DataMember]
        public string? owner_icp { get; set; }

        [DataMember]
        public int mandant { get; set; } = 2;

        [DataMember]
        public int client_benefits { get; set; } = 0;

        [DataMember]
        public string? risk_business_case_id { get; set; }
    }

    public static explicit operator CaseStateChangedRequest(CaseStateChangedModel model)
        => new CaseStateChangedRequest
        {
            header = new HeaderModel
            {
                login = model.Login
            },
            message = new MessageModel
            {
                case_id = model.CaseId,
                uver_id = model.CaseId,
                contract_no = model.ContractNumber,
                jmeno_prijmeni = model.ClientFullName,
                case_state = model.CaseStateName,
                product_type = model.ProductTypeId,
                owner_cpm = model.OwnerUserCpm,
                owner_icp = model.OwnerUserIcp,
                mandant = (int)model.Mandant,
                risk_business_case_id = model.RiskBusinessCaseId
            }
        };
}
