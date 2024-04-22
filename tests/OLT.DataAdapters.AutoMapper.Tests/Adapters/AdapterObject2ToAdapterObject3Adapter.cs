using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
    public class AdapterObject2ToAdapterObject3Adapter : OltAdapter<AdapterObject2, AdapterObject3>
    {
        public override void Map(AdapterObject2 source, AdapterObject3 destination)
        {
            destination.ObjectId = source.ObjectId;
            destination.First = source.Name?.First;
            destination.Last = source.Name?.Last;
        }

        public override void Map(AdapterObject3 source, AdapterObject2 destination)
        {
            destination.Name = new OltPersonName();
            destination.ObjectId = source.ObjectId;
            destination.Name.First = source.First;
            destination.Name.Last = source.Last;
        }
    }
}
