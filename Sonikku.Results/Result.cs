using Sonikku.Results.InferencedTypes;

namespace Sonikku.Results;

/// <summary>
/// Объект простого результата без значения.
/// </summary>
public class Result
{
    protected Result(SuccessResult success)
    {
        Outcome = Outcome.Ok;
        InnerResult = success.InnerResult;
    }

    protected Result(FailureResult failure)
    {
        Outcome = Outcome.Fail;
        InnerResult = failure.InnerResult;
        FailureMessage = failure.FailureMessage;
    }

    protected Result(AbortResult abortResult)
    {
        Outcome = Outcome.Abort;
        InnerResult = abortResult.InnerResult;
    }
    
    
    /// <summary>
    /// Вложенный <see cref="Result"/>.
    /// </summary>
    public Result? InnerResult { get; }
    
    /// <summary>
    /// Исход операции.
    /// </summary>
    public Outcome Outcome { get; }

    /// <summary>
    /// Сообщение об ошибке (пустая строка, если <see cref="Outcome"/> не является <see cref="Outcome.Fail"/>).
    /// </summary>
    public string FailureMessage { get; } = "";
    
    /// <summary>
    /// Показывает, успешна ли операция.
    /// </summary>
    public bool IsSuccessful => Outcome == Outcome.Ok;

    /// <summary>
    /// Показывает, неудачная ли операция.
    /// </summary>
    public bool IsFailed => Outcome == Outcome.Fail;

    /// <summary>
    /// Показывает, была ли операция прервана.
    /// </summary>
    public bool IsAborted => Outcome == Outcome.Abort;

    
    /// <summary>
    /// Создаёт успешный <see cref="Result"/> без значения.
    /// </summary>
    /// <returns>Объект результата, приведённый к ожидаемому типу.</returns>
    public static SuccessResult Success() => 
        new();

    /// <summary>
    /// Создаёт успешный <see cref="Result"/> со вложенным <see cref="Result"/>.
    /// </summary>
    /// <param name="innerResult">Вкладываемый результат.</param>
    /// <returns>
    /// Объект <see cref="Result"/> с успешным статусом и вложенным <see cref="Result"/>
    /// </returns>
    public static SuccessResult Success(Result innerResult) => 
        new(innerResult);
    
    /// <summary>
    /// Создаёт успешный параметризованный <see cref="Result"/> с возвращаемым значением заданного типа.
    /// </summary>
    /// <param name="value">Возвращаемое значение.</param>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    /// <returns>Успешный <see cref="Result"/>, содержащий значение.</returns>
    public static SuccessResult<T> Success<T>(T value) => 
        new(value);
    
    /// <summary>
    /// Создаёт успешный параметризованный <see cref="Result"/> с возвращаемым значением заданного типа
    /// и вложенным результатом.
    /// </summary>
    /// <param name="value">Возвращаемое значение.</param>
    /// <param name="innerResult">Вкладываемый результат.</param>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    /// <returns>Успешный <see cref="Result"/>, содержащий значение и вложенный результат.</returns>
    public static SuccessResult<T> Success<T>(T  value, Result innerResult) => 
        new(value, innerResult);

    /// <summary>
    /// Создаёт провальный <see cref="Result"/> без значения.
    /// </summary>
    /// <returns>Объект результата, приведённый к ожидаемому типу.</returns>
    public static FailureResult Fail() => 
        new();
    
    /// <summary>
    /// Создаёт провальный <see cref="Result"/> со вложенным <see cref="Result"/>.
    /// </summary>
    /// <param name="innerResult">Вкладываемый результат.</param>
    /// <returns>Объект <see cref="Result"/> со вложенным <see cref="Result"/>.</returns>
    public static FailureResult Fail(Result innerResult) => 
        new(innerResult: innerResult);

    /// <summary>
    /// Создаёт провальный <see cref="Result"/> с сообщением об ошибке.
    /// </summary>
    /// <param name="failureMessage">Сообщение об ошибке.</param>
    /// <returns>Объект <see cref="Result"/>, содержащий сообщение об ошибке.</returns>
    public static FailureResult Fail(string failureMessage) => 
        new(failureMessage);
    
    /// <summary>
    /// Создаёт провальный <see cref="Result"/> с сообщением об ошибке и вложенным <see cref="Result"/>. 
    /// </summary>
    /// <param name="failureMessage">Сообщение об ошибке.</param>
    /// <param name="innerResult">Вкладываемый результат.</param>
    /// <returns>Объект <see cref="Result"/>, содержащий сообщение об ошибке и вложенный <see cref="Result"/>.</returns>
    public static FailureResult Fail(string failureMessage, Result innerResult) => 
        new(failureMessage, innerResult);

    /// <summary>
    /// Создаёт абортированный <see cref="Result"/>.
    /// </summary>
    /// <returns>Объект <see cref="Result"/>, приведённый к ожидаемому типу.</returns>
    public static AbortResult Abort() => 
        new();

    /// <summary>
    /// Создаёт абортированный <see cref="Result"/> со вложенным <see cref="Result"/>.
    /// </summary>
    /// <param name="innerResult">Вкладываемый результат.</param>
    /// <returns>Объект <see cref="Result"/>, содержащий вложенный <see cref="Result"/>.</returns>
    public static AbortResult Abort(Result innerResult) => 
        new(innerResult);

    public static implicit operator Result(string failureMessage) => new(new FailureResult(failureMessage));
    
    public static implicit operator Result(SuccessResult successResult) => 
        new(successResult);

    public static implicit operator Result(FailureResult failureResult) =>
        new(failureResult);
    
    public static implicit operator Result(AbortResult abortResult) =>
        new(abortResult);
}