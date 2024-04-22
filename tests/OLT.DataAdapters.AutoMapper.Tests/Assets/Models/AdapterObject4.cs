namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{
    public class AdapterObject4 : AdapterObject2
    {
        public string Value => $"{ObjectId}={Name?.FullName}";
    }
}
