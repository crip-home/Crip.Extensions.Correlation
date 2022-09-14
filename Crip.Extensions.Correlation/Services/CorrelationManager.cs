using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// Correlation identifier scope manager.
/// </summary>
public class CorrelationManager : ICorrelationManager
{
    private static readonly AsyncLocal<string?> AsyncLocal = new();
    private readonly IOptions<CorrelationIdOptions> _options;
    private readonly ILogger<CorrelationManager> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationManager"/> class.
    /// </summary>
    /// <param name="options">The correlation options.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="options"/> are not provided.
    /// </exception>
    public CorrelationManager(IOptions<CorrelationIdOptions> options, ILogger<CorrelationManager> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Set(string correlationId)
    {
        AsyncLocal.Value = correlationId;
    }

    /// <inheritdoc />
    public string? Get() => AsyncLocal.Value;

    /// <inheritdoc />
    public Dictionary<string, object?> Scope() => ScopeOf(Get());

    /// <inheritdoc />
    public async Task Use(string correlationId, Func<Task> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        Set(correlationId);

        using (_logger.BeginScope(ScopeOf(correlationId))) await action();
    }

    /// <inheritdoc />
    public Task Use(Func<Task> action) => Use(string.Empty, action);

    /// <inheritdoc />
    public void Use(string correlationId, Action action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        Set(correlationId);

        using (_logger.BeginScope(ScopeOf(correlationId))) action.Invoke();
    }

    /// <inheritdoc />
    public void Use(Action action) => Use(string.Empty, action);

    private Dictionary<string, object?> ScopeOf(string? correlationId) =>
        new() { { _options.Value.PropertyName, CorrelationId(correlationId) } };

    private string CorrelationId(string? correlationId) =>
        string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId!;
}