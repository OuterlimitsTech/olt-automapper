using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
    public class AutoMapperMaps : Profile
    {
        public AutoMapperMaps()
        {
            BuildMap(CreateMap<AdapterObject1, AdapterObject2>()).WithOrderBy(p => p.OrderBy(o => o.FirstName).ThenBy(o => o.LastName));            
            BuildMap(CreateMap<AdapterObject1, OltPersonName>());
            BuildMap(CreateMap<AdapterObject3, OltPersonName>());
        }


        protected virtual IMappingExpression<AdapterObject1, AdapterObject2> BuildMap(IMappingExpression<AdapterObject1, AdapterObject2> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t))
                .ReverseMap()
                ;

            return mappingExpression;
        }

        protected virtual IMappingExpression<AdapterObject1, OltPersonName> BuildMap(IMappingExpression<AdapterObject1, OltPersonName> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.FirstName))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.LastName))
                .ReverseMap()
                ;

            return mappingExpression;
        }


        protected virtual IMappingExpression<AdapterObject3, OltPersonName> BuildMap(IMappingExpression<AdapterObject3, OltPersonName> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.First, opt => opt.MapFrom(t => t.First))
                .ForMember(f => f.Last, opt => opt.MapFrom(t => t.Last))
                .ReverseMap()
                ;

            return mappingExpression;
        }

        protected virtual IMappingExpression<AdapterObject2, AdapterObject3> BuildMap(IMappingExpression<AdapterObject2, AdapterObject3> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.ObjectId, opt => opt.MapFrom(t => t.ObjectId))
                .ForMember(f => f, opt => opt.MapFrom(t => t.Name))
                .ReverseMap()
                ;
            return mappingExpression;
        }

    }
}
