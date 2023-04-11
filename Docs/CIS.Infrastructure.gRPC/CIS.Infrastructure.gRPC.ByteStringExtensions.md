#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## ByteStringExtensions Class

```csharp
public static class ByteStringExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ByteStringExtensions
### Methods

<a name='CIS.Infrastructure.gRPC.ByteStringExtensions.ToArrayUnsafe(thisGoogle.Protobuf.ByteString)'></a>

## ByteStringExtensions.ToArrayUnsafe(this ByteString) Method

This method is memory efficient, there is no data copy (ToArray do copy of data).    
Byte array instance should only be passed to methods which treat the array contents as read-only.

```csharp
public static byte[] ToArrayUnsafe(this Google.Protobuf.ByteString bytesString);
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.ByteStringExtensions.ToArrayUnsafe(thisGoogle.Protobuf.ByteString).bytesString'></a>

`bytesString` [Google.Protobuf.ByteString](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.ByteString 'Google.Protobuf.ByteString')

#### Returns
[System.Byte](https://docs.microsoft.com/en-us/dotnet/api/System.Byte 'System.Byte')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')