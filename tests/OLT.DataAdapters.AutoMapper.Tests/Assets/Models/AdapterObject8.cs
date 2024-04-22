using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{
    public class AdapterObject8 : IAdapterObject
    {
        public int ObjectId { get; set; }
        public string? Street { get; set; }
        public List<OltPersonName> Names { get; set; } = new List<OltPersonName>();

        public static AdapterObject8 FakerData(int numberNames)
        {
            var result = new AdapterObject8
            {
                ObjectId = Faker.RandomNumber.Next(1000, 2000),
                Street = Faker.Address.StreetAddress()                
            };

            result.Names = new List<OltPersonName>();
            for (int i = 0; i < numberNames; i++)
            {
                result.Names.Add(new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last(),
                });
            }
            return result;
        }

        public static List<AdapterObject8> FakerList(int number)
        {
            var list = new List<AdapterObject8>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData(Faker.RandomNumber.Next(2, 8));
                list.Add(item);
            }
            return list;
        }
    }

}
