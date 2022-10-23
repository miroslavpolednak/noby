namespace CIS.InternalServices.DocumentDataAggregator;

public interface IDataAggregator
{
    Task<ICollection<KeyValuePair<string, object>>> GetDocumentData(int offerId);
}