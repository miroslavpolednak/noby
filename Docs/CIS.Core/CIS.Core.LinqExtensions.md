#### [CIS.Core](index.md 'index')
### [CIS.Core](CIS.Core.md 'CIS.Core')

## LinqExtensions Class

```csharp
public static class LinqExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LinqExtensions
### Methods

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int)'></a>

## LinqExtensions.SelectAsync<TSource,TResult>(this IEnumerable<TSource>, Func<TSource,Task<TResult>>, int) Method

https://stackoverflow.com/questions/35011656/async-await-in-linq-select

```csharp
public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TResult>> SelectAsync<TSource,TResult>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource,System.Threading.Tasks.Task<TResult>> method, int concurrency=int.MaxValue);
```
#### Type parameters

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TSource'></a>

`TSource`

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TResult'></a>

`TResult`
#### Parameters

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).source'></a>

`source` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[TSource](CIS.Core.LinqExtensions.md#CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TSource 'CIS.Core.LinqExtensions.SelectAsync<TSource,TResult>(this System.Collections.Generic.IEnumerable<TSource>, System.Func<TSource,System.Threading.Tasks.Task<TResult>>, int).TSource')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).method'></a>

`method` [System.Func&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')[TSource](CIS.Core.LinqExtensions.md#CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TSource 'CIS.Core.LinqExtensions.SelectAsync<TSource,TResult>(this System.Collections.Generic.IEnumerable<TSource>, System.Func<TSource,System.Threading.Tasks.Task<TResult>>, int).TSource')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[TResult](CIS.Core.LinqExtensions.md#CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TResult 'CIS.Core.LinqExtensions.SelectAsync<TSource,TResult>(this System.Collections.Generic.IEnumerable<TSource>, System.Func<TSource,System.Threading.Tasks.Task<TResult>>, int).TResult')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')

<a name='CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).concurrency'></a>

`concurrency` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

#### Returns
[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[TResult](CIS.Core.LinqExtensions.md#CIS.Core.LinqExtensions.SelectAsync_TSource,TResult_(thisSystem.Collections.Generic.IEnumerable_TSource_,System.Func_TSource,System.Threading.Tasks.Task_TResult__,int).TResult 'CIS.Core.LinqExtensions.SelectAsync<TSource,TResult>(this System.Collections.Generic.IEnumerable<TSource>, System.Func<TSource,System.Threading.Tasks.Task<TResult>>, int).TResult')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')