using System;

namespace OLT.DataAdapters.AutoMapper.Tests.Assets.Models
{
    public class PersonName
    {
        public string? First { get; set; }
        public string? Middle { get; set; }
        public string? Last { get; set; }
        public string? Suffix { get; set; }

        public virtual string? FullName => System.Text.RegularExpressions.Regex.Replace(($"{First} {Middle} {Last} {Suffix}").Trim(), @"\s+", " ", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }
}
