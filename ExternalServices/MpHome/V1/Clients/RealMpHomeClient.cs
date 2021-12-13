using ExternalServices.MpHome.V1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1
{
    internal sealed class RealMpHomeClient : BaseClient<RealMpHomeClient>, IMpHomeClient
    {
        public RealMpHomeClient(HttpClient httpClient, ILogger<RealMpHomeClient> logger) : base(httpClient, logger) { }

        public async Task<IServiceCallResult> Create(PartnerRequest partner, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome Create with partnerId {partnerId}, PartnerRequest {partner}", partnerId, System.Text.Json.JsonSerializer.Serialize(partner));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdatePartnerAsync(partnerId, partner);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> CreateContact(ContactData contact, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome CreateContact with partnerId {partnerId}, ContactData {contact}", partnerId, System.Text.Json.JsonSerializer.Serialize(contact));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    var result = await c.FomsCreateContactAsync(partnerId, contact);

                    return new SuccessfulServiceCallResult<ContactIdResponse>(result);
                });

            });
        }

        public async Task<IServiceCallResult> DeleteContact(int contactId, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome DeleteContact with partnerId {partnerId}, contactId {contactId}", partnerId, contactId);

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsDeleteContactAsync(partnerId, contactId);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome DeletePartnerLoanLink with partnerId {partnerId}, loanId {partner}", partnerId, loanId);

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsDeletePartnerLoanLinkAsync(loanId, partnerId);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateAddress(AddressData address, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateAddress with partnerId {partnerId}, AddressData {address}", partnerId, System.Text.Json.JsonSerializer.Serialize(address));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateAddressAsync(partnerId, address);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateBaseData(PartnerBaseRequest partner, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateBaseData with partnerId {partnerId}, PartnerBaseRequest {partner}", partnerId, System.Text.Json.JsonSerializer.Serialize(partner));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdatePartner2Async(partnerId, partner);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateIdentificationDocument with partnerId {partnerId}, IdentificationDocument {identificationDocument}", partnerId, System.Text.Json.JsonSerializer.Serialize(identificationDocument));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateIdentificationDocumentAsync(partnerId, identificationDocument);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateLoan(long loanId, LoanRequest loanRequest)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateLoan with loanId {loanId}, LoanRequest {loanRequest}", loanId, System.Text.Json.JsonSerializer.Serialize(loanRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateLoanAsync(loanId, loanRequest);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateLoanPartnerLink with partnerId {partnerId}, loanId {loanId}, LoanLinkRequest {loanLinkRequest}", partnerId, loanId, System.Text.Json.JsonSerializer.Serialize(loanLinkRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateLoanPartnerLinkAsync(loanId, partnerId, loanLinkRequest);

                    return new SuccessfulServiceCallResult();
                });

            });
        }

        public async Task<IServiceCallResult> UpdateSavings(long savingId, SavingRequest savingRequest)
        {
            _logger.LogDebug("Run inputs: MpHome UpdateSavings with savingId {savingId}, SavingRequest {savingRequest}", savingId, System.Text.Json.JsonSerializer.Serialize(savingRequest));

            return await WithClient(async c => {

                return await callMethod(async () => {

                    await c.FomsUpdateSavingsAsync(savingId, savingRequest);

                    return new SuccessfulServiceCallResult();
                });

            });
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
