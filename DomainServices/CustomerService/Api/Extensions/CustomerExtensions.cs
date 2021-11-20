using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using static DomainServices.CustomerService.Contracts.Contact.Types;

namespace DomainServices.CustomerService.Api
{
    internal static class CustomerExtensions
    {
        public static GetDetailResponse ToDetailResponse(this Repositories.Entities.Partner entity)
        {
            // spolecne pro FO/PO
            var model = new GetDetailResponse()
            {
                Updatable = IsUpdatatable(entity.ZdrojDat),
                IdentificationDocument = new IdentificationDocument
                {
                    IssuedBy = entity.PrukazVydal ?? "",
                    IssuedOn = entity.PrukazVydalDatum ?? null,
                    IssuingCountryCode = "TODO",
                    Number = entity.PrukazTotoznosti ?? "",
                    Type = IdentificationDocumentTypes.A, //TODO typy dokumentu
                    ValidTo = entity.PreukazPlatnostDo ?? null
                }
            };

            //FO
            if (IsFo(entity.RodneCisloIco))
            {
                model.BirthNumber = entity.RodneCisloIco;
                model.FirstName = entity.Jmeno ?? "";
                model.LastName = entity.Prijmeni ?? "";
                model.DateOfBirth = entity.DatumNarozeni ?? null;
                model.DegreeAfter = entity.TitulZa ?? "";
                model.DegreeBefore = entity.Titul ?? "";
                model.Gender = (Genders)entity.Pohlavi;
                model.PlaceOfBirth = entity.MistoNarozeni;
            }
            else //PO
            {
                model.Ico = entity.RodneCisloIco;
                model.NameJuridicalPerson = entity.Prijmeni ?? "";
            }   

            if (entity.Kontakty != null)
            {
                model.Contacts.AddRange(entity.Kontakty.Select(t => new Contact
                {
                    IsPrimary = t.PrimarniKontakt,
                    Type = ContactTypes.Unknown, //TODO typy kontaktu
                    Value = t.Value
                }));
            }

            addAddress(AddressTypes.Pernament, entity.Ulice, entity.CisloDomu1, entity.CisloDomu2, entity.Misto, entity.Psc);
            addAddress(AddressTypes.Mailing, entity.VypisyUlice, entity.VypisyCisloDomu1, entity.VypisyCisloDomu2, entity.VypisyMisto, entity.VypisyPsc);

            return model;

            void addAddress(AddressTypes type, string? street, string? nr1, string? nr2, string? city, string? zip)
            {
                if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(nr1) || !string.IsNullOrEmpty(nr2) || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(zip))
                {
                    model.Addresses.Add(new Address
                    {
                        BuildingIdentificationNumber = nr2 ?? "",
                        City = city ?? "",
                        CountryCode = "",
                        IsPrimary = type == AddressTypes.Mailing,
                        LandRegistryNumber = nr1 ?? "",
                        Postcode = zip ?? "",
                        Street = street ?? "",
                        Type = type
                    });
                }
            }
        }

        public static GetBasicDataResponse ToBasicDataResponse(this Repositories.Entities.Partner entity)
        { 
            var model = new GetBasicDataResponse()
            {
                Identity = entity.Id,
                Updatable = IsUpdatatable(entity.ZdrojDat),
                IdentificationDocument = new IdentificationDocument
                {
                    IssuedBy = entity.PrukazVydal ?? "",
                    IssuedOn = entity.PrukazVydalDatum ?? null,
                    IssuingCountryCode = "TODO",
                    Number = entity.PrukazTotoznosti ?? "",
                    Type = IdentificationDocumentTypes.A, //TODO typy dokumentu
                    ValidTo = entity.PreukazPlatnostDo ?? null
                }
            };

            //FO
            if (IsFo(entity.RodneCisloIco))
            {
                model.BirthNumber = entity.RodneCisloIco;
                model.FirstName = entity.Jmeno ?? "";
                model.LastName = entity.Prijmeni ?? "";
                model.DateOfBirth = entity.DatumNarozeni ?? null;
                model.DegreeAfter = entity.TitulZa ?? "";
                model.DegreeBefore = entity.Titul ?? "";
                model.Gender = (Genders)entity.Pohlavi;
                model.PlaceOfBirth = entity.MistoNarozeni ?? "";
            }
            else //PO
            {
                model.Ico = entity.RodneCisloIco;
                model.NameJuridicalPerson = entity.Prijmeni ?? "";
            }

            return model;
        }

        private static bool IsFo(string? rodneCisloIco)
            => !string.IsNullOrWhiteSpace(rodneCisloIco) ? rodneCisloIco.Length <= 10 : throw new NullReferenceException("RodneCisloIco");

        private static bool IsUpdatatable(int zdrojDat)
            => zdrojDat == 1;
    }
}
