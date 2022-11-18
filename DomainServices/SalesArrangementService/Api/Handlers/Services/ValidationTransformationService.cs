using DomainServices.SalesArrangementService.Api.Handlers.Forms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class ValidationTransformationService
{
    private static Regex _arrayIndexesRegex = new Regex(@"\[(?<idx>\d)\]", RegexOptions.Compiled | RegexOptions.NonBacktracking);

    public List<Contracts.ValidationMessage> TransformErrors(string formId, Form form, Dictionary<string, ExternalServices.Eas.R21.CheckFormV2.Error[]>? errors)
    {
        if (errors is null || !errors.Any()) return new List<Contracts.ValidationMessage>(0);

        var transformedItems = new List<Contracts.ValidationMessage>(errors.Count);

        // get transformation values from DB
        var transformationMatrix = ValidationTransformationCache.GetOrCreate(formId, () =>
            _dbContext.FormValidationTransformations
                .AsNoTracking()
                .Where(t => t.FormId == formId)
                .Select(t => new {
                    Category = t.Category,
                    Text = t.Text,
                    Name = t.FieldName,
                    AlterSeverity = t.AlterSeverity,
                    Path = t.FieldPath
                })
                .ToList()
                .ToImmutableDictionary(k => k.Path, v => new ValidationTransformationCache.TransformationItem
                {
                    Category = v.Category,
                    Text = v.Text,
                    Name = v.Name,
                    AlterSeverity = v.AlterSeverity
                })
        );

        foreach (var errorGroup in errors)
        {
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
                item.NobyMessageDetail = CreateNobyMessage(item, transformationMatrix);

                transformedItems.Add(item);
            }
        }

        return transformedItems;
    }

    static Contracts.ValidationMessageNoby CreateNobyMessage(Contracts.ValidationMessage item, ImmutableDictionary<string, ValidationTransformationCache.TransformationItem> transformationItems)
    {
        ValidationTransformationCache.TransformationItem titem;
        var message = new Contracts.ValidationMessageNoby();

        var matches = _arrayIndexesRegex.Matches(item.Parameter);
        if (matches.Any())
        {
            titem = transformationItems[_arrayIndexesRegex.Replace(item.Parameter, "[]")];
            message.Message = string.Format(titem.Text, matches.Select(t => t.Groups["idx"].Value).ToArray());
        }
        else
        {
            titem = transformationItems[item.Parameter];
            message.Message = titem.Text;
        }

        message.ParameterName = titem.Name;
        message.Category = titem.Category;
        message.Severity = item.ErrorQueue == "A" ? Contracts.ValidationMessageNoby.Types.NobySeverity.Error : Contracts.ValidationMessageNoby.Types.NobySeverity.Warning;

        return message;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public ValidationTransformationService(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}