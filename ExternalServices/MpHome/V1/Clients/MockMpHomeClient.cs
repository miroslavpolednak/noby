using ExternalServices.MpHome.V1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1
{
    internal sealed class MockMpHomeClient : IMpHomeClient
    {
        public Task<IServiceCallResult> Create(PartnerRequest partner, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> CreateContact(ContactData contact, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> DeleteContact(int contactId, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> UpdateAddress(AddressData address, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> UpdateBaseData(PartnerBaseRequest partner, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceCallResult> UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId)
        {
            throw new NotImplementedException();
        }
    }
}
