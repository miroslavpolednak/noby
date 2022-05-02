using System.Collections.Immutable;

namespace CIS.Core.Results;

public class ErrorServiceCallResult 
    : IServiceCallResult
{
    public IImmutableList<(int Key, string Message)> Errors { get; init; }
    public bool Success => false;
    public bool IsMultiError { get; init; }

    public ErrorServiceCallResult(int key, string message)
    {
        Errors = ImmutableList.Create<(int Key, string Message)>((key, message));
    }

    public ErrorServiceCallResult(IEnumerable<(int Key, string Message)> errors)
    {
        Errors = ImmutableList.CreateRange<(int Key, string Message)>(errors);
        IsMultiError = true;
    }

    public override string ToString()
    {
        return Errors.Any() ? Errors[0].Message : "";
    }
}
