using Sonikku.Results.InferencedTypes;

namespace Sonikku.Results;

/// <summary>
/// Результат со значением, реализующим <see cref="IDisposable"/>.
/// Позволяет вызывать <see cref="IDisposable.Dispose()"/> в области видимости объекта результата.
/// </summary>
/// <typeparam name="TSuccess">Тип возвращаемого значения. Обязан реализовывать <see cref="IDisposable"/>.</typeparam>
public class DisposableResult<TSuccess> : Result<TSuccess>, IDisposable where TSuccess : IDisposable
{
    protected DisposableResult(SuccessResult<TSuccess> successResult) : base(successResult) { }

    protected DisposableResult(FailureResult failureResult) : base(failureResult) { }
    
    protected DisposableResult(AbortResult abortResult) : base(abortResult) { }
    
    
    public static implicit operator DisposableResult<TSuccess>(SuccessResult<TSuccess> success) =>
        new(success);
    
    public static implicit operator DisposableResult<TSuccess>(FailureResult failure) =>
        new(failure);

    public static implicit operator DisposableResult<TSuccess>(AbortResult abort) =>
        new(abort);

    public static implicit operator DisposableResult<TSuccess>(TSuccess value) =>
        new(new SuccessResult<TSuccess>(value));

    public static implicit operator DisposableResult<TSuccess>(string failureMessage) =>
        new(new FailureResult(failureMessage));

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            Value?.Dispose();
        }
    }
    
    public virtual void Dispose()
    {
        Dispose(true);
    }
}