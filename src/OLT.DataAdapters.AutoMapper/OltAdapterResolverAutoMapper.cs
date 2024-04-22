using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    /// <summary>
    /// AutoMapper Adapter Resolver
    /// </summary>
    public class OltAdapterResolverAutoMapper : OltAdapterResolver
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public OltAdapterResolverAutoMapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// <seealso cref="IMapper"/>
        /// </summary>
        protected virtual IMapper Mapper { get; }

        /// <summary>
        /// Has AutoMapper Map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        protected virtual bool HasAutoMap<TSource, TDestination>()
        {
            return Mapper.ConfigurationProvider.Internal().FindTypeMapFor<TSource, TDestination>() != null;
        }

        /// <summary>
        /// Build AutoMapper Exception
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual OltAutoMapperException<TSource, TResult> BuildException<TSource, TResult>(Exception exception)
        {
            if (exception is AutoMapperMappingException autoMapperException)
            {
                return new OltAutoMapperException<TSource, TResult>(autoMapperException);
            }
            return new OltAutoMapperException<TSource, TResult>(exception);
        }

        /// <summary>
        /// Can Map Between Models
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// Falls back to <seealso cref="OltAdapterResolver.CanMap{TSource, TDestination}"/>
        /// </remarks>
        public override bool CanMap<TSource, TDestination>()
        {
            return CanProjectTo<TSource, TDestination>() || base.CanMap<TSource, TDestination>();
        }

        #region [ ProjectTo Maps ]

        /// <summary>
        /// Can Project To
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// Falls back to <seealso cref="OltAdapterResolver.CanProjectTo{TSource, TDestination}"/>
        /// </remarks>
        public override bool CanProjectTo<TSource, TDestination>()
        {
            return HasAutoMap<TSource, TDestination>() || base.CanProjectTo<TSource, TDestination>();
        }


        /// <summary>
        /// Can Project <see cref="IQueryable"/> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        /// <remarks>
        /// Falls back to <seealso cref="OltAdapterResolver.CanProjectTo{TSource, TDestination}"/>
        /// </remarks>
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

        /// <summary>
        /// Map data between <typeparamref name="TSource"/> list and return new List of <typeparamref name="TDestination"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        /// Falls back to <seealso cref="OltAdapterResolver.Map{TSource, TDestination}(List{TSource})"/>
        /// </remarks>
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

        /// <summary>
        /// Map data between <typeparamref name="TSource"/> list and return new List of <typeparamref name="TDestination"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <remarks>
        /// Falls back to <seealso cref="OltAdapterResolver.Map{TSource, TDestination}(TSource, TDestination)"/>
        /// </remarks>
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