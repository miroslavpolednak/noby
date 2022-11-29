using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public class Form
{
    public required EasFormType FormType { get; init; }

    public required DynamicFormValues? DynamicValues { get; init; }

    public required string Json { get; init; }
}