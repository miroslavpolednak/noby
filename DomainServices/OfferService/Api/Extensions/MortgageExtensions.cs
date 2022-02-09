using DomainServices.OfferService.Contracts;
using System.Text.Json;

namespace DomainServices.OfferService.Api;

internal static class MortgageExtensions
{
    //TODO: move to custom json deserializer

    /// <summary>
    /// Parses data from json string to MortgageInput object.
    /// </summary>
    public static MortgageInput ToMortgageInput(this string inputs)
    {
        var input = JsonSerializer.Deserialize<MortgageInput>(inputs);

        var inputJson = System.Text.Json.Nodes.JsonNode.Parse(inputs);
        var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(inputJson["LoanPurpose"].ToJsonString());
        
        input.LoanPurpose.AddRange(loanPurposeItems);

        return input;
    }

    /// <summary>
    /// Parses data from json string to MortgageOutput object.
    /// </summary>
    public static MortgageOutput ToMortgageOutput(this string outputs)
    {
        var output = JsonSerializer.Deserialize<MortgageOutput>(outputs);

        var inputJson = System.Text.Json.Nodes.JsonNode.Parse(outputs);
        var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(inputJson["LoanPurpose"].ToJsonString());

        output.LoanPurpose.AddRange(loanPurposeItems);

        return output;
    }

}