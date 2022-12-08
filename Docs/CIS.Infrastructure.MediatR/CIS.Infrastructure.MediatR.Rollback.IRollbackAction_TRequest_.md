#### [CIS.Infrastructure.MediatR](index.md 'index')
### [CIS.Infrastructure.MediatR.Rollback](CIS.Infrastructure.MediatR.Rollback.md 'CIS.Infrastructure.MediatR.Rollback')

## IRollbackAction<TRequest> Interface

Deklarace kontraktu pro tridu s kodem pro provedeni rollbacku Mediatr handleru.

```csharp
public interface IRollbackAction<TRequest>
    where TRequest : MediatR.IBaseRequest
```
#### Type parameters

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.TRequest'></a>

`TRequest`

Mediatr Request type
### Methods

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken)'></a>

## IRollbackAction<TRequest>.ExecuteRollback(Exception, TRequest, CancellationToken) Method

Metoda s implementaci vlastniho rollbacku

```csharp
System.Threading.Tasks.Task ExecuteRollback(System.Exception exception, TRequest request, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).exception'></a>

`exception` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

Vyjimka, ktera rollback spustila

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).request'></a>

`request` [TRequest](CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.md#CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.TRequest 'CIS.Infrastructure.MediatR.Rollback.IRollbackAction<TRequest>.TRequest')

Puvodni Mediatr request

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackAction_TRequest_.ExecuteRollback(System.Exception,TRequest,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')