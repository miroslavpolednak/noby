using DomainServices.SalesArrangementService.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DomainServices.SalesArrangementService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed partial class ValidationTransformationServiceFactory
{
    public IValidationTransformationService CreateService(int formId)
    {
        // get transformation values from Cache
        // or init cache with data from DB
        var transformationMatrix = ValidationTransformationCache.GetOrCreate(formId, () =>
            _dbContext.FormValidationTransformations
                .AsNoTracking()
                .Where(t => t.FormId == formId.ToString())
                .Select(t => new
                {
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

        return formId switch
        {
            _ => new ValidationTransformationService(transformationMatrix)
        };
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public ValidationTransformationServiceFactory(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
