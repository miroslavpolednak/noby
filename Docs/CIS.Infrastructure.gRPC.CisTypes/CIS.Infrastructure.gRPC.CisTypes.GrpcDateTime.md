#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## GrpcDateTime Class

```csharp
public sealed class GrpcDateTime :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcDateTime

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[GrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[GrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[GrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.DayFieldNumber'></a>

## GrpcDateTime.DayFieldNumber Field

Field number for the "day" field.

```csharp
public const int DayFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.HoursFieldNumber'></a>

## GrpcDateTime.HoursFieldNumber Field

Field number for the "hours" field.

```csharp
public const int HoursFieldNumber = 4;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.MinutesFieldNumber'></a>

## GrpcDateTime.MinutesFieldNumber Field

Field number for the "minutes" field.

```csharp
public const int MinutesFieldNumber = 5;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.MonthFieldNumber'></a>

## GrpcDateTime.MonthFieldNumber Field

Field number for the "month" field.

```csharp
public const int MonthFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.NanosFieldNumber'></a>

## GrpcDateTime.NanosFieldNumber Field

Field number for the "nanos" field.

```csharp
public const int NanosFieldNumber = 7;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.SecondsFieldNumber'></a>

## GrpcDateTime.SecondsFieldNumber Field

Field number for the "seconds" field.

```csharp
public const int SecondsFieldNumber = 6;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.YearFieldNumber'></a>

## GrpcDateTime.YearFieldNumber Field

Field number for the "year" field.

```csharp
public const int YearFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Day'></a>

## GrpcDateTime.Day Property

Required. Day of month. Must be from 1 to 31 and valid for the year and  
month.

```csharp
public int Day { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Hours'></a>

## GrpcDateTime.Hours Property

Required. Hours of day in 24 hour format. Should be from 0 to 23. An API  
may choose to allow the value "24:00:00" for scenarios like business  
closing time.

```csharp
public int Hours { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Minutes'></a>

## GrpcDateTime.Minutes Property

Required. Minutes of hour of day. Must be from 0 to 59.

```csharp
public int Minutes { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Month'></a>

## GrpcDateTime.Month Property

Required. Month of year. Must be from 1 to 12.

```csharp
public int Month { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Nanos'></a>

## GrpcDateTime.Nanos Property

Required. Fractions of seconds in nanoseconds. Must be from 0 to  
999,999,999.

```csharp
public int Nanos { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Seconds'></a>

## GrpcDateTime.Seconds Property

Required. Seconds of minutes of the time. Must normally be from 0 to 59. An  
API may allow the value 60 if it allows leap-seconds.

```csharp
public int Seconds { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.GrpcDateTime.Year'></a>

## GrpcDateTime.Year Property

Optional. Year of date. Must be from 1 to 9999, or 0 if specifying a  
datetime without a year.

```csharp
public int Year { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')