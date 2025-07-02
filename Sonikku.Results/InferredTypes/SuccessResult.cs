namespace Sonikku.Results.InferredTypes;

/// <summary>
/// Успешный результат.
/// </summary>
/// <remarks>Предназначен для внутреннего применения.</remarks>
public class SuccessResult
{
    internal Result? InnerResult { get; }

    internal SuccessResult(Result? innerResult = null)
    {
        InnerResult = innerResult;
    }
}

/// <summary>
/// Параметризованный успешный результат со значением.
/// </summary>
/// <remarks>Предназначен для внутреннего применения.</remarks>
public class SuccessResult<TSuccess>
{
    internal Result? InnerResult { get; }
    
    internal TSuccess Value { get; }
    
    internal SuccessResult(TSuccess value)
    {
        Value = value;
    }

    internal SuccessResult(TSuccess value, Result innerResult)
    {
        Value = value;
        InnerResult = innerResult;
    }
}