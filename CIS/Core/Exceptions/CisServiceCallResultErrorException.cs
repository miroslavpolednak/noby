using System.Collections.Immutable;

namespace CIS.Core.Exceptions;

/// <summary>
/// nevim? asi zrusit spolu s ServiceCallResult
/// </summary>
public sealed class CisServiceCallResultErrorException 
    : BaseCisException
{
    public IImmutableList<(int Key, string Message)> Errors => _result.Errors;
    
    public bool IsMultiError => _result.IsMultiError;

    private readonly Results.ErrorServiceCallResult _result;

    public CisServiceCallResultErrorException(int key, string message)
        : base(key, message)
    {
        _result = new Results.ErrorServiceCallResult(key, message);
    }

    public CisServiceCallResultErrorException(Results.ErrorServiceCallResult result)
        : base(result.Errors[0].Key, result.Errors[0].Message)
    {
        _result = result;
    }
}
