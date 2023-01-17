using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal class EasProductForm : EasForm
{
    public EasProductForm(ProductFormData formData) : base(formData)
    {
    }

    public override IEnumerable<Form> BuildForms(IEnumerable<EasFormSourceField> sourceFields, IEnumerable<DynamicFormValues> dynamicFormValues)
    {
        var dynamicFromValuesEnumerator = dynamicFormValues.GetEnumerator();

        return sourceFields.GroupBy(f => f.FormType)
                           .SelectMany(group => CreateForms(group.Key, group.AsEnumerable(), GetDynamicFormValues(dynamicFromValuesEnumerator)));
    }

    private IEnumerable<Form> CreateForms(EasFormType type, IEnumerable<EasFormSourceField> sourceFields, DynamicFormValues? dynamicFormValues)
    {
        if (type == EasFormType.F3602)
            return CreateF3602Form(sourceFields.ToList(), dynamicFormValues);

        return new[]
        {
            new Form
            {
                EasFormType = type,
                DynamicFormValues = dynamicFormValues,
                Json = CreateJson(sourceFields)
            }
        };
    }

    private IEnumerable<Form> CreateF3602Form(ICollection<EasFormSourceField> sourceFields, DynamicFormValues? dynamicFormValues)
    {
        var householdTypes = new[] { HouseholdTypes.Codebtor, HouseholdTypes.Garantor };

        foreach (var householdType in householdTypes)
        {
            if (!((ProductFormData)FormData).HouseholdData.TrySetHousehold(householdType))
                continue;

            yield return new Form
            {
                EasFormType = EasFormType.F3602,
                DynamicFormValues = dynamicFormValues,
                Json = CreateJson(sourceFields)
            };
        }
    }
}