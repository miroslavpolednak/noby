using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api
{
    internal static class CustomerExtensions
    {
        public static CustomerResponse ToDetailResponse(this Repositories.Entities.Partner entity)
        {
            // spolecne pro FO/PO
            var model = new CustomerResponse()
            {
                //Updatable = IsUpdatatable(entity.ZdrojDat),
                IdentificationDocument = new IdentificationDocument
                {
                    IssuedBy = entity.PrukazVydal ?? "",
                    IssuedOn = entity.PrukazVydalDatum ?? null,
                    //IssuingCountryCode = "TODO",
                    Number = entity.PrukazTotoznosti ?? "",
                    //Type = IdentificationDocumentTypes.A, //TODO typy dokumentu
                    ValidTo = entity.PreukazPlatnostDo ?? null
                }
            };

            //FO
            if (IsFo(entity.RodneCisloIco))
            {
                model.NaturalPerson = new NaturalPerson {
                    BirthNumber = entity.RodneCisloIco,
                    FirstName = entity.Jmeno,
                    LastName = entity.Prijmeni,
                    DateOfBirth = entity.DatumNarozeni ?? null
                };
            }
            else //PO
            { }   

            if (entity.Kontakty != null)
            {
                model.Contacts.AddRange(entity.Kontakty.Select(t => new Contact
                {
                    IsPrimary = t.PrimarniKontakt,
                    Value = t.Value
                }));
            }

            addAddress(true, 1, entity.Ulice, entity.CisloDomu4, entity.CisloDomu2, entity.Misto, entity.Psc);
            addAddress(false, 2, entity.VypisyUlice, entity.VypisyCisloDomu4, entity.VypisyCisloDomu2, entity.VypisyMisto, entity.VypisyPsc);

            return model;

            void addAddress(bool isPrimary, int addressTypeId, string? street, string? nr4, string? nr2, string? city, string? zip)
            {
                if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(nr4) || !string.IsNullOrEmpty(nr2) || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(zip))
                {
                    model.Addresses.Add(new GrpcAddress
                    {
                        BuildingIdentificationNumber = nr4 ?? "",
                        City = city ?? "",
                        IsPrimary = isPrimary,
                        LandRegistryNumber = nr2 ?? "",
                        Postcode = zip ?? "",
                        Street = street ?? "",
                        AddressTypeId = addressTypeId
                    });
                }
            }
        }

        private static bool IsFo(string? rodneCisloIco)
            => !string.IsNullOrWhiteSpace(rodneCisloIco) ? rodneCisloIco.Length <= 10 : throw new NullReferenceException("RodneCisloIco");
    }
}
