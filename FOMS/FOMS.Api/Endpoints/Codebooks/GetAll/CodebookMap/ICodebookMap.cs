namespace FOMS.Api.Endpoints.Codebooks.GetAll.CodebookMap;

public interface ICodebookMap : IEnumerable<ICodebookEndpoint>
{
    ICodebookEndpoint this[string code] { get; }
}