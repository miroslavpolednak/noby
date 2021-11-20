using DomainServices.CustomerService.Api.ExternalServices.MpHome.MpHomeWrapper;

namespace DomainServices.CustomerService.Api.ExternalServices.MpHome
{
    internal class MockMpHomeClient : IMpHomeClient
    {
        public Task Create(PartnerRequest partner, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateContact(ContactData contact, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContact(int contactId, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAddress(AddressData address, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBaseData(PartnerBaseRequest partner, int partnerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId)
        {
            throw new NotImplementedException();
        }
    }
}
