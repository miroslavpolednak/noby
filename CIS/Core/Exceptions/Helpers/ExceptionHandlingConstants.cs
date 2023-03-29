namespace CIS.Core.Exceptions;

public struct ExceptionHandlingConstants
{
    /// <summary>
    /// Klíč v gRPC Trailers kolekci, který označuje záznam s CIS error kódy
    /// </summary>
    public const string GrpcTrailerCisCodeKey = "ciscode";

    /// <summary>
    /// Klíč v gRPC Trailers kolekci, který označuje záznam s názvem argumentu, který vyvolal vyjímku (CisArgumentException)
    /// </summary>
    public const string GrpcTrailerCisArgumentKey = "argument";
}