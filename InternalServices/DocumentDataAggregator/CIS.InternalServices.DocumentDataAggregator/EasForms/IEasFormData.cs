using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public interface IEasFormData
{
    internal EasFormRequestType EasFormRequestType { get; }

    Task LoadAdditionalData();
}