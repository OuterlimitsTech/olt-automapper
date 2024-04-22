using System;
using AutoMapper;

namespace OLT.Core
{
    /// <summary>
    /// AutoMapper DataAdapter Exception
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class OltAutoMapperException<TSource, TDestination> : OltException
    {
        /// <summary>
        /// Automapper Exception
        /// </summary>
        /// <param name="exception"></param>
        public OltAutoMapperException(AutoMapperMappingException exception) :
            base($"AutoMapper Exception while using map OLT AutoMapper Map: {typeof(TSource).FullName} -> {typeof(TDestination).FullName} {Environment.NewLine}{exception.Message}", exception)
        {

        }

        /// <summary>
        /// General AutoMapper Exception
        /// </summary>
        /// <param name="exception"></param>
        public OltAutoMapperException(Exception exception) :
            base($"AutoMapper Exception while using map OLT AutoMapper Map: {typeof(TSource).FullName} -> {typeof(TDestination).FullName}", exception)
        {
            
        }

    }
}