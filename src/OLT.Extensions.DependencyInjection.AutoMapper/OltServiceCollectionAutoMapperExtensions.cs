using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{

    public static class OltServiceCollectionAutoMapperExtensions
    {
        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, OltAutoMapperAssemblyFilter? filter = null)
        {
            return AddOltInjectionAutoMapper(services, new List<Assembly>(), null, ServiceLifetime.Transient, filter);
        }

        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="includeAssemblyScan"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// <param name="services"></param>
        /// <param name="includeAssembliesScan"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services,  List<Assembly> includeAssembliesScan, OltAutoMapperAssemblyFilter? filter = null)
        {
            return AddOltInjectionAutoMapper(services, includeAssembliesScan, null, ServiceLifetime.Transient, filter);
        }

        /// <summary>
        /// Scans for automapper profiles using <seealso cref="ServiceCollectionExtensions.AddAutoMapper(IServiceCollection, Action{IMapperConfigurationExpression})"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="includeAssembliesScan"></param>
        /// <param name="configAction"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, List<Assembly> includeAssembliesScan, Action<IMapperConfigurationExpression>? configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient, OltAutoMapperAssemblyFilter? filter = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (includeAssembliesScan == null)
            {
                includeAssembliesScan = new List<Assembly>();
            }

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
            var assembliesToScan = baseAssemblies.GetAllReferencedAssemblies().ToList();

            filter.RemoveAllExclusions(assembliesToScan);

            services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
                configAction?.Invoke(cfg);
            }, assembliesToScan, serviceLifetime);
            return services;
        }

        
    }
}
