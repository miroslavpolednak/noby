namespace ExternalServices.AddressWhisperer.V1;

public interface IAddressWhispererClient
{
    /// <summary>
    /// Vraci seznam nalezenych adres pro 'text'
    /// </summary>
    /// <param name="sessionId">Session ID vygenerovane konzumentem</param>
    /// <param name="text">Hledany retezec</param>
    /// <param name="pageSize">Pocet vracenych zaznamu</param>
    /// <param name="country">ISO kod zeme nebo null (bude doplnen CZ)</param>
    /// <returns>
    /// V pripade nalezenych zaznamu <see cref="SuccessfulServiceCallResult{TModel}">SuccessfulServiceCallResult&lt;List&lt;Shared.FoundSuggestion&gt;&gt;</see>
    /// V pripade zadneho nalezeneho vysledku <see cref="EmptyServiceCallResult"></see>
    /// </returns>
    Task<IServiceCallResult> GetSuggestions(string sessionId, string text, int pageSize, string? country, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId">SessionId generované přímo na frontendu (unikátní GUID)</param>
    /// <param name="addressId">Id adresy</param>
    /// <param name="country">Id státu z číselníku Country</param>
    /// <param name="title">Parametr title našeptávače adres</param>
    /// <returns>
    /// V pripade nalezeneho zaznamu <see cref="SuccessfulServiceCallResult{TModel}">SuccessfulServiceCallResult&lt;AddressDetail&gt;</see>
    /// V pripade zadneho nalezeneho vysledku <see cref="EmptyServiceCallResult"></see>
    /// </returns>
    Task<IServiceCallResult> GetAddressDetail(string sessionId, string addressId, string title, string country, CancellationToken cancellationToken = default(CancellationToken));
}
