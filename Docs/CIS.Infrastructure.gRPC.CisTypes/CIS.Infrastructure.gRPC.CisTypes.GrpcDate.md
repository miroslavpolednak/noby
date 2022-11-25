#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## GrpcDate Class

```csharp
public sealed class GrpcDate :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.GrpcDate>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.GrpcDate>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.GrpcDate>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcDate

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[GrpcDate](CIS.Infrastructure.gRPC.CisTypes.GrpcDate.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[GrpcDate](CIS.Infrastructure.gRPC.CisTypes.GrpcDate.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[GrpcDate](CIS.Infrastructure.gRPC.CisTypes.GrpcDate.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.DayFieldNumber'></a>

## GrpcDate.DayFieldNumber Field

Field number for the "day" field.

```csharp
public const int DayFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.MonthFieldNumber'></a>

## GrpcDate.MonthFieldNumber Field

Field number for the "month" field.

```csharp
public const int MonthFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.YearFieldNumber'></a>

## GrpcDate.YearFieldNumber Field

Field number for the "year" field.

```csharp
public const int YearFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.Day'></a>

## GrpcDate.Day Property

Day of a month. Must be from 1 to 31 and valid for the year and month, or 0  
to specify a year by itself or a year and month where the day isn't  
significant.

```csharp
public int Day { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.Month'></a>

## GrpcDate.Month Property

Month of a year. Must be from 1 to 12, or 0 to specify a year without a  
month and day.

```csharp
public int Month { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDate.Year'></a>

## GrpcDate.Year Property

Year of the date. Must be from 1 to 9999, or 0 to specify a date without  
a year.

```csharp
public int Year { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')