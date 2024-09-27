using FluentValidation;

namespace NOBY.Api.Endpoints.Administration.UpdateAdminFeBanner;

internal sealed class UpdateAdminFeBannerRequestValidator
    : AbstractValidator<AdminUpdateAdminFeBannerRequest>
{
    public UpdateAdminFeBannerRequestValidator()
    {
        RuleFor(t => t.VisibleTo)
            .NotEmpty()
            .Must(t => t > DateTime.Now)
            .WithMessage("Čas platnosti do musí být v budoucnu");
    }
}
