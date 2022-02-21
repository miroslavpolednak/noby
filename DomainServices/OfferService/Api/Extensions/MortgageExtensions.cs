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

        var jsonLoanPurpose = System.Text.Json.Nodes.JsonNode.Parse(inputs)?["LoanPurpose"];

        if (jsonLoanPurpose != null )
        {
            var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(jsonLoanPurpose.ToJsonString());
            input?.LoanPurpose.AddRange(loanPurposeItems);
        }

#pragma warning disable CS8603 // Possible null reference return.
        return input;
#pragma warning restore CS8603 // Possible null reference return.
    }


    /// <summary>
    /// Parses data from json string to MortgageOutput object.
    /// </summary>
    public static MortgageOutput ToMortgageOutput(this string outputs)
    {
        var output = JsonSerializer.Deserialize<MortgageOutput>(outputs);

        var jsonLoanPurpose = System.Text.Json.Nodes.JsonNode.Parse(outputs)?["LoanPurpose"];

        if (jsonLoanPurpose != null)
        {
            var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(jsonLoanPurpose.ToJsonString());
            output?.LoanPurpose.AddRange(loanPurposeItems);
        }

#pragma warning disable CS8603 // Possible null reference return.
        return output;
#pragma warning restore CS8603 // Possible null reference return.
    }

}