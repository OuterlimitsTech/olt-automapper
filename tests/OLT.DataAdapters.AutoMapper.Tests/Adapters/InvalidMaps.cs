using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{

    public class InvalidAfterMap : OltAdapterAfterMap<AdapterObject8, AdapterObject1>
    {
        public override IQueryable<AdapterObject1> AfterMap(IQueryable<AdapterObject1> queryable)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Used to OltAdapterResolverAutoMapper.BuildException not being a AutoMapperMappingException
    /// </summary>
    public class InvalidMaps : Profile
    {
        
        public InvalidMaps()
        {
            BuildMap(CreateMap<AdapterObject8, AdapterObject1>()).AfterMap(new InvalidAfterMap());  //Force Exception in AfterMap
        }

        protected IMappingExpression<AdapterObject8, AdapterObject1> BuildMap(IMappingExpression<AdapterObject8, AdapterObject1> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ReverseMap()
                ;

            return mappingExpression;
        }
    }
}
