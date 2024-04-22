using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public class OltAdapterResolverAutoMapper : OltAdapterResolver
    {
        public OltAdapterResolverAutoMapper(
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        protected virtual IMapper Mapper { get; }

        protected virtual bool HasAutoMap<TSource, TDestination>()
        {
            return Mapper.ConfigurationProvider.Internal().FindTypeMapFor<TSource, TDestination>() != null;
        }

        protected virtual OltAutoMapperException<TSource, TResult> BuildException<TSource, TResult>(Exception exception)
        {
            if (exception is AutoMapperMappingException autoMapperException)
            {
                return new OltAutoMapperException<TSource, TResult>(autoMapperException);
            }
            return new OltAutoMapperException<TSource, TResult>(exception);
        }

        public override bool CanMap<TSource, TDestination>()
        {
            return CanProjectTo<TSource, TDestination>() || base.CanMap<TSource, TDestination>();
        }

        #region [ ProjectTo Maps ]

        public override bool CanProjectTo<TSource, TDestination>()
        {
            return HasAutoMap<TSource, TDestination>() || base.CanProjectTo<TSource, TDestination>();
        }

        public override IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, Action<OltAdapterActionConfig>? configAction = null)
        {

            if (HasAutoMap<TSource, TDestination>())
            {
                var config = new OltAdapterActionConfig();
                configAction?.Invoke(config);

                try
                {
                    source = config.DisableBeforeMap ? source : ApplyBeforeMaps<TSource, TDestination>(source);
                    var mapped = source.ProjectTo<TDestination>(Mapper.ConfigurationProvider);
                    return config.DisableAfterMap ? mapped : ApplyAfterMaps<TSource, TDestination>(mapped);
                }
                catch (Exception ex)
                {
                    throw BuildException<TSource, TDestination>(ex);
                }
            }
            return base.ProjectTo<TSource, TDestination>(source, configAction);
        }

        #endregion

        #region [ Maps ]

        public override List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                try
                {
                    return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.AsEnumerable()).ToList();
                }
                catch (Exception exception)
                {
                    throw BuildException<TSource, TDestination>(exception);
                }
            }

            return base.Map<TSource, TDestination>(source);
        }


        public override TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                try
                {                    
                    return Mapper.Map(source, destination);
                }
                catch (Exception exception)
                {
                    throw BuildException<TSource, TDestination>(exception);
                }
            }

            return base.Map(source, destination);
        }

        #endregion

    }
}