using System;
using System.Linq;

namespace Reports.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static Type[] GetImplementingTypes(this Type type)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(type.IsAssignableFrom)
                .ToArray();
        }
    }
}
