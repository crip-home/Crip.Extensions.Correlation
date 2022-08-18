using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Crip.Extensions.Correlation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Crip.Extensions.Correlation;

/// <summary>
/// Service collection configuration extensions.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    /// <summary>
    /// Adds the <see cref="ICorrelationService"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>An updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCorrelation(this IServiceCollection services) => services
        .AddTransient<ICorrelationService, CorrelationService>()
        .AddTransient<IHttpCorrelationAccessor, HttpCorrelationAccessor>()
        .AddTransient<CorrelationIdHeaderHandler>();

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
    /// a binding between the <typeparamref name="TClient" /> type and a named <see cref="HttpClient"/>. The client name will
    /// be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <typeparam name="TClient">
    /// The type of the typed client. The type specified will be registered in the service collection as
    /// a transient service. See <see cref="ITypedHttpClientFactory{TClient}" /> for more details about authoring typed clients.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The implementation type of the typed client. The type specified will be instantiated by the
    /// <see cref="ITypedHttpClientFactory{TImplementation}"/>.
    /// </typeparam>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddTracedHttpClient<TClient, TImplementation>(
        this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient => services
        .AddHttpClient<TClient, TImplementation>()
        .AddHttpMessageHandler<CorrelationIdHeaderHandler>();

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
    /// a binding between the <typeparamref name="TClient" /> type and a named <see cref="HttpClient"/>. The client name will
    /// be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    /// <typeparam name="TClient">
    /// The type of the typed client. The type specified will be registered in the service collection as
    /// a transient service. See <see cref="ITypedHttpClientFactory{TClient}" /> for more details about authoring typed clients.
    /// </typeparam>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddTracedHttpClient<TClient>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
        where TClient : class => services
        .AddHttpClient<TClient>(configureClient)
        .AddHttpMessageHandler<CorrelationIdHeaderHandler>();

    /// <summary>
    /// /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
    /// a binding between the <typeparamref name="TClient" /> type and a named <see cref="HttpClient"/>. The client name will
    /// be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
    /// <typeparam name="TClient">
    /// The type of the typed client. The type specified will be registered in the service collection as
    /// a transient service. See <see cref="ITypedHttpClientFactory{TClient}" /> for more details about authoring typed clients.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The implementation type of the typed client. The type specified will be instantiated by the
    /// <see cref="ITypedHttpClientFactory{TImplementation}"/>.
    /// </typeparam>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddTracedHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        Action<HttpClient> configureClient)
        where TClient : class
        where TImplementation : class, TClient => services
        .AddHttpClient<TClient, TImplementation>(configureClient)
        .AddHttpMessageHandler<CorrelationIdHeaderHandler>();

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
    /// a binding between the <typeparamref name="TClient" /> type and a named <see cref="HttpClient"/>. The client name will
    /// be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
    /// <typeparam name="TClient">
    /// The type of the typed client. The type specified will be registered in the service collection as
    /// a transient service. See <see cref="ITypedHttpClientFactory{TClient}" /> for more details about authoring typed clients.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The implementation type of the typed client. The type specified will be instantiated by the
    /// <see cref="ITypedHttpClientFactory{TImplementation}"/>.
    /// </typeparam>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddTracedHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
        where TClient : class
        where TImplementation : class, TClient => services
        .AddHttpClient<TClient, TImplementation>(configureClient)
        .AddHttpMessageHandler<CorrelationIdHeaderHandler>();
}