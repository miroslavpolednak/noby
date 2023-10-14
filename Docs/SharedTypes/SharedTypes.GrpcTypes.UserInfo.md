#### [SharedTypes](index.md 'index')
### [SharedTypes.GrpcTypes](SharedTypes.GrpcTypes.md 'SharedTypes.GrpcTypes')

## UserInfo Class

Zakladni informace o uzivateli (poradci).  
Pouziva se hlavne jako info properta na entite kde neni moznost joinovani na tabulku uzivatelu.

```csharp
public sealed class UserInfo :
Google.Protobuf.IMessage<SharedTypes.GrpcTypes.UserInfo>,
Google.Protobuf.IMessage,
System.IEquatable<SharedTypes.GrpcTypes.UserInfo>,
Google.Protobuf.IDeepCloneable<SharedTypes.GrpcTypes.UserInfo>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserInfo

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[UserInfo](SharedTypes.GrpcTypes.UserInfo.md 'SharedTypes.GrpcTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[UserInfo](SharedTypes.GrpcTypes.UserInfo.md 'SharedTypes.GrpcTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[UserInfo](SharedTypes.GrpcTypes.UserInfo.md 'SharedTypes.GrpcTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='SharedTypes.GrpcTypes.UserInfo.UserIdFieldNumber'></a>

## UserInfo.UserIdFieldNumber Field

Field number for the "userId" field.

```csharp
public const int UserIdFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.UserInfo.UserNameFieldNumber'></a>

## UserInfo.UserNameFieldNumber Field

Field number for the "userName" field.

```csharp
public const int UserNameFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='SharedTypes.GrpcTypes.UserInfo.UserId'></a>

## UserInfo.UserId Property

v33id uzivatele

```csharp
public int UserId { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.UserInfo.UserName'></a>

## UserInfo.UserName Property

Cele jmeno uzivatele

```csharp
public string UserName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')