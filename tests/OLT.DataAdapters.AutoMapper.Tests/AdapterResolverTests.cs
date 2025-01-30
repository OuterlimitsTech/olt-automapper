using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {
        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Null(adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(true));

                Assert.Null(adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(true));


            }
        }

        [Fact]
        public void CanMapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMap<AdapterObject1, AdapterObject2>());
                Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject1>());
                Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject3>());
                Assert.True(adapterResolver.CanMap<AdapterObject3, AdapterObject2>());

                Assert.False(adapterResolver.CanMap<AdapterObject1, AdapterObject3>());
                Assert.False(adapterResolver.CanMap<AdapterObject3, AdapterObject1>());

            }
        }

        [Fact]
        public void CanProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject3>());

                Assert.True(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(23);
                var expected = obj1Values.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                var result1 = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable());
                Assert.Equal(expected.Select(s => s.FirstName), result1.Select(s => s.Name!.First));
                Assert.Equal(expected.Select(s => s.LastName), result1.Select(s => s.Name!.Last));

                var result2 = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable(), configAction => { configAction.DisableBeforeMap = true; configAction.DisableAfterMap = true; }).OrderBy(p => p.Name!.First).ThenBy(p => p.Name!.Last).ToList();
                Assert.Equal(expected.Select(s => s.FirstName), result2.Select(s => s.Name!.First));
                Assert.Equal(expected.Select(s => s.LastName), result2.Select(s => s.Name!.Last));

                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject1, AdapterObject4>(AdapterObject1.FakerList(3).AsQueryable()));
            }
        }


        [Fact]
        public void ApplyDefaultOrderByTest()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(56);
                var expected = obj1Values.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                var result1 = adapterResolver.ApplyDefaultOrderBy<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable()).ToList();
                Assert.Equal(expected.Select(s => s.FirstName), result1.Select(s => s.FirstName));
                Assert.Equal(expected.Select(s => s.LastName), result1.Select(s => s.LastName));

            }
        }


        [Fact]
        public void InvalidMapExceptionTests()
        {

            //Not a AutoMapperMappingException
            using (var provider = BuildProvider(new List<Profile> { new InvalidMaps() }))
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Throws<OltAutoMapperException<AdapterObject8, AdapterObject1>>(() => adapterResolver.ProjectTo<AdapterObject8, AdapterObject1>(AdapterObject8.FakerList(28).AsQueryable()));
            }

        }

        [Fact]
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1 = AdapterObject1.FakerData();

                var obj2Result = adapterResolver.Map<AdapterObject1, AdapterObject2>(obj1, new AdapterObject2());
                Assert.Equal(obj1.FirstName, obj2Result!.Name!.First);
                Assert.Equal(obj1.LastName, obj2Result!.Name!.Last);
                var result1 = adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result, new AdapterObject1());

                Assert.Equal(obj1.FirstName, result1!.FirstName);
                Assert.Equal(obj1.LastName, result1!.LastName);

                var obj3 = AdapterObject3.FakerData();
                obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3, new AdapterObject2());
                Assert.Equal(obj3.First, obj2Result!.Name!.First);
                Assert.Equal(obj3.Last, obj2Result!.Name!.Last);
                var result2 = adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result, new AdapterObject3());

                Assert.Equal(obj3.First, result2!.First);
                Assert.Equal(obj3.Last, result2.Last);

                Assert.Throws<OltAdapterNotFoundException<NeverAdapterObject, AdapterObject1>>(() => adapterResolver.Map<NeverAdapterObject, AdapterObject1>(new NeverAdapterObject(), new AdapterObject1()));
            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(3);                
                var result1 = adapterResolver.Map<AdapterObject1, AdapterObject2>(obj1Values);
                Assert.Equal(obj1Values.Count, result1.Count);
                Assert.Equal(obj1Values.Select(s => s.FirstName), result1.Select(s => s.Name!.First));
                Assert.Equal(obj1Values.Select(s => s.LastName), result1.Select(s => s.Name!.Last));

                //Don't care!!!!  FluentAssertions removal 
                //var obj3Values = AdapterObject3.FakerList(3);
                //var expected2 = obj3Values.OrderBy(p => p.ObjectId).ToList();
                //var result2 = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3Values).OrderBy(p => p.ObjectId).ToList();
                //Assert.Equal(obj3Values.Count, result2.Count);
                //Assert.Equal(obj3Values.Select(s => s.First), result2.Select(s => s.Name!.First));
                //Assert.Equal(obj3Values.Select(s => s.Last), result2.Select(s => s.Name!.Last));


                Assert.Throws<OltAdapterNotFoundException<NeverAdapterObject, AdapterObject1>>(() => adapterResolver.Map<NeverAdapterObject, AdapterObject1>(new List<NeverAdapterObject>()));

            }
        }

    }
}
