using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal class EasServiceForm<TFormData> : EasForm<TFormData> where TFormData : AggregatedData
{
    public EasServiceForm(TFormData formData) : base(formData)
    {
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields)
    {
        var dynamicValues = dynamicFormValues.First();

        var easFormType = EasFormTypeFactory.GetEasFormType(dynamicValues.DocumentTypeId);

        yield return new Form
        {
            EasFormType = easFormType,
            DynamicFormValues = dynamicValues,
            DefaultValues = DefaultValuesFactory.Create(easFormType),
            Json = CreateJson(sourceFields)
        };
    }

    public override void SetFormResponseSpecificData(GetEasFormResponse response)
    {
        response.ContractNumber = _formData.Case.Data.ContractNumber;
    }
}