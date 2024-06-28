namespace NOBY.ApiContracts;

public partial class CustomerIncomeData
{
    public static CustomerIncomeData Create(CustomerIncomeSharedDataEmployement? model)
    {
        return new CustomerIncomeData
        {
            Discriminator = nameof(CustomerIncomeData.Employment),
            Employment = model
        };
    }

    public static CustomerIncomeData Create(CustomerIncomeSharedDataOther? model)
    {
        return new CustomerIncomeData
        {
            Discriminator = nameof(CustomerIncomeData.Other),
            Other = model
        };
    }

    public static CustomerIncomeData Create(CustomerIncomeSharedDataEntrepreneur? model)
    {
        return new CustomerIncomeData
        {
            Discriminator = nameof(CustomerIncomeData.Entrepreneur),
            Entrepreneur = model
        };
    }
}
