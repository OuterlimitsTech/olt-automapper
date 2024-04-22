﻿namespace OLT.Core
{
    /// <summary>
    /// Assembly Scan Filter
    /// </summary>
    public class OltAutoMapperAssemblyFilter : OltAssemblyFilter
    {
        /// <summary>
        /// OLT.*, MyApp.*, and includes exclustions <seealso cref="OltAssemblyFilter.WithDefaultDIExclusionFilters"/>
        /// </summary>
        /// <param name="filters"></param>        
        public OltAutoMapperAssemblyFilter(params string[] filters) : base(filters)
        {
            base.WithDefaultDIExclusionFilters();
        }
    }
}
