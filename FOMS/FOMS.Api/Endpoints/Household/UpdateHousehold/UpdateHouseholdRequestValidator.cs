using CIS.Core.Validation;
using FluentValidation;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator()
    {
    }
}
