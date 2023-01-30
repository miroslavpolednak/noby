using FluentValidation;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal abstract class UpdateCustomersValidator
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
    }
}
