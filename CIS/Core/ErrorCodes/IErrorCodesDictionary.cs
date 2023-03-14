namespace CIS.Core.ErrorCodes;

/// <summary>
/// Slovník obsahující mapování chybových kódů na chybové hlášky.
/// Implementace slovníku je v base třídě ErrorCodeMapperBase, jiná implementace by neměla existovat.
/// </summary>
public interface IErrorCodesDictionary
    : IReadOnlyDictionary<int, string>
{
}