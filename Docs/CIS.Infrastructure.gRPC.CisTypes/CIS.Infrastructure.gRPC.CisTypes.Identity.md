#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## Identity Class

Jednoznacna identifikace klienta s ohledem na ruzne mandanty.

```csharp
public sealed class Identity :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.Identity>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.Identity>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.Identity>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Identity

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[Identity](CIS.Infrastructure.gRPC.CisTypes.Identity.md 'CIS.Infrastructure.gRPC.CisTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[Identity](CIS.Infrastructure.gRPC.CisTypes.Identity.md 'CIS.Infrastructure.gRPC.CisTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[Identity](CIS.Infrastructure.gRPC.CisTypes.Identity.md 'CIS.Infrastructure.gRPC.CisTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.Identity.IdentityIdFieldNumber'></a>

## Identity.IdentityIdFieldNumber Field

Field number for the "identityId" field.

```csharp
public const int IdentityIdFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.Identity.IdentitySchemeFieldNumber'></a>

## Identity.IdentitySchemeFieldNumber Field

Field number for the "identityScheme" field.

```csharp
public const int IdentitySchemeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.Identity.IdentityId'></a>

## Identity.IdentityId Property

Id klienta - bud ID_PARTNER nebo KBID

```csharp
public long IdentityId { get; set; }
```

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

<a name='CIS.Infrastructure.gRPC.CisTypes.Identity.IdentityScheme'></a>

## Identity.IdentityScheme Property

KB nebo MP nebo v budoucnu neco jineho

```csharp
public CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes IdentityScheme { get; set; }
```

#### Property Value
[CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes 'CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes')