using DomainServices.CustomerService.Api.ExternalServices.MpHome.MpHomeWrapper;

namespace DomainServices.CustomerService.Api.ExternalServices.MpHome;

internal interface IMpHomeClient
{
    /// <summary>
    /// Vytvoří/updatuje partnera v KonsDB na základě poskytnutých dat.
    /// </summary>
    Task Create(PartnerRequest partner, int partnerId);

    /// <summary>
    /// Aktualizuje základní data partnera v KonsDB na základě partnerId a poskytnutých dat
    /// </summary>
    Task UpdateBaseData(PartnerBaseRequest partner, int partnerId);

    /// <summary>
    /// Updatuje doklad partnera v KonsDB na základě partnerId.
    /// </summary>
    Task UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId);

    /// <summary>
    /// Updatuje adresu partnera v KonsDB na základě partnerId a poskytnutých dat adresy.
    /// </summary>
    Task UpdateAddress(AddressData address, int partnerId);

    /// <summary>
    /// Vytvoření kontaktu partnera v KonsDB na základě poskytnutých dat.
    /// </summary>
    Task<int> CreateContact(ContactData contact, int partnerId);

    /// <summary>
    /// Smazání kontaktu partnera v KonsDB na základě poskytnutého contactId a partnerId
    /// </summary>
    Task DeleteContact(int contactId, int partnerId);
}
