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

        /// <summary>
        /// vytvoří/updatuje vazbu mezi úvěrem a dlužníky/spoludlužníky dle zadaného partnerId a loanId
        /// </summary>
        Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest);

        /// <summary>
        /// Odstraní vazbu mezi úvěrem a dlužníky/spoludlužníky dle zadaného partnerId a loanId.
        /// </summary>
        Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId);

        /// <summary>
        /// vytvoří/updatuje záznam o úvěru v KonsDB v tabulce dbo.uver na základě poskytnutých dat
        /// </summary>
        Task<IServiceCallResult> UpdateLoan(long loanId, LoanRequest loanRequest);

        /// <summary>
        /// vytvoří/updatuje záznam o spoření (v tabulce dbo.sporeni) v KonsDB na základě poskytnutých dat a naváže spoření na konkrétního partnera dle zadaného partnerId( = vytvoří vazbu mezi partnerId a sporeniId)
        /// </summary>
        Task<IServiceCallResult> UpdateSavings(long savingId, SavingRequest savingRequest);
    }
}
