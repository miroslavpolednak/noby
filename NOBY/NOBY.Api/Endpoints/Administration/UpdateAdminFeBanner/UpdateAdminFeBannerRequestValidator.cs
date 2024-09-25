using FluentValidation;

namespace NOBY.Api.Endpoints.Administration.UpdateAdminFeBanner;

internal sealed class UpdateAdminFeBannerRequestValidator
    : AbstractValidator<AdminUpdateAdminFeBannerRequest>
{
    public UpdateAdminFeBannerRequestValidator()
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
