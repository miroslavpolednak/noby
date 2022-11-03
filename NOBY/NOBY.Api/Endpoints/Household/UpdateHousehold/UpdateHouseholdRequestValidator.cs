using CIS.Core.Validation;
using FluentValidation;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

public class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator()
    {
    }
}
