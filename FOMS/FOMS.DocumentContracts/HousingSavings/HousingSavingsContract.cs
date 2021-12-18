namespace FOMS.DocumentContracts.HousingSavings;

public class HousingSavingsContract : BaseContract
{
    public SharedModels.CustomerDetail? Customer { get; set; }

    public SharedModels.Citizenship? Citizenship { get; set; }

    public object GetPart(int partId)
        => partId switch
        {
            1 => new HousingSavingsPart1
            {
                Citizenship = Citizenship,
                Customer = Customer
            },

            _ => throw new NotImplementedException($"Document Part {partId} has not been implemented")
        };
    
    public void MergePart(int partId, object data)
    {
        switch (partId)
        {
            case 1:
                MergePart(castObjectToPart<HousingSavingsPart1>(data));
                break;

            default:
                throw new NotImplementedException($"Document Part {partId} has not been implemented");
        }
    }

    private void MergePart(HousingSavingsPart1 partData)
    {
        Customer = partData.Customer;
        Citizenship = partData.Citizenship;
    }
}
