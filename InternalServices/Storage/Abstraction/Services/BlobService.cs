namespace CIS.InternalServices.Storage.Abstraction.BlobStorage;

/// <summary>
/// Ukladani a cteni souboru na spolecne uloziste. Interne vola gRPC sluzbu InternalServices.Storage.
/// Pouziti Abstraction.BlobService:
/// V Startup.cs je nutne zaregistrovat Abstraction sluzbu:
/// .AddStorage(opt =>
/// {
///     opt.ServiceUrl = "https://localhost:5001";
///     opt.DefaultApplicationKey = "{default_app_key}";
/// })
/// ServiceUrl = URL gRPC sluzby InternalServices.Storage.
/// DefaultApplicationKey = Kazda aplikace vyuzivajici sluzbu InternalServicesStorage musi mit alespon jeden ApplicationKey. Vychozi ApplicationKey muze byt definovan v AddStorage().
/// </summary>
internal class BlobService : IBlobServiceAbstraction
{
    private readonly IMediator _mediator;

    public BlobService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Zapise data do uloziste.
    /// Interne uklada data do adresare ${applicationKey}.
    /// Kazda aplikace vyuzivajici sluzbu InternalServicesStorage musi mit alespon jede ApplicationKey.
    /// </summary>
    /// <param name="data">Data - soubor</param>
    /// <param name="name">Nazev souboru</param>
    /// <param name="contentType">MIME type souboru</param>
    /// <param name="applicationKey">Unikatni kod aplikace aktualni aplikace, ktera zapisuje data. Pokud neni nastaven zde, bere se vychozi DefaultApplicationKey z services.AddStorage().</param>
    /// <returns>ID nove vytvoreneho blobu</returns>
    /// <exception cref="ArgumentException">applicationKey neni validni; data = null</exception>
    /// <exception cref="ServiceUnavailableException">Sluzba neni dostupna</exception>
    public async Task<string> Save(byte[] data, string name = "", string contentType = "", string? applicationKey = null)
        => await _mediator.Send(new Dto.BlobSaveRequest(data, name, contentType, applicationKey));

    public async Task<Contracts.BlobGetResponse> Get(string blobKey)
        => await _mediator.Send(new Dto.BlobGetRequest(blobKey));

    public async Task Delete(string blobKey)
        => await _mediator.Send(new Dto.BlobDeleteRequest(blobKey));
}
