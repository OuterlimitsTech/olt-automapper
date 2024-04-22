using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLT.Core
{
    public static class OltAutomapperExtensions 
    {
        /// <summary>
        /// Sets default OrderBy of <typeparamref name="TSource"/> for <seealso cref="IOltPaged"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="expression"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMappingExpression<TSource, TDestination> WithOrderBy<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func)
            where TSource : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(func, false);
            return expression;
        }


        /// <summary>
        /// Registers a Before Map <typeparamref name="TSource"/> for Mapping Expression for <see cref="IQueryable"/> ProjectTo
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="expression"></param>
        /// <param name="beforeMap"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMappingExpression<TSource, TDestination> BeforeMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, IOltBeforeMap<TSource, TDestination> beforeMap)
            where TSource : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (beforeMap == null)
            {
                throw new ArgumentNullException(nameof(beforeMap));
            }

            OltAdapterMapConfigs.BeforeMap.Register(beforeMap, false);
            return expression;
        }

        /// <summary>
        /// Registers a After Map <typeparamref name="TSource"/> for Mapping Expression for <see cref="IQueryable"/> ProjectTo
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="expression"></param>
        /// <param name="afterMap"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMappingExpression<TSource, TDestination> AfterMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, IOltAfterMap<TSource, TDestination> afterMap)
            where TSource : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (afterMap == null)
            {
                throw new ArgumentNullException(nameof(afterMap));
            }

            OltAdapterMapConfigs.AfterMap.Register(afterMap, false);
            return expression;
        }

    }
}
