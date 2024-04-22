using OLT.Core;

namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{
    public class AdapterObject9 : IAdapterObject
    {
        public int ObjectId { get; set; }
        public string? StreetAddress { get; set; }
        public OltPersonName? Name { get; set; }
    }
}
