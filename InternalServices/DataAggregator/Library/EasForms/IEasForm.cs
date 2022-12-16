namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IEasForm<out TData>
{
    TData FormData { get; }

    ICollection<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues);
}