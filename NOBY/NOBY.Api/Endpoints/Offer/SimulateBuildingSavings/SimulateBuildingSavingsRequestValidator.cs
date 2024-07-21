using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.SimulateBuildingSavings;

internal sealed class SimulateBuildingSavingsRequestValidator : AbstractValidator<OfferSimulateBuildingSavingsRequest>
{
    public SimulateBuildingSavingsRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.BlueBang))
            .WithErrorCode(90067);

        RuleFor(t => t.ContractStartDate).LessThan(DateOnly.FromDateTime(DateTime.Now)).WithErrorCode(90032);
        RuleFor(t => t).Must(t => !t.IsClientJuridicalPerson || !t.StateSubsidyRequired).WithErrorCode(90032);
        RuleFor(t => t).Must(t => !t.SimulateUntilBindingPeriod || t.ContractTerminationDate == null).WithErrorCode(90032);
        RuleFor(t => t).Must(t => !t.IsClientSVJ || t.IsClientJuridicalPerson).WithErrorCode(90032);

        RuleFor(t => t).Must(t => t.ExtraDeposits.All(e => e.Date > t.ContractStartDate)).WithErrorCode(90032);

        RuleForEach(t => t.ExtraDeposits).ChildRules(t =>
        {
            t.RuleFor(x => x.Amount).Must(amount => amount % 1000 == 0).WithErrorCode(90032);
        }).WithErrorCode(90032);
    }
}