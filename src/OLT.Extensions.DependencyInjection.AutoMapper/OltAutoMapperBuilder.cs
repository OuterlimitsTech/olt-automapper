using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OLT.Core;

/// <summary>
/// Calls AddAutoMapper Extensions and register <seealso cref="IOltAdapterResolver"/> to <seealso cref="OltAdapterResolverAutoMapper"/>
/// </summary>
public class OltAutoMapperBuilder
{
    private readonly List<Assembly> _scanAssemblies = new List<Assembly>();
    private readonly List<Profile> _profiles = new List<Profile>();

    /// <summary>
    /// The service collection for dependency injection.
    /// </summary>
    protected readonly IServiceCollection _services;

    /// <summary>
    /// The service lifetime for Automapper.
    /// </summary>
    protected ServiceLifetime _serviceLifetime = ServiceLifetime.Transient;

    /// <summary>
    /// Requires <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services"></param>
    public OltAutoMapperBuilder(IServiceCollection services)
    {
        _services = services;            
    }

    /// <summary>
    /// Set the <see cref="ServiceLifetime"/> for Automapper
    /// </summary>
    /// <remarks>
    /// Default is <see cref="ServiceLifetime.Transient"/>
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual OltAutoMapperBuilder WithServiceLifetime(ServiceLifetime value)
    {
        _serviceLifetime = value;
        return this;
    }


    /// <summary>
    /// Add mapping definitions contained in assemblies.
    /// Looks for <see cref="Profile" /> definitions and classes decorated with <see cref="AutoMapAttribute" />
    /// </summary>
    /// <param name="assembliesToScan">Assemblies containing mapping definitions</param>
    public virtual OltAutoMapperBuilder AddMaps(IEnumerable<Assembly> assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }


    /// <summary>
    /// Add mapping definitions contained in assemblies.
    /// Looks for <see cref="Profile" /> definitions and classes decorated with <see cref="AutoMapAttribute" />
    /// </summary>
    /// <param name="assembliesToScan">Assemblies containing mapping definitions</param>
    public virtual OltAutoMapperBuilder AddMaps(params Assembly[] assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }

    /// <summary>
    /// Add profiles contained in an IEnumerable
    /// </summary>
    /// <param name="profiles">IEnumerable of Profile</param>
    public virtual OltAutoMapperBuilder AddProfiles(params Profile[] profiles)
    {
        _profiles.AddRange(profiles);
        return this;
    }

    /// <summary>
    /// Builds the AutoMapper configuration and registers the necessary services.
    /// </summary>
    /// <param name="configAction">An optional action to configure the AutoMapper.</param>
    public virtual void Build(Action<IMapperConfigurationExpression>? configAction = null)
    {
        _services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();

        _services.AddAutoMapper(cfg =>
        {
            cfg.AddCollectionMappers();
            cfg.AddProfiles(_profiles);
            configAction?.Invoke(cfg);
        }, _scanAssemblies, _serviceLifetime);

    }

}
