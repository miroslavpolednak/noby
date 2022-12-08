#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## UserIdentity Class

Jednoznacna identifikace uzivatele

```csharp
public sealed class UserIdentity :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.UserIdentity>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.UserIdentity>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.UserIdentity>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserIdentity

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[UserIdentity](CIS.Infrastructure.gRPC.CisTypes.UserIdentity.md 'CIS.Infrastructure.gRPC.CisTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[UserIdentity](CIS.Infrastructure.gRPC.CisTypes.UserIdentity.md 'CIS.Infrastructure.gRPC.CisTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[UserIdentity](CIS.Infrastructure.gRPC.CisTypes.UserIdentity.md 'CIS.Infrastructure.gRPC.CisTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.UserIdentity.IdentityFieldNumber'></a>

## UserIdentity.IdentityFieldNumber Field

Field number for the "identity" field.

```csharp
public const int IdentityFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.UserIdentity.IdentitySchemeFieldNumber'></a>

## UserIdentity.IdentitySchemeFieldNumber Field

Field number for the "identityScheme" field.

```csharp
public const int IdentitySchemeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Identity'></a>

## UserIdentity.Identity Property

login uzivatele

```csharp
public string Identity { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')