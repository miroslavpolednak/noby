using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;
using CIS.InternalServices.DocumentDataAggregator.EasForms.Json;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

internal class EasForm<TData> : IEasForm<TData> where TData : IEasFormData
{
    private readonly IReadOnlyCollection<EasFormSourceField> _sourceFields;

    private IEnumerator<DynamicFormValues> _dynamicFormValues = Enumerable.Empty<DynamicFormValues>().GetEnumerator();

    public EasForm(TData formData, IReadOnlyCollection<EasFormSourceField> sourceFields)
    {
        _sourceFields = sourceFields;
        FormData = formData;
    }

    public TData FormData { get; }

    public ICollection<Form> BuildForms(IEnumerable<DynamicFormValues> dynamicFormValues)
    {
        _dynamicFormValues = dynamicFormValues.GetEnumerator();

        return _sourceFields.GroupBy(f => f.FormType)
                            .SelectMany(group => CreateForms(group.Key, group))
                            .ToList();
    }

    protected DynamicFormValues? GetDynamicFormValues() => _dynamicFormValues.MoveNext() ? _dynamicFormValues.Current : default;

    protected virtual IEnumerable<Form> CreateForms(EasFormType type, IEnumerable<EasFormSourceField> sourceFields)
    {
        return new[] { CreateForm(type, GetDynamicFormValues(), sourceFields) };
    }

    protected Form CreateForm(EasFormType type, DynamicFormValues? dynamicFormValues, IEnumerable<EasFormSourceField> sourceFields)
    {
        return new Form
        {
            FormType = type,
            DynamicValues = dynamicFormValues,
            Json = CreateJson(sourceFields)
        };
    }

    private string CreateJson(IEnumerable<EasFormSourceField> sourceFieldGroups)
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