using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System.Linq;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class AdapterObject2ToAdapterObject5PagedAdapter : OltAdapterPaged<AdapterObject2, AdapterObject5>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public override void Map(AdapterObject2 source, AdapterObject5 destination)
        {
            throw new System.NotImplementedException();
        }

        public override void Map(AdapterObject5 source, AdapterObject2 destination)
        {
            throw new System.NotImplementedException();
        }

        public override IQueryable<AdapterObject5> Map(IQueryable<AdapterObject2> queryable)
        {
            return queryable.Select(entity => new AdapterObject5
            {
                ObjectId = entity.ObjectId,
                First = entity.Name!.First, 
                Last = entity.Name!.Last,
            });
        }

        public override IOrderedQueryable<AdapterObject2> DefaultOrderBy(IQueryable<AdapterObject2> queryable)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return queryable.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First).ThenBy(p => p.ObjectId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

  
    }
}
