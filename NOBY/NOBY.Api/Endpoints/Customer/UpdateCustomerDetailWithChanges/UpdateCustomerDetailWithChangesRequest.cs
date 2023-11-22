using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

public sealed class UpdateCustomerDetailWithChangesRequest
    : Shared.BaseCustomerDetail, Shared.ICustomerDetailContacts, IRequest
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    [EmailAddress]
    public NOBY.Dto.EmailAddressDto? EmailAddress { get; set; }

    public NOBY.Dto.PhoneNumberDto? MobilePhone { get; set; }

    internal CustomerIdentificationObj? CustomerIdentification { get; set; }


    internal UpdateCustomerDetailWithChangesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }

    internal class CustomerIdentificationObj
    {
        public int? IdentificationMethodId { get; set; }

        public DateTime? IdentificationDate { get; set; }

        public string? CzechIdentificationNumber { get; set; }
    }
}
