# CRIP AspNetCore Correlation

![issues](https://img.shields.io/github/issues/crip-home/Crip.Extensions.Correlation?style=for-the-badge&logo=appveyor)
![forks](https://img.shields.io/github/forks/crip-home/Crip.Extensions.Correlation?style=for-the-badge&logo=appveyor)
![stars](https://img.shields.io/github/stars/crip-home/Crip.Extensions.Correlation?style=for-the-badge&logo=appveyor)
![license](https://img.shields.io/github/license/crip-home/Crip.Extensions.Correlation?style=for-the-badge&logo=appveyor)

The correlation ID is meant to be used to help Admin trace what was happening at the time of an error/request. It
is only valuable as an interactive tool to help the Admin track down your error. An Admin uses the correlation ID as
"breadcrumbs" to retrace a requests or processes in the Logging System to find what leads up to
and causes a problem.

![Correlation usage diagram](correlation.drawio.png)

## Installation

Install [Crip.Extensions.Correlation NuGet package](https://www.nuget.org/packages/Crip.Extensions.Correlation),
or [GitHub package](https://github.com/orgs/crip-home/packages?repo_name=Crip.Extensions.Correlation)

## Setup

Configure services:

```csharp
services.AddHttpContextAccessor(); // package requires that HTTP context accessor is available
services.AddCorrelation();
```

Configure pipeline:

```csharp
// Add middlewares as early as possible
app.UseCorrelation();
// or
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<CorrelationIdLoggingMiddleware>();
```

## HTTP client correlation

Correlation makes sense only when entire system is using same Correlation ID. To do so, you can use "traced" HTTP client
witch will automatically add header to all outgoing requests.

```csharp
services.AddTracedHttpClient<TClient, TImplementation>(((provider, client) =>
{
    var config = provider.GetRequiredService<IOptions<TClientOptions>>().Value;
    client.BaseAddress = new Uri(config.BaseUrl);
}));
```

## Customization

You can customize correlation by providing custom options:

```csharp
services.Configure<CorrelationIdOptions>(options =>
{
    options.Cookie = "X-Correlation-Id"; // name of the cookie key to use as correlation identifier
    options.Header = "X-Correlation-Id"; // name of the HTTP request/response header
    options.PropertyName = "CorrelationId"; // name of the loggable property
    options.IncludeInResponse = true; // include correlation identifier header in responses
});
```