using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public interface IDocumentGeneratorServiceClient
{
    /// <summary>
    /// Vygenerování a naplnění PDF dle zadaných vstupních dat.
    /// </summary>
    /// <remarks>
    /// Metoda naplní požadované PDF (podle typu, verze a varianty) daty, které příjdou na vstupu.
    /// Data se získávají přes službu <a href="https://wiki.kb.cz/display/HT/DataAggregatorService">DataAggregator</a> metodou <a href="https://wiki.kb.cz/display/HT/getDocumentData">GetDocumentData</a>.
    /// </remarks>
    /// <returns></returns>
    Task<Document> GenerateDocument(GenerateDocumentRequest request, CancellationToken cancellationToken = default);
}