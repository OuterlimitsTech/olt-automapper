using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{


    public class AdapterMapConfigTests : BaseAdpaterTests
    {


        [Fact]
        public void BeforeMapExtensionTests()
        {
            var maps = new AdapterMapConfigMaps();

            IMappingExpression<AdapterObject4, AdapterObject5>? expression = null;
            IOltBeforeMap<AdapterObject4, AdapterObject5>? beforeMap = null;
            Func<IQueryable<AdapterObject4>, IOrderedQueryable<AdapterObject4>>? func = null;

            Assert.Throws<ArgumentNullException>(nameof(beforeMap), () => OltAutomapperExtensions.BeforeMap(maps.BeforeMapExpression, beforeMap!));
            Assert.Throws<ArgumentNullException>(nameof(func), () => OltAutomapperExtensions.WithOrderBy(maps.BeforeMapExpression, func!));

            var orderBy = new OltBeforeMapOrderBy<AdapterObject4, AdapterObject5>(p => p.OrderBy(t => t.Name!.First));

            Assert.Throws<ArgumentNullException>(nameof(expression), () => OltAutomapperExtensions.WithOrderBy(expression!, p => p.OrderBy(p => p.Name!.Last)));
            Assert.Throws<ArgumentNullException>(nameof(expression), () => OltAutomapperExtensions.BeforeMap(expression!, orderBy));

            try
            {
                OltAutomapperExtensions.WithOrderBy(maps.BeforeMapExpression, p => p.OrderBy(p => p.Name!.Last));
                OltAutomapperExtensions.BeforeMap(maps.BeforeMapExpression, orderBy);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }


        [Fact]
        public void AfterMapExtensionTests()
        {
            var maps = new AdapterMapConfigMaps();

            IMappingExpression<AdapterObject5, AdapterObject1>? expression = null;
            IOltAfterMap<AdapterObject5, AdapterObject1>? afterMap = null;
            //Func<IQueryable<AdapterObject1>, IOrderedQueryable<AdapterObject1>> func = null;

            Assert.Throws<ArgumentNullException>(nameof(afterMap), () => OltAutomapperExtensions.AfterMap(maps.AfterMapExpression, afterMap!));
            //Assert.Throws<ArgumentNullException>(nameof(func), () => OltAutomapperExtensions.WithOrderBy(maps.AfterMapExpression, func));

            var orderBy = new OltAfterMapOrderBy<AdapterObject5, AdapterObject1>(p => p.OrderBy(t => t.FirstName));

            //Assert.Throws<ArgumentNullException>(nameof(expression), () => OltAutomapperExtensions.WithOrderBy(expression, p => p.OrderBy(p => p.LastName)));
            Assert.Throws<ArgumentNullException>(nameof(expression), () => OltAutomapperExtensions.AfterMap(expression!, orderBy));

            try
            {
                //OltAutomapperExtensions.WithOrderBy(maps.AfterMapExpression, p => p.OrderBy(p => p.LastName));
                OltAutomapperExtensions.AfterMap(maps.AfterMapExpression, orderBy);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

    }
}
