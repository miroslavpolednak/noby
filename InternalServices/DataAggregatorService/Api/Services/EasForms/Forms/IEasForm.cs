using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal interface IEasForm
{
    AggregatedData AggregatedData { get; }

    IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields);

    void SetFormResponseSpecificData(GetEasFormResponse response);
}