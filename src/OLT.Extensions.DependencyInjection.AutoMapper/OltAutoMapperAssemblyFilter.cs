using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{

    /// <summary>
    /// Assembly Scan Filter
    /// </summary>    
    [Obsolete("Move to OLT.Utility.AssemblyScanner")]
    public class OltAutoMapperAssemblyFilter 
    {

        /// <summary>
        /// OLT.*, MyApp.*, and includes exclustions <seealso cref="WithDefaultDIExclusionFilters"/>
        /// </summary>
        /// <param name="filters"></param>        
        public OltAutoMapperAssemblyFilter(params string[] filters)
        {
            Filters.AddRange(filters);
            WithDefaultDIExclusionFilters();
        }


        /// <summary>
        /// Defaults "Microsoft.*" and "System.*" to prevent Microsoft and System Assemblies from loading into the DI scan
        /// </summary>   
        /// <remarks>
        /// <list type="table">
        /// <item>https://github.com/dotnet/SqlClient/issues/1930</item>
        /// <item>https://github.com/borisdj/EFCore.BulkExtensions/issues/1402</item>
        /// </list>
        /// </remarks>
        public OltAutoMapperAssemblyFilter WithDefaultDIExclusionFilters()
        {
            this.ExcludeFilters = new List<string> { "Microsoft.*", "System.*" };
            return this;
        }

        public List<string> Filters { get; set; } = new List<string>();
        public List<string> ExcludeFilters { get; set; } = new List<string>();

        public virtual IEnumerable<Assembly> FilterAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (Filters.Count > 0 || ExcludeFilters.Count > 0)
            {
                var filtered = Filters.Count > 0 ? assemblies.Where(ShouldIncludeAssembly) : assemblies;
                return ExcludeFilters.Count > 0 ? filtered.Where(p => ShouldExcludeAssembly(p) == false) : filtered;
            }

            return assemblies;
        }



        public virtual bool ShouldIncludeAssembly(Assembly assembly)
        {
            return Filters.Exists(filter => MatchesFilter(assembly, filter));
        }

        protected virtual bool MatchesFilter(Assembly assembly, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return false;
            }

            if (assembly.FullName is null)
            {
                return false;
            }

            if (filter.EndsWith("*"))
            {
                var prefix = filter.TrimEnd('*');
                return assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                var assemblyName = assembly.FullName.Split(',')[0];
                return string.Equals(assemblyName, filter, StringComparison.OrdinalIgnoreCase);
            }
        }

        public virtual bool ShouldExcludeAssembly(Assembly assembly)
        {
            return ExcludeFilters.Exists(filter => MatchesFilter(assembly, filter));
        }

        public virtual void RemoveAllExclusions(List<Assembly> assemblies)
        {
            ExcludeFilters.ForEach(excludeName => RemoveAll(assemblies, excludeName));
        }

        public virtual void RemoveAll(List<Assembly> assemblies, string excludeName)
        {
            assemblies.RemoveAll(p => MatchesFilter(p, excludeName));
        }

    }
}
