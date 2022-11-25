#### [CIS.Infrastructure.gRPC.CisTypes](index.md 'index')
### [CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes.md 'CIS.Infrastructure.gRPC.CisTypes')

## NullableGrpcDateTime Class

```csharp
public sealed class NullableGrpcDateTime :
Google.Protobuf.IMessage<CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime>,
Google.Protobuf.IMessage,
System.IEquatable<CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime>,
Google.Protobuf.IDeepCloneable<CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; NullableGrpcDateTime

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[NullableGrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[NullableGrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[NullableGrpcDateTime](CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.md 'CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.DayFieldNumber'></a>

## NullableGrpcDateTime.DayFieldNumber Field

Field number for the "day" field.

```csharp
public const int DayFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.HoursFieldNumber'></a>

## NullableGrpcDateTime.HoursFieldNumber Field

Field number for the "hours" field.

```csharp
public const int HoursFieldNumber = 4;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.MinutesFieldNumber'></a>

## NullableGrpcDateTime.MinutesFieldNumber Field

Field number for the "minutes" field.

```csharp
public const int MinutesFieldNumber = 5;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.MonthFieldNumber'></a>

## NullableGrpcDateTime.MonthFieldNumber Field

Field number for the "month" field.

```csharp
public const int MonthFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.NanosFieldNumber'></a>

## NullableGrpcDateTime.NanosFieldNumber Field

Field number for the "nanos" field.

```csharp
public const int NanosFieldNumber = 7;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.SecondsFieldNumber'></a>

## NullableGrpcDateTime.SecondsFieldNumber Field

Field number for the "seconds" field.

```csharp
public const int SecondsFieldNumber = 6;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.YearFieldNumber'></a>

## NullableGrpcDateTime.YearFieldNumber Field

Field number for the "year" field.

```csharp
public const int YearFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Day'></a>

## NullableGrpcDateTime.Day Property

Day of a month. Must be from 1 to 31 and valid for the year and month, or 0  
to specify a year by itself or a year and month where the day isn't  
significant.

```csharp
public int Day { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Hours'></a>

## NullableGrpcDateTime.Hours Property

Required. Hours of day in 24 hour format. Should be from 0 to 23. An API  
may choose to allow the value "24:00:00" for scenarios like business  
closing time.

```csharp
public int Hours { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Minutes'></a>

## NullableGrpcDateTime.Minutes Property

Required. Minutes of hour of day. Must be from 0 to 59.

```csharp
public int Minutes { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Month'></a>

## NullableGrpcDateTime.Month Property

Month of a year. Must be from 1 to 12, or 0 to specify a year without a  
month and day.

```csharp
public int Month { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Nanos'></a>

## NullableGrpcDateTime.Nanos Property

Required. Fractions of seconds in nanoseconds. Must be from 0 to  
999,999,999.

```csharp
public int Nanos { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Seconds'></a>

## NullableGrpcDateTime.Seconds Property

Required. Seconds of minutes of the time. Must normally be from 0 to 59. An  
API may allow the value 60 if it allows leap-seconds.

```csharp
public int Seconds { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime.Year'></a>

## NullableGrpcDateTime.Year Property

Year of the date. Must be from 1 to 9999, or 0 to specify a date without  
a year.

```csharp
public int Year { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')