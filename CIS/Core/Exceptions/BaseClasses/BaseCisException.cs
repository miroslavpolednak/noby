﻿using System.Globalization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Base třída pro CIS vyjímky. Obsahuje vlastnost ExceptionCode, která určuje o jakou vyjímku se jedná.
/// </summary>
public abstract class BaseCisException
    : Exception
{
    /// <summary>
    /// Společný kód pro neznámou chybu
    /// </summary>
    public const int UnknownExceptionCode = 0;

    public string ExceptionCode { get; init; }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    protected BaseCisException(int exceptionCode, string? message)
        : this(exceptionCode.ToString(CultureInfo.InvariantCulture), message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    protected BaseCisException(int exceptionCode, string? message, Exception innerException)
        : this(exceptionCode.ToString(CultureInfo.InvariantCulture), message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    protected BaseCisException(string exceptionCode, string? message)
        : base(message)
    {
        ExceptionCode = exceptionCode;
    }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    protected BaseCisException(string exceptionCode, string? message, Exception innerException)
        : base(message, innerException)
    {
        ExceptionCode = exceptionCode;
    }
}