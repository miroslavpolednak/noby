#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## ModificationStamp Class

Informace o tom kdo a kdy menil/zalozil danou entitu.

```csharp
public sealed class ModificationStamp :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.ModificationStamp>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.ModificationStamp>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.ModificationStamp>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ModificationStamp

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[ModificationStamp](CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.md 'CIS.Infrastructure.gRPC.CisTypes.ModificationStamp')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[ModificationStamp](CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.md 'CIS.Infrastructure.gRPC.CisTypes.ModificationStamp')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[ModificationStamp](CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.md 'CIS.Infrastructure.gRPC.CisTypes.ModificationStamp')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.DateTimeFieldNumber'></a>

## ModificationStamp.DateTimeFieldNumber Field

Field number for the "dateTime" field.

```csharp
public const int DateTimeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.UserIdFieldNumber'></a>

## ModificationStamp.UserIdFieldNumber Field

Field number for the "userId" field.

```csharp
public const int UserIdFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.UserNameFieldNumber'></a>

## ModificationStamp.UserNameFieldNumber Field

Field number for the "userName" field.

```csharp
public const int UserNameFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.DateTime'></a>

## ModificationStamp.DateTime Property

Cas zmeny

```csharp
public CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime DateTime { get; set; }
```

#### Property Value
[GrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime')

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.UserId'></a>

## ModificationStamp.UserId Property

v33id uzivatele

```csharp
public System.Nullable<int> UserId { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Infrastructure.gRPC.CisTypes.ModificationStamp.UserName'></a>

## ModificationStamp.UserName Property

Cele jmeno uzivatele

```csharp
public string UserName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')