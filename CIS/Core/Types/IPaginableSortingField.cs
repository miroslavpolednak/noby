namespace CIS.Core.Types;

public interface IPaginableSortingField
{
    string Field { get; }
    bool Descending { get; }
}