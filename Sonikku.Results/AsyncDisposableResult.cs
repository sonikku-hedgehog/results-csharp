using Sonikku.Results.InferencedTypes;

namespace Sonikku.Results;

/// <summary>
/// Результат со значением, реализующим <see cref="IDisposable"/> и <see cref="IAsyncDisposable"/>.
/// Позволяет вызывать <see cref="IAsyncDisposable.DisposeAsync()"/> в области видимости объекта результата.
/// </summary>
/// <typeparam name="TSuccess">Тип возвращаемого значения. Обязан реализовывать
/// <see cref="IDisposable"/> и <see cref="IAsyncDisposable"/>.</typeparam>
public class AsyncDisposableResult<TSuccess> : 
    DisposableResult<TSuccess>, IDisposable, IAsyncDisposable 
    where TSuccess : class, IDisposable, IAsyncDisposable
{
    protected AsyncDisposableResult(SuccessResult<TSuccess> successResult) : base(successResult) { }

    protected AsyncDisposableResult(FailureResult failureResult) : base(failureResult) { }
    
    protected AsyncDisposableResult(AbortResult abortResult) : base(abortResult) { }
    
    
    public static implicit operator AsyncDisposableResult<TSuccess>(SuccessResult<TSuccess> success) =>
        new(success);
    
    public static implicit operator AsyncDisposableResult<TSuccess>(FailureResult failure) =>
        new(failure);

    public static implicit operator AsyncDisposableResult<TSuccess>(AbortResult abort) =>
        new(abort);

    public static implicit operator AsyncDisposableResult<TSuccess>(TSuccess value) =>
        new(new SuccessResult<TSuccess>(value));
    
    public static implicit operator AsyncDisposableResult<TSuccess>(string failure) =>
        new(new FailureResult(failure));

    public override void Dispose()
    {
        Dispose(true);
    }
    
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (Value is not null)
        {
            await Value.DisposeAsync().ConfigureAwait(false);
        }

        Value = null;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
    }
}