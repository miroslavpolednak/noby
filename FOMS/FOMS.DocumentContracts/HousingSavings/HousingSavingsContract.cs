namespace FOMS.DocumentContracts.HousingSavings;

public sealed partial class HousingSavingsContract : BaseContract
{
    public SharedModels.CustomerDetail? Customer { get; set; }
    public SharedModels.Citizenship? Citizenship { get; set; }
    public Models.FinancesSection? Finaces { get; set; }
    public SharedModels.SZKU? SZKU { get; set; }
    public TaxResidency.TaxResidencyContract? TaxResidency { get; set; }
}

//TODO udelat do source generator
public sealed partial class HousingSavingsContract
{
    public object GetPart(int partId)
        => partId switch
        {
            1 => new HousingSavingsPart1
            {
                Citizenship = Citizenship,
                Customer = Customer,
                SZKU = SZKU,
                TaxResidency = TaxResidency
            },
            2 => new HousingSavingsPart2
            {
                Finances = Finaces
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
            case 2:
                MergePart(castObjectToPart<HousingSavingsPart2>(data));
                break;

            default:
                throw new NotImplementedException($"Document Part {partId} has not been implemented");
        }
    }

    private void MergePart(HousingSavingsPart1 partData)
    {
        Customer = partData.Customer;
        Citizenship = partData.Citizenship;
        SZKU = partData.SZKU;
        TaxResidency= partData.TaxResidency;
    }

    private void MergePart(HousingSavingsPart2 partData)
    {
        Finaces = partData.Finances;
    }
}