namespace ExternalServices.SbWebApi.Shared.Responses;

[DataContract]
internal class CaseStateChangedResponse
{
    [DataMember]
    public int request_id { get; set; }

    [DataMember]
    public ResultModel? result { get; set; }

    [DataContract]
    public class ResultModel
    {
        [DataMember]
        public int return_val { get; set; }

        [DataMember]
        public string? return_text { get; set; }
    }
}
