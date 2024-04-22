using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{

    public class AdapterObject2 : IAdapterObject
    {
        public int ObjectId { get; set; }
        public OltPersonName? Name { get; set; }

        public static AdapterObject2 FakerData()
        {
            var result = new AdapterObject2
            {
                ObjectId = Faker.RandomNumber.Next(3000, 4000),
                Name = new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last(),
                }
            };


            return result;
        }

        public static List<AdapterObject2> FakerList(int number)
        {
            var list = new List<AdapterObject2>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
