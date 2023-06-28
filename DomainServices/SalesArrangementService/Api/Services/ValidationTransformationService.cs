using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using static DomainServices.SalesArrangementService.Api.Services.ValidationTransformationCache;

namespace DomainServices.SalesArrangementService.Api.Services;

internal sealed partial class ValidationTransformationServiceFactory
{
    private sealed class ValidationTransformationService
        : IValidationTransformationService
    {
        public List<Contracts.ValidationMessage> TransformErrors(string json, Dictionary<string, Eas.CheckFormV2.ErrorDto[]>? errors)
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
                            return (Convert.ToInt32(m.Groups["idx"].Value, CultureInfo.InvariantCulture) + 1).ToString(CultureInfo.InvariantCulture);
                    }
                }).ToArray();
                message.ParameterName = string.Format(CultureInfo.InvariantCulture, titem.Text, arguments);
            }
            else
            {
                titem = getTransformationItem(item.Parameter);
                message.ParameterName = titem.Text;
            }

            message.Message = getMessage();
            message.Category = titem.Category;
            message.CategoryOrder = titem.CategoryOrder;

            // severity
            if (titem.AlterSeverity == Database.FormValidationTransformationAlterSeverity.Ignore)
                message.Severity = Contracts.ValidationMessageNoby.Types.NobySeverity.None;
            else if (titem.AlterSeverity == Database.FormValidationTransformationAlterSeverity.AlterToWarning)
                message.Severity = Contracts.ValidationMessageNoby.Types.NobySeverity.Warning;
            else
            {
                message.Severity = item.ErrorQueue switch
                {
                    "A" => Contracts.ValidationMessageNoby.Types.NobySeverity.Error,
                    "I" => Contracts.ValidationMessageNoby.Types.NobySeverity.Warning,
                    _ => Contracts.ValidationMessageNoby.Types.NobySeverity.None
                };
            }
            
            return message;

            string getMessage()
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    return string.IsNullOrEmpty(item.AdditionalInformation) ? item.Message : $"{item.Message} ({item.AdditionalInformation})";
                }
                else
                {
                    return string.IsNullOrEmpty(item.AdditionalInformation) ? $"'{item.Value}' {item.Message}" : $"'{item.Value}' {item.Message} ({item.AdditionalInformation})";
                }
            }

            TransformationItem getTransformationItem(string key)
            {
                if (_transformationMatrix.ContainsKey(key))
                {
                    return _transformationMatrix[key];
                }
                else
                {
                    // polozka neexistuje v transformacni tabulce... co s tim?
                    return new TransformationItem
                    {
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
        private readonly IReadOnlyDictionary<string, TransformationItem> _transformationMatrix;

        public ValidationTransformationService(IReadOnlyDictionary<string, TransformationItem> transformationMatrix)
        {
            _transformationMatrix = transformationMatrix;
        }
    }
}