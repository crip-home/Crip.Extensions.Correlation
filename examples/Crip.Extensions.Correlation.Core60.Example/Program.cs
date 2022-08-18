using Crip.Extensions.Correlation;
using Crip.Extensions.Correlation.Core60.Example.Clients;
using Crip.Extensions.Correlation.Core60.Example.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<CorrelationIdOptions>(options =>
{
    options.IncludeInResponse = true;
});

AddTestClient(builder);

builder.Services.AddCorrelation();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCorrelation();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void AddTestClient(WebApplicationBuilder builder)
{
    var services = builder.Services;
    var configuration = builder.Configuration;

    services.Configure<TestControllerClientOptions>(configuration.GetSection("TestController"));
    services.AddTracedHttpClient<ITestControllerClient, TestControllerClient>((provider, client) =>
    {
        var config = provider.GetRequiredService<IOptions<TestControllerClientOptions>>().Value;
        client.BaseAddress = new Uri(config.BaseUrl);
    });
}