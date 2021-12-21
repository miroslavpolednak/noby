namespace FOMS.DocumentContracts.TaxResidency;

public sealed partial class TaxResidencyContract : BaseContract
{
    public Models.MainSection? Form { get; set; }
}

public sealed partial class TaxResidencyContract
{
    public object GetPart(int partId)
        => partId switch
        {
            1 => new TaxResidencyPart1
            {
                Form = Form,
            },
            _ => throw new NotImplementedException($"Document Part {partId} has not been implemented")
        };

    public void MergePart(int partId, object data)
    {
        switch (partId)
        {
            case 1:
                MergePart(castObjectToPart<TaxResidencyPart1>(data));
                break;
            default:
                throw new NotImplementedException($"Document Part {partId} has not been implemented");
        }
    }

    private void MergePart(TaxResidencyPart1 partData)
    {
        Form = partData.Form;
    }
}