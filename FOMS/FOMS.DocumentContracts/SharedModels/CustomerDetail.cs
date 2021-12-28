namespace FOMS.DocumentContracts.SharedModels;

public class CustomerDetail : PersonalData
{
    public int? Id { get; set; }

    public Address? HomeAddress { get; set; }

    public Contacts? Contacts { get; set; }
}
