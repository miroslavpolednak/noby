﻿using DomainServices.SalesArrangementService.Api.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using static DomainServices.SalesArrangementService.Api.Services.ValidationTransformationCache;

namespace DomainServices.SalesArrangementService.Api.Services;

internal sealed partial class ValidationTransformationServiceFactory
{
    private class ValidationTransformationService
        : IValidationTransformationService
    {
        public List<Contracts.ValidationMessage> TransformErrors(string json, Dictionary<string, Eas.CheckFormV2.Error[]>? errors)
        {
            // no errors -> empty list
            if (errors is null || !errors.Any()) return new List<Contracts.ValidationMessage>(0);

            // init method result
            var transformedItems = new List<Contracts.ValidationMessage>(errors.Count);

            // parse json data to searchable object
            _jsonFormData = JObject.Parse(json)!;

            // for each error field
            foreach (var errorGroup in errors)
            {
                // each error field may contain multiple error messages
                foreach (var error in errorGroup.Value)
                {
                    // kopie chyby SB
                    var item = new Contracts.ValidationMessage
                    {
                        AdditionalInformation = error.AdditionalInformation,
                        Code = error.ErrorCode,
                        ErrorQueue = error.ErrorQueue,
                        Message = error.ErrorMessage,
                        Severity = error.Severity,
                        Value = error.Value,
                        Parameter = errorGroup.Key
                    };
                    // transformace na NOBY
                    item.NobyMessageDetail = CreateNobyMessage(item);

                    transformedItems.Add(item);
                }
            }

            return transformedItems;
        }

        private Contracts.ValidationMessageNoby CreateNobyMessage(Contracts.ValidationMessage item)
        {
            ValidationTransformationCache.TransformationItem titem;
            var message = new Contracts.ValidationMessageNoby();
            
            var matches = _arrayIndexesRegex.Matches(item.Parameter);
            if (matches.Any())
            {
                titem = getTransformationItem(_arrayIndexesRegex.Replace(item.Parameter, _parameterReplaceEvaluator));
                string[] arguments = matches.Select(m =>
                {
                    switch (m.Groups["par"].Value)
                    {
                        case "seznam_ucastniku":
                            return getJsonValue($"seznam_ucastniku[{m.Groups["idx"].Value}].klient.jmeno") + " " + getJsonValue($"seznam_ucastniku[{m.Groups["idx"].Value}].klient.prijmeni_nazev");
                        default:
                            return (Convert.ToInt32(m.Groups["idx"].Value) + 1).ToString();
                    }
                }).ToArray();
                message.Message = string.Format(titem.Text, arguments);
            }
            else
            {
                titem = getTransformationItem(item.Parameter);
                message.Message = titem.Text;
            }

            message.ParameterName = titem.Name;
            message.Category = titem.Category;
            
            // severity
            if (titem.AlterSeverity == Database.FormValidationTransformationAlterSeverity.Ignore)
                message.Severity = Contracts.ValidationMessageNoby.Types.NobySeverity.None;
            else if (titem.AlterSeverity == Database.FormValidationTransformationAlterSeverity.AlterToWarning)
                message.Severity = Contracts.ValidationMessageNoby.Types.NobySeverity.Warning;
            else
                message.Severity = item.ErrorQueue == "A" ? Contracts.ValidationMessageNoby.Types.NobySeverity.Error : Contracts.ValidationMessageNoby.Types.NobySeverity.Warning;

            return message;

            TransformationItem getTransformationItem(string key)
            {
                if (_transformationMatrix.Any(t => t.Key == key))
                {
                    return _transformationMatrix[key];
                }
                else
                {
                    // polozka neexistuje v transformacni tabulce... co s tim?
                    return new TransformationItem
                    {
                        Name = item.Parameter,
                        Category = "-unknown-",
                        Text = item.Message
                    };
                }
            }
        }

        private string getJsonValue(string path)
            => _jsonFormData?.SelectToken(path)?.Value<string>() ?? "???";

        // global props
        private static Regex _arrayIndexesRegex = new Regex(@"(?<par>\w+)\[(?<idx>\d)\]", RegexOptions.Compiled | RegexOptions.NonBacktracking);
        private static MatchEvaluator _parameterReplaceEvaluator = new MatchEvaluator((Match m) => $"{m.Groups["par"]}[]");

        // instance props
        private JObject? _jsonFormData;
        private readonly ImmutableDictionary<string, TransformationItem> _transformationMatrix;

        public ValidationTransformationService(ImmutableDictionary<string, TransformationItem> transformationMatrix)
        {
            _transformationMatrix = transformationMatrix;
        }
    }
}