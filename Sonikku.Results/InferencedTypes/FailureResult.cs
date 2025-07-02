namespace Sonikku.Results.InferencedTypes;

/// <summary>
/// Провальный результат.
/// </summary>
/// <remarks>Предназначен для внутреннего применения.</remarks>
public class FailureResult
{
    internal Result? InnerResult { get; }

    internal string FailureMessage { get; }

    internal FailureResult(string failureMessage = "", Result? innerResult = null)
    {
        FailureMessage = failureMessage;
        InnerResult = innerResult;
    }
}