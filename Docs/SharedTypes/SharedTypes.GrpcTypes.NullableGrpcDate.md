#### [SharedTypes](index.md 'index')
### [SharedTypes.GrpcTypes](SharedTypes.GrpcTypes.md 'SharedTypes.GrpcTypes')

## NullableGrpcDate Class

```csharp
public sealed class NullableGrpcDate :
Google.Protobuf.IMessage<SharedTypes.GrpcTypes.NullableGrpcDate>,
Google.Protobuf.IMessage,
System.IEquatable<SharedTypes.GrpcTypes.NullableGrpcDate>,
Google.Protobuf.IDeepCloneable<SharedTypes.GrpcTypes.NullableGrpcDate>,
Google.Protobuf.IBufferMessage
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; NullableGrpcDate

Implements [Google.Protobuf.IMessage&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1')[NullableGrpcDate](SharedTypes.GrpcTypes.NullableGrpcDate.md 'SharedTypes.GrpcTypes.NullableGrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage-1 'Google.Protobuf.IMessage`1'), [Google.Protobuf.IMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IMessage 'Google.Protobuf.IMessage'), [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[NullableGrpcDate](SharedTypes.GrpcTypes.NullableGrpcDate.md 'SharedTypes.GrpcTypes.NullableGrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1'), [Google.Protobuf.IDeepCloneable&lt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1')[NullableGrpcDate](SharedTypes.GrpcTypes.NullableGrpcDate.md 'SharedTypes.GrpcTypes.NullableGrpcDate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IDeepCloneable-1 'Google.Protobuf.IDeepCloneable`1'), [Google.Protobuf.IBufferMessage](https://docs.microsoft.com/en-us/dotnet/api/Google.Protobuf.IBufferMessage 'Google.Protobuf.IBufferMessage')
### Fields

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.DayFieldNumber'></a>

## NullableGrpcDate.DayFieldNumber Field

Field number for the "day" field.

```csharp
public const int DayFieldNumber = 3;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.MonthFieldNumber'></a>

## NullableGrpcDate.MonthFieldNumber Field

Field number for the "month" field.

```csharp
public const int MonthFieldNumber = 2;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.YearFieldNumber'></a>

## NullableGrpcDate.YearFieldNumber Field

Field number for the "year" field.

```csharp
public const int YearFieldNumber = 1;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.Day'></a>

## NullableGrpcDate.Day Property

Day of a month. Must be from 1 to 31 and valid for the year and month, or 0  
to specify a year by itself or a year and month where the day isn't  
significant.

```csharp
public int Day { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.Month'></a>

## NullableGrpcDate.Month Property

Month of a year. Must be from 1 to 12, or 0 to specify a year without a  
month and day.

```csharp
public int Month { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='SharedTypes.GrpcTypes.NullableGrpcDate.Year'></a>

## NullableGrpcDate.Year Property

Year of the date. Must be from 1 to 9999, or 0 to specify a date without  
a year.

```csharp
public int Year { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')