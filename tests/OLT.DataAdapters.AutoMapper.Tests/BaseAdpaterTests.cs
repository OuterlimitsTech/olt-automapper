using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using System.Collections.Generic;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public abstract class BaseAdpaterTests
    {
        private readonly List<Profile> DefaultMaps = new List<Profile> { new AutoMapperMaps() };

        protected void RegisterMaps(IServiceCollection services, IConfiguration configuration, List<Profile> maps)
        {
            var licenseKey = configuration.GetValue<string>("AUTOMAPPER_LICENSE_KEY");
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = licenseKey;
                maps.ForEach(map => cfg.AddProfile(map));                
            });
        }

        protected ServiceProvider BuildProvider(List<Profile>? maps = null)
        {

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<BaseAdpaterTests>()
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            //using var loggerProvider = new InMemoryLoggerProvider();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3Adapter>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject5PagedAdapter>();
            RegisterMaps(services, configuration, maps ?? DefaultMaps);
            return services.BuildServiceProvider();
        }
    }


}
