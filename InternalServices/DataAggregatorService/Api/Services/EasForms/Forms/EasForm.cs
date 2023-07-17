using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Json;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

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
        var jsonObject = CreateJsonObject(sourceFieldGroups).GetJsonObject(_formData);

        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(jsonObject, jsonOptions);
    }

    private static EasFormJsonObject CreateJsonObject(IEnumerable<EasFormSourceField> sourceFields)
    {
        var jsonObject = new EasFormJsonObjectImpl();

        foreach (var sourceField in sourceFields)
            jsonObject.Add(sourceField.JsonPropertyName.Split('.'), sourceField.FieldPath);

        return jsonObject;
    }
}