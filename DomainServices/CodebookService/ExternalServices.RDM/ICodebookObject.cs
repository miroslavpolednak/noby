namespace DomainServices.CodebookService.ExternalServices.RDM;

public sealed class CodebookObject<T>
    where T : class, new()
{
    public ICodebookList<T> Codebook { get; set; }
}

public interface ICodebookList<T>
    where T : class, new()
{
    public List<T> CodebookEntries { get; set; }
}
