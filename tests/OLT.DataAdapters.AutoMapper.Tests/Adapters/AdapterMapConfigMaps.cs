using AutoMapper;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
    public class AdapterMapConfigMaps : Profile
    {
        public AdapterMapConfigMaps()
        {
            BeforeMapExpression = CreateMap<AdapterObject4, AdapterObject5>();
            AfterMapExpression = CreateMap<AdapterObject5, AdapterObject1>();
        }
        public IMappingExpression<AdapterObject4, AdapterObject5> BeforeMapExpression { get; set; }
        public IMappingExpression<AdapterObject5, AdapterObject1> AfterMapExpression { get; set; }


    }
}
