using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal static class Extensions
{
   
    #region Codebooks

    public static DomainServices.CodebookService.Contracts.GenericCodebookItem? ToCodebookItem(this DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new DomainServices.CodebookService.Contracts.GenericCodebookItem
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid,
        };
    }

    public static DomainServices.CodebookService.Contracts.GenericCodebookItem? ToCodebookItem(this DomainServices.CodebookService.Contracts.Endpoints.LoanKinds.LoanKindsItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new DomainServices.CodebookService.Contracts.GenericCodebookItem
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid,
        };
    }

    public static DomainServices.CodebookService.Contracts.GenericCodebookItem? ToCodebookItem(this DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes.LoanPurposesItem item)
    {
        if (item == null)
        {
            return null;
        }

        return new DomainServices.CodebookService.Contracts.GenericCodebookItem
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
