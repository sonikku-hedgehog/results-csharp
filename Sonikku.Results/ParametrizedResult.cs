using Sonikku.Results.InferredTypes;

namespace Sonikku.Results;

/// <summary>
/// Параметризованный результат со значением.
/// </summary>
/// <typeparam name="TSuccess">Тип возвращаемого значения.</typeparam>
public class Result<TSuccess> : Result
{
    protected Result(SuccessResult<TSuccess> successResult) : base(new SuccessResult(successResult.InnerResult))
    {
        Value = successResult.Value;
    }

    protected Result(FailureResult failureResult) : base(failureResult)
    { }
    
    protected Result(AbortResult abortResult) : base(abortResult) 
    { }
    
    
    /// <summary>
    /// Возвращаемое значение.
    /// </summary>
    public TSuccess? Value { get; protected set; }
    
    
    public static implicit operator Result<TSuccess>(SuccessResult<TSuccess> success) =>
        new(success);
    
    public static implicit operator Result<TSuccess>(FailureResult failure) =>
        new(failure);

    public static implicit operator Result<TSuccess>(AbortResult abort) =>
        new(abort);

    public static implicit operator Result<TSuccess>(TSuccess value) =>
        new(new SuccessResult<TSuccess>(value));
    
    public static implicit operator Result<TSuccess>(string failureMessage) =>
        new(new FailureResult(failureMessage));
}