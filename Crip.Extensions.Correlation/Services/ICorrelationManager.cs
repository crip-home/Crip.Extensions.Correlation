using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// Correlation identifier scope manager contract.
/// </summary>
public interface ICorrelationManager
{
    /// <summary>
    /// Set correlation identifier value.
    /// </summary>
    /// <param name="correlationId">Correlation identifier value</param>
    void Set(string correlationId);

    /// <summary>
    /// Get current correlation identifier value.
    /// </summary>
    /// <returns>Correlation identifier value.</returns>
    string? Get();

    /// <summary>
    /// Get correlation scope.
    /// </summary>
    /// <returns>Correlation scope.</returns>
    Dictionary<string, object?> Scope();

    /// <summary>
    /// Create correlation context for background task.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to use under context.</param>
    /// <param name="action">The action to execute in a context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Use(string correlationId, Func<Task> action);

    /// <summary>
    /// Create correlation context for background task.
    /// </summary>
    /// <param name="action">The action to execute in a context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Use(Func<Task> action);

    /// <summary>
    /// Create correlation context for background task.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to use under context.</param>
    /// <param name="action">The action to execute in a context.</param>
    void Use(string correlationId, Action action);

    /// <summary>
    /// Create correlation context for background task.
    /// </summary>
    /// <param name="action">The action to execute in a context.</param>
    void Use(Action action);
}