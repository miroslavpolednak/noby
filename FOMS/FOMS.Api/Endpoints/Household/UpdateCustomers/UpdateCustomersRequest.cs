using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

public class UpdateCustomersRequest
    : IRequest<UpdateCustomersResponse>
{
    [JsonIgnore]
    internal int HouseholdId;

    /// <summary>
    /// ID hlavniho frajera v domacnosti
    /// </summary>
    public Customer? Customer1 { get; set; }

    /// <summary>
    /// ID spoludluznika
    /// </summary>
    public Customer? Customer2 { get; set; }

    internal UpdateCustomersRequest InfuseId(int householdId)
    {
        this.HouseholdId = householdId;
        return this;
    }

    public class Customer : Services.CreateCustomer.IClientInfo
    {
        /// <summary>
        /// ID entity pokud jiz existuje
        /// </summary>
        public int? CustomerOnSAId { get; set; }

        /// <summary>
        /// Identita klienta, pokud se ma zalozit novy CustomerOnSA
        /// </summary>
        public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }

        /// <summary>
        /// Jmeno klienta, pokud se ma zalozit novy CustomerOnSA
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Prijmeni klienta, pokud se ma zalozit novy CustomerOnSA
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Datum narozeni klienta, pokud se ma zalozit novy CustomerOnSA
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
    }
}
