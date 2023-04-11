using FluentValidation;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal sealed class UpdateCustomersValidator
    : AbstractValidator<UpdateCustomersRequest>
{
    public UpdateCustomersValidator()
    {
        When(t => t.Customer1 is not null && t.Customer2 is not null, () =>
        {
            RuleFor(x => x.Customer1!.CustomerOnSAId)
                .NotEqual(x => x.Customer2!.CustomerOnSAId)
                .WithMessage("CustomerOnSAId most not be equal");

            When(t => t.Customer1!.Identity is not null && t.Customer2!.Identity is not null, () =>
            {
                RuleFor(x => x.Customer1!.Identity!.Id)
                    .NotEqual(x => x.Customer2!.Identity!.Id)
                    .WithMessage("KBID most not be equal");
            });
        });

        RuleFor(t => t.Customer1!.CustomerOnSAId)
            .Must((r, id) => (r.Customer1!.Identity?.Id ?? 0) == 0 || id.GetValueOrDefault() > 0)
            .WithMessage("CustomerOnSAId1 can not be empty while Identity is set")
            .When(t => t.Customer1 is not null);

        RuleFor(t => t.Customer2!.CustomerOnSAId)
            .Must((r, id) => (r.Customer2!.Identity?.Id ?? 0) == 0 || id.GetValueOrDefault() > 0)
            .WithMessage("CustomerOnSAId2 can not be empty while Identity is set")
            .When(t => t.Customer2 is not null);
    }
}
