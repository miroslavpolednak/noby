namespace NOBY.ApiContracts;

public partial class CustomerUpdateCustomerDetailWithChangesRequest : IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    [JsonIgnore]
    public CustomerIdentificationObj? CustomerIdentification { get; set; }

    [JsonIgnore]
    public bool IsEmailConfirmed { get; set; }

    [JsonIgnore]
    public bool IsPhoneConfirmed { get; set; }

    public CustomerUpdateCustomerDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }

    public sealed class CustomerIdentificationObj
    {
        public int? IdentificationMethodId { get; set; }

        public DateTime? IdentificationDate { get; set; }

        public string? CzechIdentificationNumber { get; set; }
    }
}