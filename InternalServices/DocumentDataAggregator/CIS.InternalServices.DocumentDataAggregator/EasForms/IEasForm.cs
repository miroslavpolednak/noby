namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public interface IEasForm<out TData>
{
    TData FormData { get; }

    ICollection<Form> BuildForms();
}