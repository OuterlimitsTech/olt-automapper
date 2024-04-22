using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
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

                var obj2Queryable = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable());

                var expected = obj1Values
                    .Select(s => new AdapterObject2
                    {
                        ObjectId = s.ObjectId,
                        Name = new OltPersonName
                        {
                            First = s.FirstName,
                            Last = s.LastName,
                        }
                    }).ToList();

                obj2Queryable
                    .Should()
                    .BeEquivalentTo(expected.OrderBy(p => p.Name!.First).ThenBy(p => p.Name!.Last), opt => opt.WithStrictOrdering());


                adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable(), configAction => { configAction.DisableBeforeMap = true; configAction.DisableAfterMap = true; })
                    .Should()
                    .BeEquivalentTo(expected, opt => opt.WithStrictOrdering());


                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject1, AdapterObject4>(AdapterObject1.FakerList(3).AsQueryable()));
            }
        }

        [Fact]
        public void ProjectToPagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var pagingParams = new OltPagingParams { Page = 1, Size = 25 };

                var obj1Values = AdapterObject1.FakerList(56);
                var expected = obj1Values.Select(s => new AdapterObject2
                {
                    ObjectId = s.ObjectId,
                    Name = new OltPersonName
                    {
                        First = s.FirstName,
                        Last = s.LastName,
                    }
                })
                    .OrderBy(p => p.Name!.First)
                    .ThenBy(p => p.Name!.Last)
                    .ToList();


                var obj2Result = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable()).ToList();
                obj2Result.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());

                var paged = obj2Result.AsQueryable().ToPaged(pagingParams);
                var expectedPaged = expected.AsQueryable().ToPaged(pagingParams);
                paged.Should().BeEquivalentTo(expectedPaged, opt => opt.WithStrictOrdering());
            }
        }

        [Fact]
        public void ApplyDefaultOrderByTest()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(56);
                var obj2Result = adapterResolver.ApplyDefaultOrderBy<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable()).ToList();
                obj2Result.Should().BeEquivalentTo(obj1Values.OrderBy(p => p.FirstName).ThenBy(p => p.LastName), opt => opt.WithStrictOrdering());
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
                Assert.Equal(obj1.FirstName, obj2Result.Name!.First);
                Assert.Equal(obj1.LastName, obj2Result.Name!.Last);
                adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result, new AdapterObject1()).Should().BeEquivalentTo(obj1);


                var obj3 = AdapterObject3.FakerData();
                obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3, new AdapterObject2());
                Assert.Equal(obj3.First, obj2Result.Name!.First);
                Assert.Equal(obj3.Last, obj2Result.Name!.Last);
                adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result, new AdapterObject3()).Should().BeEquivalentTo(obj3);

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
                var obj2Result = adapterResolver.Map<AdapterObject1, AdapterObject2>(obj1Values);
                obj2Result.Should().HaveCount(obj1Values.Count);
                obj2Result.Select(s => s.Name!.First).Should().BeEquivalentTo(obj1Values[0].FirstName, obj1Values[1].FirstName, obj1Values[2].FirstName);
                obj2Result.Select(s => s.Name!.Last).Should().BeEquivalentTo(obj1Values[0].LastName, obj1Values[1].LastName, obj1Values[2].LastName);
                adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result).Should().BeEquivalentTo(obj1Values);


                var obj3Values = AdapterObject3.FakerList(3);
                obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3Values);
                obj2Result.Should().HaveCount(obj3Values.Count);
                obj2Result.Select(s => s.Name!.First).Should().BeEquivalentTo(obj3Values[0].First, obj3Values[1].First, obj3Values[2].First);
                obj2Result.Select(s => s.Name!.Last).Should().BeEquivalentTo(obj3Values[0].Last, obj3Values[1].Last, obj3Values[2].Last);

                adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result).Should().BeEquivalentTo(obj3Values);
                

                Assert.Throws<OltAdapterNotFoundException<NeverAdapterObject, AdapterObject1>>(() => adapterResolver.Map<NeverAdapterObject, AdapterObject1>(new List<NeverAdapterObject>()));

            }
        }

    }
}
