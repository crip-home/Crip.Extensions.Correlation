using System.Collections.Generic;

namespace Crip.Extensions.Correlation.Services;

public interface ICorrelationManager
{
    void Set(string correlationId);
    string? Get();
    Dictionary<string, object?> Scope();
}