using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.Forms;

internal interface IEasForm
{
    AggregatedData AggregatedData { get; }

    IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields);

    void SetFormResponseSpecificData(GetEasFormResponse response);
}