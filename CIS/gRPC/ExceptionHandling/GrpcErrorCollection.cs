﻿using CIS.Core.Exceptions;
using Grpc.Core;

namespace CIS.Infrastructure.gRPC;

public sealed class GrpcErrorCollection : List<GrpcErrorCollection.GrpcErrorCollectionItem>
{
    public GrpcErrorCollection() { }

    public GrpcErrorCollection(IEnumerable<GrpcErrorCollectionItem> errors) 
        : base(errors) { }

    public Metadata CreateTrailersFromErrors()
    {
        Metadata trailersCollection = new();

        // add exc codes
        trailersCollection.Add(ExceptionHandlingConstants.GrpcTrailerCisCodeKey, string.Join(",", this.Select(t => t.Code)));

        // add exc messages
        for (int i = 0; i < this.Count(); i++)
            trailersCollection.Add(this[i].CreateTrailerEntry(i));

        return trailersCollection;
    }

    public void Add(int exceptionCode, string message)
        => this.Add(new(exceptionCode, message));

    public bool ThrowExceptionIfErrors(StatusCode statusCode, string message)
    {
        if (this.Any())
            throw GrpcExceptionHelpers.CreateRpcException(statusCode, message, this);
        else
            return true;
    }

    public class GrpcErrorCollectionItem
    {
        public int Code { get; init; }
        public string Message { get; init; }

        public GrpcErrorCollectionItem(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public GrpcErrorCollectionItem(string code, string message)
        {
            if (int.TryParse(code, out int code2))
                Code = code2;
            Message = message;
        }

        public Metadata.Entry CreateTrailerEntry(int index)
        {
            return new Metadata.Entry($"ciserr_{index}_{this.Code}", this.Message);
        }
    }
}
