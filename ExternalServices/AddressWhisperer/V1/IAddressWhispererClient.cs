using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.AddressWhisperer.V1;

public interface IAddressWhispererClient
    : IExternalServiceClient
{
    /// <summary>
    /// Vraci seznam nalezenych adres pro 'text'
    /// </summary>
    /// <param name="sessionId">Session ID vygenerovane konzumentem</param>
    /// <param name="text">Hledany retezec</param>
    /// <param name="pageSize">Pocet vracenych zaznamu</param>
    /// <param name="country">ISO kod zeme nebo null (bude doplnen CZ)</param>
    Task<List<Dto.FoundSuggestion>> GetSuggestions(string sessionId, string text, int pageSize, string? country, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId">SessionId generované přímo na frontendu (unikátní GUID)</param>
    /// <param name="addressId">Id adresy</param>
    /// <param name="country">Id státu z číselníku Country</param>
    /// <param name="title">Parametr title našeptávače adres</param>
    Task<Dto.AddressDetail?> GetAddressDetail(string sessionId, string addressId, string title, string country, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
