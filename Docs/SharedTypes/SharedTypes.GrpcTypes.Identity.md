#### [SharedTypes](index.md 'index')
### [SharedTypes.GrpcTypes](SharedTypes.GrpcTypes.md 'SharedTypes.GrpcTypes')

## Identity Class

Jednoznacna identifikace klienta s ohledem na ruzne mandanty.

```csharp
public sealed class Identity :
Google.Protobuf.IMessage<SharedTypes.GrpcTypes.Identity>,
Google.Protobuf.IMessage,
System.IEquatable<SharedTypes.GrpcTypes.Identity>,
Google.Protobuf.IDeepCloneable<SharedTypes.GrpcTypes.Identity>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Identity

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[Identity](SharedTypes.GrpcTypes.Identity.md 'SharedTypes.GrpcTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[Identity](SharedTypes.GrpcTypes.Identity.md 'SharedTypes.GrpcTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[Identity](SharedTypes.GrpcTypes.Identity.md 'SharedTypes.GrpcTypes.Identity')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='SharedTypes.GrpcTypes.Identity.IdentityIdFieldNumber'></a>

## Identity.IdentityIdFieldNumber Field

Field number for the "identityId" field.

```csharp
public const int IdentityIdFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.Identity.IdentitySchemeFieldNumber'></a>

## Identity.IdentitySchemeFieldNumber Field

Field number for the "identityScheme" field.

```csharp
public const int IdentitySchemeFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='SharedTypes.GrpcTypes.Identity.IdentityId'></a>

## Identity.IdentityId Property

Id klienta - bud ID_PARTNER nebo KBID

```csharp
public long IdentityId { get; set; }
```

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

<a name='SharedTypes.GrpcTypes.Identity.IdentityScheme'></a>

## Identity.IdentityScheme Property

KB nebo MP nebo v budoucnu neco jineho

```csharp
public SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes IdentityScheme { get; set; }
```

#### Property Value
[SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes 'SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes')