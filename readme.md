# Result Object

This library contains classes implementing Result Object pattern
that allows you to get rid of exception throws. It will allow you
to improve code readability and the control flow.

## Table of contents

* [Features](#features)
* [Usage](#usage)
    * [Result](#result)
    * [Result\<TSuccess\>](#resulttsuccess)
    * [DisposableResult](#disposableresult)
    * [AsyncDisposableResult](#asyncdisposableresult)
    * [Type inference](#type-inference)
* [Common recommendations](#common-recommendations)

## Features

* Simple (without values) and complex (with values) result objects
* Strong typing
* Disposable results: allows you to dispose result object "in-place"
* Reliable syntax with type inference support

## Usage

There are several static methods to create result objects:

```csharp
Result success = Result.Success();
Result failure = Result.Failure();
Result abort = Result.Abort();
```

The arguments of these functions may vary depending on
the desired result status and the type of the value.

```csharp
// Simple result with no value
Result success = Result.Success();
// Parametrized result
Result<int> intSuccess = Result.Success(1);
// Error message string
Result failure = Result.Failure("Error message"); 
// Successful result with IDisposable value argument
DisposableResult<MemoryStream> disposableResult = Result.Success(new MemoryStream()); 
```

The following properties are purposed to check the result status and
present in every `Result` object variation:

```csharp
public Outcome Outcome { get; }

public bool IsSuccessful => Outcome == Outcome.Ok;

public bool IsFailed => Outcome == Outcome.Fail;

public bool IsAborted => Outcome == Outcome.Abort;
```

You can also include one `Result` object into another by passing
it as `innerResult` to any creation method. It allows you to simulate
stack trace.

```csharp
Result result1 = Result.Fail();
Result<int> result2 = Result.Fail("Fail", innerResult: result1);
```

### Result

A simple result object without value. Recommended for use as a replacement
for `void` methods that throw exceptions.

Example:

```csharp
// Original methods
public void WriteOrThrow(bool shouldThrow)
{
    if (shouldThrow)
        throw new Exception();
    Console.WriteLine("Written");
}

// Result-based implementation
public Result Write(bool shouldWrite)
{
    if (!shouldWrite)
        return Result.Fail();
    Console.WriteLine("Written");
    return Result.Success();
}
```

### Result\<TSuccess\>

A result object with value. Recommended for use in methods that return value
or error message to get rid of exception throwing.

```csharp
// Original method
public User GetUserById(int id)
{
    if (!_users.TryGetValue(id, out User user))
        throw new Exception($"User with ID {id} not found");
    return user;
}

// Result-based implementation
public Result<User> GetUserById(int id)
{
    if (!_users.TryGetValue(id, out User user))
        return Result.Fail($"User with ID {id} not found");
    return Result.Success(user);
}

// With type inference
public Result<User> GetUserById(int id)
{
    if (!_users.TryGetValue(id, out User user))
        return $"User with ID {id} not found";
    return user;
}
```

### DisposableResult

A result object that supports value disposal. The value type, of course,
should implement `IDisposable`. It is recommended for use with `using` keyword
that guarantees `Value?.Dispose()` at the end of caller scope.

Пример:

```csharp
// Original methods
public MemoryStream CreateStreamOrThrow(bool shouldThrow)
{
    if (shouldThrow)
        throw new Exception("No streams today, sorry");
    return new MemoryStream();
}

public void UseStream()
{
    try 
    {
        using MemoryStream memoryStream = CreateStreamOrThrow(false);
        memoryStream.Write([]);
    }
    catch (Exception ex) {
        throw new Exception("Failed to write to stream", ex);
    }
}

// Result-based implementation
public DisposableResult<MemoryStream> CreateStream(bool shouldCreate)
{
    if (!shouldCreate)
        return Result.Fail("No streams today, sorry");
    return Result.Success(new MemoryStream());
}

public Result UseStream()
{
    using DisposableResult<MemoryStream> streamCreateResult = CreateStream(true);
    if (streamCreateResult.IsFailed)
        return Result.Fail("Failed to write to stream", streamCreateResult);
    MemoryStream memoryStream = streamCreateResult.Value!;
    memoryStream.Write([]);
    return Result.Success();
}
```

**Note** that you can include `DisposableResult` into any other result object
as an `InnerResult`, but don't forget to use `using` keyword in-place to prevent
memory leaks and other harmful errors. Don't try to cast underlying `InnerResult`
to `DisposableResult` and get its `Value` because it will lead to undefined behavior.

### AsyncDisposableResult

The same as [DisposableResult](#disposableresult) but with `DisposeAsync()` support.
All rules of usage of `DisposableResult` are also applied to this, but pay
attention to official MSDN guidelines of how to use Async Dispose Pattern.

Despite that `IAsyncDisposable` itself does not force you to
implement non-async `Dispose()` in your object, `AsyncDisposableResult` does:
it is absolutely required to implement `IDisposable` in the returned value type
to prevent nasty memory leaks.

### Type inference

Examples:

```csharp
Result<int> result1 = Result.Success(1);
result1 = 0;

Result<int> result2 = Result.Success(2);
result2 = 2;
result2 = Result.Fail("Error!");
result2 = "Error!";
```

It looks quite simple. However, due to the implementation specifics of type inference,
there are some limitations:

* You can't use `var` keyword, because all `Result` objects are created by conversion
  from the internal type.
* You will have to use type casting in cases like this:
  ```csharp
  public ValueTask<Result> SampleMethodAsync()
  {
      // It may look redundant, but it is what it is :(
      return ValueTask.FromResult((Result)Result.Success());
  }
  ```
* Since the Result.Fail("Error message") function takes a string, 
  the Result<string> type cannot be inferred. 
  Specify the generic type manually.

## Common recommendations

* Use `Result` only in methods and functions as returned types
  and do not store them as fields or properties.
* Replace exceptions with `Result` objects.
* Do not throw exceptions from methods that return `Result`.
  ~~Actually, never throw exceptions at all~~ Throw them only
  when you need to ultimately crash your application.
* If you need to handle a value stored in `Result`, then do it in-place.
  Don't expect the outer method to cast it and its `Value` to the proper type.
* **Do not** forget to use `using` and `await using` keywords on `DisposableResult`
  and `AsyncDisposableResult` respectively.
* **Do not** cast `InnerResult` and its `Value`.
* If your pass `CancellationToken` into a method, take into account a possibility
  of `Aborted` outcome status. If you abort the execution of your method,
  consider returning `Result.Abort()`.