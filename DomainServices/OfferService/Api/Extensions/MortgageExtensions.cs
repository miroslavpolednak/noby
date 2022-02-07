using DomainServices.OfferService.Contracts;
using System.Text.Json;

namespace DomainServices.OfferService.Api;

internal static class MortgageExtensions
{
    /// <summary>
    /// Parses data from json string to MortgageInput object.
    /// </summary>
    public static MortgageInput ToMortgageInput(this string inputs)
    {
        inputs = inputs ?? String.Empty;
        var input = JsonSerializer.Deserialize<MortgageInput>(inputs);

        var inputJson = System.Text.Json.Nodes.JsonNode.Parse(inputs);
        var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(inputJson["LoanPurpose"].ToJsonString());
        
        input.LoanPurpose.AddRange(loanPurposeItems);

        return input;
    }
  
}
