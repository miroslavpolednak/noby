using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Json;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

internal abstract class EasForm
{
    protected EasForm(AggregatedData formData)
    {
        FormData = formData;
    }

    public AggregatedData FormData { get; }
    
    public abstract IEnumerable<Form> BuildForms(IEnumerable<EasFormSourceField> sourceFields, IEnumerable<DynamicFormValues> dynamicFormValues);

    protected DynamicFormValues? GetDynamicFormValues(IEnumerator<DynamicFormValues> enumerator) => enumerator.MoveNext() ? enumerator.Current : default;

    protected string CreateJson(IEnumerable<EasFormSourceField> sourceFieldGroups)
    {
        var jsonObject = CreateJsonObject(sourceFieldGroups).GetJsonObject(FormData);

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
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