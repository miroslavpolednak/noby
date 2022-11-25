#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## UserInfo Class

Zakladni informace o uzivateli (poradci).  
Pouziva se hlavne jako info properta na entite kde neni moznost joinovani na tabulku uzivatelu.

```csharp
public sealed class UserInfo :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.UserInfo>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.UserInfo>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.UserInfo>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserInfo

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[UserInfo](CIS.Infrastructure.gRPC.CisTypes.UserInfo.md 'CIS.Infrastructure.gRPC.CisTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[UserInfo](CIS.Infrastructure.gRPC.CisTypes.UserInfo.md 'CIS.Infrastructure.gRPC.CisTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[UserInfo](CIS.Infrastructure.gRPC.CisTypes.UserInfo.md 'CIS.Infrastructure.gRPC.CisTypes.UserInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.UserInfo.UserIdFieldNumber'></a>

## UserInfo.UserIdFieldNumber Field

Field number for the "userId" field.

```csharp
public const int UserIdFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.UserInfo.UserNameFieldNumber'></a>

## UserInfo.UserNameFieldNumber Field

Field number for the "userName" field.

```csharp
public const int UserNameFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.UserInfo.UserId'></a>

## UserInfo.UserId Property

v33id uzivatele

```csharp
public int UserId { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.UserInfo.UserName'></a>

## UserInfo.UserName Property

Cele jmeno uzivatele

```csharp
public string UserName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')