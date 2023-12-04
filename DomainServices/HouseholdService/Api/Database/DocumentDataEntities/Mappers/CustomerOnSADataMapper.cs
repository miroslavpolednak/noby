using CIS.Core.Attributes;
using __Contracts = DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class CustomerOnSADataMapper
{
    public CustomerOnSAData MapToData(__Contracts.CustomerAdditionalData additionalData, __Contracts.CustomerChangeMetadata changeMetadata)
    {
        return new CustomerOnSAData
        {
            AdditionalData = additionalData is null ? null : new()
            {
                HasRelationshipWithCorporate = additionalData.HasRelationshipWithCorporate,
                HasRelationshipWithKB = additionalData.HasRelationshipWithKB,
                HasRelationshipWithKBEmployee = additionalData.HasRelationshipWithKBEmployee,
                IsAddressWhispererUsed = additionalData.IsAddressWhispererUsed,
                IsPoliticallyExposed = additionalData.IsPoliticallyExposed,
                IsUSPerson = additionalData.IsUSPerson,
                LegalCapacity = additionalData.LegalCapacity is null ? null : new()
                {
                    RestrictionTypeId = additionalData.LegalCapacity.RestrictionTypeId,
                    RestrictionUntil = additionalData.LegalCapacity.RestrictionUntil
                }
            },
            ChangeMetadata = changeMetadata is null ? null : new()
            {
                WasCRSChanged = changeMetadata.WasCRSChanged,
                WereClientDataChanged = changeMetadata.WereClientDataChanged
            }
        };
    }

    public (__Contracts.CustomerAdditionalData?, __Contracts.CustomerChangeMetadata?) MapFromDataToSingle(CustomerOnSAData? data)
    {
        __Contracts.CustomerAdditionalData? a = data?.AdditionalData is null ? null : new()
        {
            HasRelationshipWithCorporate = data.AdditionalData.HasRelationshipWithCorporate,
            HasRelationshipWithKB = data.AdditionalData.HasRelationshipWithKB,
            HasRelationshipWithKBEmployee = data.AdditionalData.HasRelationshipWithKBEmployee,
            IsAddressWhispererUsed = data.AdditionalData.IsAddressWhispererUsed,
            IsPoliticallyExposed = data.AdditionalData.IsPoliticallyExposed,
            IsUSPerson = data.AdditionalData.IsUSPerson,
            LegalCapacity = data.AdditionalData.LegalCapacity is null ? null : new()
            {
                RestrictionTypeId = data.AdditionalData.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = data.AdditionalData.LegalCapacity.RestrictionUntil
            }
        };

        __Contracts.CustomerChangeMetadata? b = data?.ChangeMetadata is null ? null : new()
        {
            WasCRSChanged = data.ChangeMetadata.WasCRSChanged,
            WereClientDataChanged = data.ChangeMetadata.WereClientDataChanged
        };

        return (a, b);
    }
}
