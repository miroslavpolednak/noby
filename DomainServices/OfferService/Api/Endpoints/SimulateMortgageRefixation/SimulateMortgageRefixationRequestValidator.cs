using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationRequestValidator
    : AbstractValidator<SimulateMortgageRefixationRequest>
{
    public SimulateMortgageRefixationRequestValidator()
    {
        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t.BasicParameters)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);
    }
}
