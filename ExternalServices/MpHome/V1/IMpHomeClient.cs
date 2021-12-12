using ExternalServices.MpHome.V1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1
{
    public interface IMpHomeClient
    {
        /// <summary>
        /// Vytvoří/updatuje partnera v KonsDB na základě poskytnutých dat.
        /// </summary>
        Task<IServiceCallResult> Create(PartnerRequest partner, int partnerId);

        /// <summary>
        /// Aktualizuje základní data partnera v KonsDB na základě partnerId a poskytnutých dat
        /// </summary>
        Task<IServiceCallResult> UpdateBaseData(PartnerBaseRequest partner, int partnerId);

        /// <summary>
        /// Updatuje doklad partnera v KonsDB na základě partnerId.
        /// </summary>
        Task<IServiceCallResult> UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId);

        /// <summary>
        /// Updatuje adresu partnera v KonsDB na základě partnerId a poskytnutých dat adresy.
        /// </summary>
        Task<IServiceCallResult> UpdateAddress(AddressData address, int partnerId);

        /// <summary>
        /// Vytvoření kontaktu partnera v KonsDB na základě poskytnutých dat.
        /// </summary>
        Task<IServiceCallResult> CreateContact(ContactData contact, int partnerId);

        /// <summary>
        /// Smazání kontaktu partnera v KonsDB na základě poskytnutého contactId a partnerId
        /// </summary>
        Task<IServiceCallResult> DeleteContact(int contactId, int partnerId);
    }
}
