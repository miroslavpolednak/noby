using System.Text.Json;
using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;
using CIS.InternalServices.DocumentDataAggregator.EasForms.Json.Data;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

internal class EasForm<TData> : IEasForm<TData> where TData : notnull
{
    private readonly IReadOnlyCollection<EasFormSourceField> _sourceFields;

    public EasForm(TData formData, IReadOnlyCollection<EasFormSourceField> sourceFields)
    {
        _sourceFields = sourceFields;
        FormData = formData;
    }

    public TData FormData { get; }

    public ICollection<Form> BuildForms() =>
        _sourceFields.GroupBy(f => f.FormType)
                     .Select(CreateForm)
                     .ToList();

    private Form CreateForm(IGrouping<EasFormType, EasFormSourceField> sourceFieldGroups)
    {
        var jsonObject = CreateJsonObject(sourceFieldGroups);

        var json = JsonSerializer.Serialize(jsonObject.GetJsonObject(FormData), new JsonSerializerOptions { WriteIndented = true });

        return new Form
        {
            FormType = sourceFieldGroups.Key,
            Json = json
        };
    }

    private static EasFormJsonObject CreateJsonObject(IEnumerable<EasFormSourceField> sourceFields)
    {
        var jsonObject = new EasFormJsonObjectImpl();

        foreach (var sourceField in sourceFields)
            jsonObject.Add(sourceField.JsonPropertyName.Split('.'), sourceField.FieldPath);

        return jsonObject;
    }
}