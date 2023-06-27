#### [CIS.Infrastructure.CisMediatR](index.md 'index')
### [CIS.Infrastructure.CisMediatR.Rollback](CIS.Infrastructure.CisMediatR.Rollback.md 'CIS.Infrastructure.CisMediatR.Rollback')

## IRollbackAction<TRequest> Interface

Deklarace kontraktu pro tridu s kodem pro provedeni rollbacku Mediatr handleru.

```csharp
public interface IRollbackAction<TRequest>
    where TRequest : MediatR.IBaseRequest
```
#### Type parameters

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.TRequest'></a>

`TRequest`

Mediatr Request type
### Properties

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.OverrideThrownException'></a>

## IRollbackAction<TRequest>.OverrideThrownException Property

Pokud bude nastaveno na True, tak se misto exception, ktera rollback zpusobila, vrati exception z OverrideException()

```csharp
bool OverrideThrownException { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')
### Methods

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken)'></a>

## IRollbackAction<TRequest>.ExecuteRollback(Exception, TRequest, CancellationToken) Method

Metoda s implementaci vlastniho rollbacku

```csharp
System.Threading.Tasks.Task ExecuteRollback(System.Exception exception, TRequest request, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).exception'></a>

`exception` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

Vyjimka, ktera rollback spustila

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).request'></a>

`request` [TRequest](CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.md#CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.TRequest 'CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction<TRequest>.TRequest')

Puvodni Mediatr request

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.OnOverrideException(System.Exception)'></a>

## IRollbackAction<TRequest>.OnOverrideException(Exception) Method

Vytvoreni exception misto puvodni, ktera spustila rollback

```csharp
System.Exception OnOverrideException(System.Exception exception);
```
#### Parameters

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.OnOverrideException(System.Exception).exception'></a>

`exception` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

Puvodni vyjimka

#### Returns
[System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')