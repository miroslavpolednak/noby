namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

public class DeltaComparerResult
{
    public bool ClientDataWereChanged { get; set; }

    public bool  CrsWasChanged { get; set; }
}