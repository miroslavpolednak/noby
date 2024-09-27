using FluentValidation;

namespace NOBY.Api.Endpoints.Administration.CreateAdminFeBanner;

internal sealed class CreateAdminFeBannerRequestValidator
    : AbstractValidator<AdminCreateAdminFeBannerRequest>
{
    public CreateAdminFeBannerRequestValidator()
    {
        RuleFor(t => t.VisibleTo)
            .NotEmpty()
            .Must(t => t > DateTime.Now)
            .WithMessage("Čas platnosti do musí být v budoucnu");
    }
}
