namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public interface ICodebookMap : IEnumerable<ICodebookEndpoint>
{
    ICodebookEndpoint this[string code] { get; }
}