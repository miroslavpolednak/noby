#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## PaginationRequest Class

```csharp
public sealed class PaginationRequest :
CIS.Core.Types.IPaginableRequest,
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.PaginationRequest>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.PaginationRequest>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.PaginationRequest>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; PaginationRequest

Implements [CIS.Core.Types.IPaginableRequest](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest 'CIS.Core.Types.IPaginableRequest'), [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[PaginationRequest](CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.md 'CIS.Infrastructure.gRPC.CisTypes.PaginationRequest')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[PaginationRequest](CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.md 'CIS.Infrastructure.gRPC.CisTypes.PaginationRequest')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[PaginationRequest](CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.md 'CIS.Infrastructure.gRPC.CisTypes.PaginationRequest')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.PageSizeFieldNumber'></a>

## PaginationRequest.PageSizeFieldNumber Field

Field number for the "pageSize" field.

```csharp
public const int PageSizeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.RecordOffsetFieldNumber'></a>

## PaginationRequest.RecordOffsetFieldNumber Field

Field number for the "recordOffset" field.

```csharp
public const int RecordOffsetFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.SortingFieldNumber'></a>

## PaginationRequest.SortingFieldNumber Field

Field number for the "sorting" field.

```csharp
public const int SortingFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')