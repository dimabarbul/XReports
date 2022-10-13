using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XReports.Interfaces;

namespace XReports.DependencyInjection
{
    public class EpplusWriterOptions
    {
        private readonly List<Assembly> assemblies = new List<Assembly>();

        public IReadOnlyCollection<Type> Types => this.GetTypes();

        public void AddFromAssembly(params Assembly[] assemblies)
        {
            this.assemblies.AddRange(assemblies);
        }

        private Type[] GetTypes()
        {
            return this.assemblies.SelectMany(this.GetTypesFromAssembly)
                .ToArray();
        }

        private IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(this.IsFormatterTypeValid);
        }

        private bool IsFormatterTypeValid(Type t)
        {
            return t.IsClass && !t.IsAbstract && typeof(IEpplusFormatter).IsAssignableFrom(t);
        }
    }
}
