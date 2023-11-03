#### [SharedTypes](index.md 'index')
### [SharedTypes.GrpcTypes](SharedTypes.GrpcTypes.md 'SharedTypes.GrpcTypes')

## UserIdentity Class

Jednoznacna identifikace uzivatele

```csharp
public sealed class UserIdentity :
Google.Protobuf.IMessage<SharedTypes.GrpcTypes.UserIdentity>,
Google.Protobuf.IMessage,
System.IEquatable<SharedTypes.GrpcTypes.UserIdentity>,
Google.Protobuf.IDeepCloneable<SharedTypes.GrpcTypes.UserIdentity>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserIdentity

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[UserIdentity](SharedTypes.GrpcTypes.UserIdentity.md 'SharedTypes.GrpcTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[UserIdentity](SharedTypes.GrpcTypes.UserIdentity.md 'SharedTypes.GrpcTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[UserIdentity](SharedTypes.GrpcTypes.UserIdentity.md 'SharedTypes.GrpcTypes.UserIdentity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='SharedTypes.GrpcTypes.UserIdentity.IdentityFieldNumber'></a>

## UserIdentity.IdentityFieldNumber Field

Field number for the "identity" field.

```csharp
public const int IdentityFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.UserIdentity.IdentitySchemeFieldNumber'></a>

## UserIdentity.IdentitySchemeFieldNumber Field

Field number for the "identityScheme" field.

```csharp
public const int IdentitySchemeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='SharedTypes.GrpcTypes.UserIdentity.Identity'></a>

## UserIdentity.Identity Property

login uzivatele

```csharp
public string Identity { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')