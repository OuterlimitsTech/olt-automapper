using AutoMapper;
using OLT.Core;

namespace OLT.Extensions.DependencyInjection.AutoMapper.Tests.Assets
{
    public class AutoMapperMaps : Profile
    {
        public AutoMapperMaps()
        {
            BuildMap(CreateMap<AdapterObject1, AdapterObject2>());
            BuildMap(CreateMap<AdapterObject1, OltPersonName>());
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
    }
}
