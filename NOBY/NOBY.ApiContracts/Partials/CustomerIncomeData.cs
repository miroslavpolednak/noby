namespace NOBY.ApiContracts;

public partial class CustomerIncomeDataOneOf
{
    public static CustomerIncomeDataOneOf Create(CustomerIncomeSharedDataEmployement? model)
    {
        return new CustomerIncomeDataOneOf
        {
            Discriminator = nameof(CustomerIncomeDataOneOf.Employment),
            Employment = model
        };
    }

    public static CustomerIncomeDataOneOf Create(CustomerIncomeSharedDataOther? model)
    {
        return new CustomerIncomeDataOneOf
        {
            Discriminator = nameof(CustomerIncomeDataOneOf.Other),
            Other = model
        };
    }

    public static CustomerIncomeDataOneOf Create(CustomerIncomeSharedDataEntrepreneur? model)
    {
        return new CustomerIncomeDataOneOf
        {
            Discriminator = nameof(CustomerIncomeDataOneOf.Entrepreneur),
            Entrepreneur = model
        };
    }
}
