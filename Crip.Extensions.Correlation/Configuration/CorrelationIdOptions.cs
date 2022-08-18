namespace Crip.Extensions.Correlation;

/// <summary>
/// Correlation identifier options.
/// </summary>
public class CorrelationIdOptions
{
    /// <summary>
    /// The default correlation logging property name.
    /// </summary>
    public const string CorrelationPropertyName = "CorrelationId";

    /// <summary>
    /// The default correlation header name.
    /// </summary>
    public const string CorrelationHeaderName = "X-Correlation-Id";

    /// <summary>
    /// The default correlation cookie name.
    /// </summary>
    public const string CorrelationCookieName = "X-Correlation-Id";

    /// <summary>
    /// Gets or sets the logging field name where the correlation identifier will be
    /// written.
    /// </summary>
    public string PropertyName { get; set; } = CorrelationPropertyName;

    /// <summary>
    /// Gets or sets the header field name where the correlation identifier will be
    /// sent/received from.
    /// </summary>
    public string Header { get; set; } = CorrelationHeaderName;

    /// <summary>
    /// Gets or sets the cookie field name where the correlation identifier will be
    /// sent/received from.
    /// </summary>
    public string Cookie { get; set; } = CorrelationCookieName;

    /// <summary>
    /// Gets or sets a value indicating whether correlation identifier should be returned
    /// in the response headers.
    /// </summary>
    public bool IncludeInResponse { get; set; } = true;
}