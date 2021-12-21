using ExternalServices.MpHome.V1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1
{
    internal sealed class MockMpHomeClient : IMpHomeClient
    {
        public async Task<IServiceCallResult> Create(PartnerRequest partner, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> CreateContact(ContactData contact, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult<ContactIdResponse>(new () { ContactId = 123 }));
        }

        public async Task<IServiceCallResult> DeleteContact(int contactId, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateAddress(AddressData address, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateBaseData(PartnerBaseRequest partner, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateLoan(long loanId, LoanRequest loanRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateSavings(long savingId, SavingRequest savingRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }
    }
}
