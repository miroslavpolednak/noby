﻿using DomainServices.CustomerService.Contracts;
using DomainServices.RealEstateValuationService.Contracts;
using SharedTypes.Extensions;

namespace DomainServices.RealEstateValuationService.Api.Extensions;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
internal static class OrderRequestExtensions
{
    public static void FillBaseOrderData(
        this ExternalServices.PreorderService.Dto.IOrderBaseData model,
        CaseService.Contracts.Case caseInstance,
        Customer customer,
        UserService.Clients.Dto.UserDto currentUser,
        in long[]? realEstateIds,
        in long[]? attachments
        )
    {
        model.DealNumber = caseInstance.Data.ContractNumber;
        model.EFormId = 0;
        model.CompanyCode = "01";
        model.ProductCode = "01";
        model.Cpm = Convert.ToInt64(currentUser.UserInfo.Cpm, CultureInfo.InvariantCulture);
        model.Icp = Convert.ToInt64(currentUser.UserInfo.Icp, CultureInfo.InvariantCulture);
        model.ClientID = customer.Identities?.GetKbIdentityOrDefault()?.IdentityId;
        model.ClientName = $"{customer.NaturalPerson?.FirstName} {customer.NaturalPerson?.LastName}";
        model.ClientEmail = customer.Contacts?.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email)?.Email?.EmailAddress;

        if (realEstateIds?.Any() ?? false)
        {
            model.CremRealEstateIds = realEstateIds;
        }

        model.AttachmentIds = attachments ?? Array.Empty<long>();
    }

    public static void FillBaseStandardOrderData(
        this ExternalServices.PreorderService.Dto.IOrderStandardBaseData model,
        UserService.Clients.Dto.UserDto currentUser,
        Database.Entities.RealEstateValuation entity,
        SpecificDetailHouseAndFlatObject? houseAndFlat,
        in Dto.GetProductPropertiesResult productProps)
    {
        model.ProductOwner = "01";
        model.ContactPersonName = $"{currentUser.UserInfo.FirstName} {currentUser.UserInfo.LastName}";
        model.ContactPersonEmail = currentUser.UserInfo.Email;
        model.ContactPersonTel = currentUser.UserInfo.PhoneNumber;
        model.BagmanRealEstateTypeId = entity.BagmanRealEstateTypeId ?? "";
        model.RealEstateTypeId = entity.ACVRealEstateTypeId ?? "";
        model.Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased;
        model.OwnershipLimitations = houseAndFlat?.OwnershipRestricted;
        model.IsCellarFlat = houseAndFlat?.FlatOnlyDetails?.Basement;
        model.IsNotUsableTechnicalState = houseAndFlat?.PoorCondition;
        model.MaturityLoan = productProps.LoanDuration;
        model.PurposeLoan = productProps.LoanPurpose ?? "";

        if (productProps.CollateralAmount.HasValue)
            model.ActualPurchasePrice = Convert.ToDouble(productProps.CollateralAmount, CultureInfo.InvariantCulture);
        if (productProps.LoanAmount.HasValue)
            model.LoanAmount = Convert.ToDouble(productProps.LoanAmount, CultureInfo.InvariantCulture);
    }
}
