using CIS.Foms.Enums;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class HouseholdInfo
{
    private readonly CustomerOnSA? _customerOnSa1;
    private readonly CustomerOnSA? _customerOnSa2;

    public Household? Household { get; init; }

    public CustomerOnSA? CustomerOnSa1
    {
        get => _customerOnSa1;
        init
        {
            _customerOnSa1 = value;

            SetDebtorOrCodebtor(value);
        }
    }

    public CustomerOnSA? CustomerOnSa2
    {
        get => _customerOnSa2;
        init
        {
            _customerOnSa2 = value;

            SetDebtorOrCodebtor(value);
        }
    }

    public CustomerOnSA Debtor { get; private set; } = null!;

    public CustomerOnSA? Codebtor { get; private set; }

    private void SetDebtorOrCodebtor(CustomerOnSA? customerOnSA)
    {
        if (customerOnSA is null)
            return;

        switch ((CustomerRoles)customerOnSA.CustomerRoleId)
        {
            case CustomerRoles.Debtor:
                Debtor = customerOnSA;
                break;

            case CustomerRoles.Codebtor:
                Codebtor = customerOnSA;
                break;
        }
    }
}