using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration.EasForm;
using CIS.InternalServices.DataAggregator.EasForms.FormData;

namespace CIS.InternalServices.DataAggregator.EasForms;

internal class ProductEasForm : EasForm<ProductFormData>
{
    public ProductEasForm(ProductFormData formData, IReadOnlyCollection<EasFormSourceField> sourceFields) : base(formData, sourceFields)
    {
    }

    protected override IEnumerable<Form> CreateForms(EasFormType type, IEnumerable<EasFormSourceField> sourceFields)
    {
        if (type == EasFormType.F3602)
            return CreateF3602Form(sourceFields.ToList());

        FormData.DynamicFormValues = GetDynamicFormValues();

        return new[] { CreateForm(type, FormData.DynamicFormValues, sourceFields) };
    }

    private IEnumerable<Form> CreateF3602Form(ICollection<EasFormSourceField> sourceFields)
    {
        var householdTypes = new[] { HouseholdTypes.Codebtor, HouseholdTypes.Garantor };

        foreach (var householdType in householdTypes)
        {
            if (!FormData.InternalHouseholdData.TrySetHousehold(householdType))
                continue;

            FormData.DynamicFormValues = GetDynamicFormValues();

            yield return CreateForm(EasFormType.F3602, FormData.DynamicFormValues, sourceFields);
        }
    }
}