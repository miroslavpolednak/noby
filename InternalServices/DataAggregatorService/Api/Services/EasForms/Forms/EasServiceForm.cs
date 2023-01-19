using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal class EasServiceForm : EasForm
{
    public EasServiceForm(ProductFormData formData) : base(formData)
    {
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<EasFormSourceField> sourceFields, IEnumerable<DynamicFormValues> dynamicFormValues)
    {
        var dynamicFormValuesEnumerator = dynamicFormValues.GetEnumerator();

        return sourceFields.GroupBy(f => f.FormType).Select(group => new Form
        {
            EasFormType = group.Key,
            DynamicFormValues = GetDynamicFormValues(dynamicFormValuesEnumerator),
            Json = CreateJson(group.AsEnumerable())
        });
    }
}