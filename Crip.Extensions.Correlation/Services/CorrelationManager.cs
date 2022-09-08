using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Services;

public class CorrelationManager : ICorrelationManager
{
    private static readonly AsyncLocal<string?> AsyncLocal = new();
    private readonly IOptions<CorrelationIdOptions> _options;

    public CorrelationManager(IOptions<CorrelationIdOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void Set(string correlationId)
    {
        AsyncLocal.Value = correlationId;
    }

    public string? Get() => AsyncLocal.Value;

    public Dictionary<string, object?> Scope() =>
        new() { { _options.Value.PropertyName, Get() } };
}