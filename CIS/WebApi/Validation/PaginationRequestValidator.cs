using FluentValidation;

namespace CIS.Infrastructure.WebApi.Validation;

public class PaginationRequestValidator
    : AbstractValidator<Types.PaginationRequest?>
{
    public PaginationRequestValidator()
    {
        RuleFor(t => t.RecordOffset)
            .GreaterThanOrEqualTo(0).WithMessage("Pagination RecordOffset must be >= 0");

        RuleFor(t => t.PageSize)
            .GreaterThan(0).WithMessage("Pagination PageSize must be > 0");

        RuleForEach(t => t.Sorting)
            .SetValidator(new PaginationSortFieldValidator()).When(t => t.Sorting is not null && t.Sorting.Any());
    }
}

public class PaginationSortFieldValidator
    : AbstractValidator<Types.PaginationSortingField>
{
    public PaginationSortFieldValidator()
    {
        RuleFor(t => t.Field)
            .NotEmpty().WithMessage("Pagination/SortField can not be empty");
    }
}
