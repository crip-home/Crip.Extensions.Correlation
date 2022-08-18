using System;
using Crip.Extensions.Correlation.Core50.Example.Clients;
using Crip.Extensions.Correlation.Core50.Example.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Core50.Example;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.Configure<CorrelationIdOptions>(options =>
        {
            options.IncludeInResponse = true;
        });

        services.Configure<TestControllerClientOptions>(Configuration.GetSection("TestController"));
        services.AddTracedHttpClient<ITestControllerClient, TestControllerClient>(((provider, client) =>
        {
            var config = provider.GetRequiredService<IOptions<TestControllerClientOptions>>().Value;
            client.BaseAddress = new Uri(config.BaseUrl);
        }));

        services.AddCorrelation();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelation();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}