using CIS.Foms.Enums;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class HouseholdInfo
{
    private CustomerOnSA? _customerOnSa1;
    private CustomerOnSA? _customerOnSa2;

    public required Household Household { get; init; }

    public List<CustomerOnSA> CustomersOnSa { get; } = new(2);

    public CustomerOnSA? CustomerOnSa1
    {
        get => _customerOnSa1;
        set
        {
            _customerOnSa1 = value;

            SetDebtorOrCodebtor(value);
        }
    }

    public CustomerOnSA? CustomerOnSa2
    {
        get => _customerOnSa2;
        set
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

        CustomersOnSa.Add(customerOnSA);

        switch ((CustomerRoles)customerOnSA.CustomerRoleId)
        {
            case CustomerRoles.Debtor:
                Debtor = customerOnSA;
                break;

            case CustomerRoles.Codebtor:
                Codebtor = customerOnSA;
                break;

            default:
                throw new NotImplementedException();
        }
    }
}