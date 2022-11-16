using DomainServices.SalesArrangementService.Api.Handlers.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

internal sealed class ValidationTransformationService
{
    private static Regex _arrayIndexesRegex = new Regex(@"\[(?<idx>\d)\]", RegexOptions.Compiled | RegexOptions.NonBacktracking);

    public List<Contracts.ValidationMessage> TransformErrors(string formId, Form form, Dictionary<string, ExternalServices.Eas.R21.CheckFormV2.Error[]>? errors)
    {
        if (errors is null || !errors.Any()) return new List<Contracts.ValidationMessage>(0);

        var transformedItems = new List<Contracts.ValidationMessage>(errors.Count);
        // get transformation values from DB
        var transformationMatrix = ValidationTransformationCache.GetOrCreate(formId, _getCacheItemsFce);

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
                item.NobyMessageDetail = item.CreateNobyMessage(transformationMatrix);

                transformedItems.Add(item);
            }
        }

        return transformedItems;
    }

    static Contracts.ValidationMessageNoby CreateNobyMessage(this Contracts.ValidationMessage item, ImmutableDictionary<string, TransformationItem> transformationItems)
    {
        ValidationTransformationCache.TransformationItem titem;
        var matches = _arrayIndexesRegex.Matches(item.Parameter);
        if (matches.Any())
        {
            titem = transformationItems[_arrayIndexesRegex.Replace(item.Parameter, "[]")];
            string.Format(titem.Text, matches.Select(t => t.Groups["idx"].Value).ToArray());
        }
        else
            titem = transformationItems[item.Parameter];

        return new Contracts.ValidationMessageNoby
        {
            Category = titem.Category,
            Message = titem.Text,
            ParameterName = titem.Name,
            Severity = Contracts.ValidationMessageNoby.Types.NobySeverity.None
        };
    }

    Func<ImmutableDictionary<string, TransformationItem>> _getCacheItemsFce = () =>
    {
        return _dbContext.FormValidationTransformations
        .AsNoTracking()
            .Where(t => t.FormId == formId)
            .Select(t => new {
                Category = t.Category,
                Text = t.Text,
                Name = t.Name,
                AlterSeverity = t.AlterSeverity,
                Path = t.Path
            })
            .ToList()
            .ToImmutableDictionary(k => k.Path, v => new ValidationTransformationCache.TransformationItem
            {
                Category = v.Category,
                Text = v.Text,
                Name = v.Name,
                AlterSeverity = v.AlterSeverity
            });
    };

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public ValidationTransformationService(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}