namespace Sonikku.Results.InferredTypes;

/// <summary>
/// Абортированный результат.
/// </summary>
/// <remarks>Предназначен для внутреннего применения.</remarks>
public class AbortResult
{
    internal Result? InnerResult { get; }
    
    internal AbortResult() { }

    internal AbortResult(Result innerResult)
    {
        InnerResult = innerResult;
    }
}