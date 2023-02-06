using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal class EasServiceForm : EasForm
{
    public EasServiceForm(ServiceFormData formData) : base(formData)
    {
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<EasFormSourceField> sourceFields, IEnumerable<DynamicFormValues> dynamicFormValues)
    {
        var dynamicFormValuesEnumerator = dynamicFormValues.GetEnumerator();

        return sourceFields.GroupBy(f => f.FormType).Select(group =>
        {
            var formValues = GetDynamicFormValues(dynamicFormValuesEnumerator);

            ((ServiceFormData)FormData).DynamicFormValues = formValues;

            return new Form
            {
                EasFormType = group.Key,
                DynamicFormValues = formValues,
                Json = CreateJson(group.AsEnumerable())
            };
        });
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = FormData.Case.Data.ContractNumber;
    }
}