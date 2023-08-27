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
        var jsonObject = new JsonObject();

        foreach (var sourceField in sourceFieldGroups)
        {
            var jsonValueSource = EasFormJsonValueSource.Create(sourceField);

            jsonObject.Add(sourceField.JsonPropertyName, jsonValueSource);
        }

        return jsonObject.Serialize(_formData);
    }
}