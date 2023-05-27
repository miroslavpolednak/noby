using NOBY.Api.SharedDto;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal static class Extensions
{
   
    #region Codebooks

    public static GenericCodebookResponse.Types.GenericCodebookItem? ToCodebookItem(this ProductTypesResponse.Types.ProductTypeItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new GenericCodebookResponse.Types.GenericCodebookItem
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid,
        };
    }

    public static GenericCodebookResponse.Types.GenericCodebookItem? ToCodebookItem(this GenericCodebookResponse.Types.GenericCodebookItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new GenericCodebookResponse.Types.GenericCodebookItem
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid,
        };
    }

    public static GenericCodebookResponse.Types.GenericCodebookItem? ToCodebookItem(this LoanPurposesResponse.Types.LoanPurposeItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new GenericCodebookResponse.Types.GenericCodebookItem
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid,
        };
    }

    #endregion

    public static BankAccount? ToPaymentAccount(this DomainServices.ProductService.Contracts.PaymentAccount paymentAccount)
    {
        if (paymentAccount == null)
        {
            return null;
        }

        return new BankAccount
        {
            Prefix = paymentAccount.Prefix,
            Number = paymentAccount.Number,
            BankCode = paymentAccount.BankCode,
        };
    }

}
