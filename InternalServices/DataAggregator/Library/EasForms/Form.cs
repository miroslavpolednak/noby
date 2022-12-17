using CIS.InternalServices.DataAggregator.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregator.EasForms;

public class Form
{
    public required EasFormType FormType { get; init; }

    public required DynamicFormValues? DynamicValues { get; init; }

    public required string Json { get; init; }
}