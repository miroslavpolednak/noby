using ExternalServices.MpHome.V1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1
{
    internal sealed class RealMpHomeClient : BaseClient<RealMpHomeClient>, IMpHomeClient
    {
        public RealMpHomeClient(HttpClient httpClient, ILogger<RealMpHomeClient> logger) : base(httpClient, logger) { }

        public Task<IServiceCallResult> Create(PartnerRequest partner, int partnerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IServiceCallResult> CreateContact(ContactData contact, int partnerId)
        {
            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.FomsCreateContactAsync(partnerId, contact);

                    return new SuccessfulServiceCallResult<ContactIdResponse>(result);
                });

            });
        }

        public async Task<IServiceCallResult> DeleteContact(int contactId, int partnerId)
        {
            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsDeleteContactAsync(partnerId, contactId);

                    return new SuccessfulServiceCallResult();
                });

            });
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

        private Client CreateClient()
        => new(_httpClient?.BaseAddress?.ToString(), _httpClient);

        private async Task<IServiceCallResult> WithClient(Func<Client, Task<IServiceCallResult>> fce)
        {
            try
            {
                return await fce(CreateClient());
            }
            catch (ApiException<ModelErrorWrapper> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<ModelErrorWrapper>>(ex);
            }
            catch (ApiException<Error> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<Error>>(ex);
            }
            catch (ApiException<ProblemDetails> ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException<ProblemDetails>>(ex);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SuccessfulServiceCallResult<ApiException>(ex);
            }
        }
    }
}
