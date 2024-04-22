using System.Collections.Generic;

namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{

    public class AdapterObject1 : IAdapterObject
    {
        public int ObjectId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public static AdapterObject1 FakerData()
        {
            var result = new AdapterObject1
            {
                ObjectId = Faker.RandomNumber.Next(1000, 2000),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            return result;
        }

        public static List<AdapterObject1> FakerList(int number)
        {
            var list = new List<AdapterObject1>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
