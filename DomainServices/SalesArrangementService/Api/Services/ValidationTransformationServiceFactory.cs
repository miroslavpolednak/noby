using Microsoft.EntityFrameworkCore;

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
                .Where(t => t.FormId == formId.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .Select(t => new
                {
                    t.Category,
                    t.Text,
                    t.CategoryOrder,
                    t.AlterSeverity,
                    Path = t.FieldPath
                })
                .ToList()
                .ToDictionary(k => k.Path, v => new ValidationTransformationCache.TransformationItem
                {
                    Category = v.Category,
                    CategoryOrder = v.CategoryOrder,
                    Text = v.Text,
                    AlterSeverity = v.AlterSeverity
                })
                .AsReadOnly()
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
