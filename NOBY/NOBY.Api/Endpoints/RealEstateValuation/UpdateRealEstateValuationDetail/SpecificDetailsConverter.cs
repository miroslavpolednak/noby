using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

internal class SpecificDetailsConverter : System.Text.Json.Serialization.JsonConverter<ISpecificDetails>
{
    public override ISpecificDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader).RootElement.GetRawText();

        if (string.IsNullOrWhiteSpace(json))
            return default;
        
        var jObject = JObject.Parse(json);

        if (ValidateJson<HouseAndFlatDetails>(jObject))
            return jObject.ToObject<HouseAndFlatDetails>();

        if (ValidateJson<ParcelDetails>(jObject))
            return jObject.ToObject<ParcelDetails>();

        return default;
    }

    public override void Write(Utf8JsonWriter writer, ISpecificDetails value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private static bool ValidateJson<TSchema>(JToken jObject) where TSchema : class
    {
        var generator = new JSchemaGenerator
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var schema = generator.Generate(typeof(TSchema));

        return jObject.IsValid(schema);
    }
}