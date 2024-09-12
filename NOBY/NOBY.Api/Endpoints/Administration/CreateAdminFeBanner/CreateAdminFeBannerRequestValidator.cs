using FluentValidation;

namespace NOBY.Api.Endpoints.Administration.CreateAdminFeBanner;

internal sealed class CreateAdminFeBannerRequestValidator
    : AbstractValidator<AdminCreateAdminFeBannerRequest>
{
    public CreateAdminFeBannerRequestValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty();

        RuleFor(t => t.Description)
            .NotEmpty();

        RuleFor(t => t.VisibleFrom)
            .NotEmpty();

        RuleFor(t => t.VisibleTo)
            .NotEmpty()
            .Must(t => t > DateTime.Now);
    }
}
