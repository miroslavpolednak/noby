using CIS.InternalServices.DataAggregator.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregator.EasForms;

public interface IEasFormData
{
    internal EasFormRequestType EasFormRequestType { get; }

    Task LoadAdditionalData();
}