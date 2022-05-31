using DomainServices.OfferService.Contracts;
using System.Text.Json;

namespace DomainServices.OfferService.Api;

internal static class MortgageExtensions
{
    // TODO: move to custom json deserializer

    /// <summary>
    /// Parses data from json string to BasicParameters object.
    /// </summary>
    public static BasicParameters ToBasicParameters(this string json)
    {
        var basicParameters = JsonSerializer.Deserialize<BasicParameters>(json)!;

        return basicParameters;
    }

    /// <summary>
    /// Parses data from json string to SimulationInputs object.
    /// </summary>
    public static SimulationInputs ToSimulationInputs(this string json)
    {
        var inputs = JsonSerializer.Deserialize<SimulationInputs>(json);

        var jsonLoanPurpose = System.Text.Json.Nodes.JsonNode.Parse(json)?["LoanPurposes"];

        if (jsonLoanPurpose != null)
        {
            var loanPurposeItems = JsonSerializer.Deserialize<List<LoanPurpose>>(jsonLoanPurpose.ToJsonString());
            inputs?.LoanPurposes.AddRange(loanPurposeItems);
        }

        return inputs!;
    }

    /// <summary>
    /// Parses data from json string to BaseSimulationResults object.
    /// </summary>
    public static BaseSimulationResults ToBaseSimulationResults(this string json)
    {
        var results = JsonSerializer.Deserialize<BaseSimulationResults>(json);

        return results!;
    }

    /// <summary>
    /// Parses data from json string to SimulationResults object.
    /// </summary>
    public static SimulationResults ToSimulationResults(this string json)
    {
        var results = JsonSerializer.Deserialize<SimulationResults>(json);

        var jsonPaymentScheduleSimple = System.Text.Json.Nodes.JsonNode.Parse(json)?["PaymentScheduleSimple"];
        var jsonMarketingActions = System.Text.Json.Nodes.JsonNode.Parse(json)?["MarketingActions"];
        var jsonFees = System.Text.Json.Nodes.JsonNode.Parse(json)?["Fees"];

        if (jsonPaymentScheduleSimple != null)
        {
            var itemsPaymentScheduleSimple = JsonSerializer.Deserialize<List<PaymentScheduleSimple>>(jsonPaymentScheduleSimple.ToJsonString());
            results?.PaymentScheduleSimple.AddRange(itemsPaymentScheduleSimple);
        }

        if (jsonMarketingActions != null)
        {
            var itemsMarketingActions = JsonSerializer.Deserialize<List<ResultMarketingAction>>(jsonMarketingActions.ToJsonString());
            results?.MarketingActions.AddRange(itemsMarketingActions);
        }

        if (jsonFees != null)
        {
            var itemsFees = JsonSerializer.Deserialize<List<ResultFee>>(jsonFees.ToJsonString());
            results?.Fees.AddRange(itemsFees);
        }

        return results!;
    }

}