using Microsoft.Extensions.DependencyInjection;
using System;

namespace OLT.Core;

/// <summary>
/// Provides extension methods for adding AutoMapper services to the IServiceCollection.
/// </summary>
public static class OltServiceCollectionAutoMapperExtensions
{
    /// <summary>
    /// Adds AutoMapper services to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="licenseKey">License key for AutoMapper.</param>
    /// <param name="action">An action to configure the OltAutoMapperBuilder.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection AddOltAutoMapper(this IServiceCollection services, string licenseKey, Action<OltAutoMapperBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(licenseKey);
        var builder = new OltAutoMapperBuilder(services);
        action(builder);
        builder.Build(licenseKey);
        return services;
    }
    
}
