namespace Sonikku.Results;

/// <summary>
/// Тип результата.
/// </summary>
public enum Outcome
{
    /// <summary>
    /// Успешное завершение задачи
    /// </summary>
    Ok,
    /// <summary>
    /// Неудачное завершение задачи
    /// </summary>
    Fail,
    /// <summary>
    /// Задача отменена
    /// </summary>
    Abort
}