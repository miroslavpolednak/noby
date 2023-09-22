using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.Forms;

internal abstract class EasForm<TFormData> : IEasForm where TFormData : AggregatedData
{
    protected readonly TFormData _formData;

    protected EasForm(TFormData formData)
    {
        _formData = formData;
    }

    public AggregatedData AggregatedData => _formData;

    public abstract IEnumerable<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields);

    public abstract void SetFormResponseSpecificData(GetEasFormResponse response);

    protected string CreateJson(IEnumerable<EasFormSourceField> sourceFieldGroups)
    {
        var jsonObject = new JsonBuilder<EasFormJsonValueSource>();

        foreach (var sourceField in sourceFieldGroups) 
            jsonObject.Add(sourceField.JsonPropertyName, sourceField);

        return jsonObject.Serialize(_formData);
    }

    protected string CalculateFormIdentifier(char prefix, EasFormType formType, long numberIdentifier) => $"{prefix}{formType}{numberIdentifier}";
}