using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OLT.Core
{

    /// <summary>
    /// Provides extension methods for adding AutoMapper services to the IServiceCollection.
    /// </summary>
    public static class OltServiceCollectionAutoMapperExtensions
    {

        /// <summary>
        /// Adds AutoMapper services to the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="action">An action to configure the OltAutoMapperBuilder.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddOltAutoMapper(this IServiceCollection services, Action<OltAutoMapperBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(services);
            var builder = new OltAutoMapperBuilder(services);
            action(builder);
            builder.Build();
            return services;
        }



        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="filter">An optional filter to apply to the assembly scan.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        [Obsolete("Use AddOltAutoMapper")]
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, OltAutoMapperAssemblyFilter? filter = null)
        {
            return AddOltInjectionAutoMapper(services, new List<Assembly>(), null, ServiceLifetime.Transient, filter);
        }

        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="includeAssemblyScan">The assembly to include in the scan.</param>
        /// <param name="filter">An optional filter to apply to the assembly scan.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        /// <exception cref="ArgumentNullException">Thrown when includeAssemblyScan is null.</exception>
        [Obsolete("Use AddOltAutoMapper")]
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, Assembly includeAssemblyScan, OltAutoMapperAssemblyFilter? filter = null)
        {
            if (includeAssemblyScan == null)
            {
                throw new ArgumentNullException(nameof(includeAssemblyScan));
            }

            return AddOltInjectionAutoMapper(services, new List<Assembly> { includeAssemblyScan }, null, ServiceLifetime.Transient, filter);
        }

        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="includeAssembliesScan">The list of assemblies to include in the scan.</param>
        /// <param name="filter">An optional filter to apply to the assembly scan.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        [Obsolete("Use AddOltAutoMapper")]
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, List<Assembly> includeAssembliesScan, OltAutoMapperAssemblyFilter? filter = null)
        {
            return AddOltInjectionAutoMapper(services, includeAssembliesScan, null, ServiceLifetime.Transient, filter);
        }

        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="includeAssembliesScan">The list of assemblies to include in the scan.</param>
        /// <param name="configAction">An optional action to configure the IMapperConfigurationExpression.</param>
        /// <param name="serviceLifetime">The lifetime of the services to add.</param>
        /// <param name="filter">An optional filter to apply to the assembly scan.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
        [Obsolete("Use AddOltAutoMapper")]
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, List<Assembly> includeAssembliesScan, Action<IMapperConfigurationExpression>? configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient, OltAutoMapperAssemblyFilter? filter = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            includeAssembliesScan = includeAssembliesScan ?? new List<Assembly>();
            filter = filter ?? new OltAutoMapperAssemblyFilter();

            var baseAssemblies = new List<Assembly>
            {
                Assembly.GetExecutingAssembly()
            };

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                baseAssemblies.Add(entryAssembly);
            }

            baseAssemblies.AddRange(includeAssembliesScan);            

            var assemblyScanner = new OLT.Utility.AssemblyScanner.OltAssemblyScanBuilder();
            assemblyScanner
                .IncludeAssembly(baseAssemblies)
                .DeepScan()
                .ExcludeFilter(filter.ExcludeFilters.ToArray())
                .IncludeFilter(filter.Filters.ToArray())
                .ExcludeAutomapper()                
                .ExcludeMicrosoft();


            var assembliesToScan = assemblyScanner.Build();

            var builder = new OltAutoMapperBuilder(services);
            builder.WithServiceLifetime(serviceLifetime);
            builder.AddMaps(assembliesToScan);
            builder.Build(configAction);
            return services;
        }
    }
}
