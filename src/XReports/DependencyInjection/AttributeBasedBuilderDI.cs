using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using XReports.Extensions;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.SchemaBuilders;

namespace XReports.DependencyInjection
{
    public static class AttributeBasedBuilderDI
    {
        public static IServiceCollection UseAttributeBasedBuilder(this IServiceCollection services)
        {
            Type[] types = typeof(IAttributeHandler).GetImplementingTypes();
            foreach (Type t in types)
            {
                services.AddScoped(t);
            }

            services.AddScoped<AttributeBasedBuilder>(
                sp => new AttributeBasedBuilder(sp, types.Select(t => (IAttributeHandler) sp.GetRequiredService(t)))
            );

            RegisterPostBuilders(services);

            return services;
        }

        private static void RegisterPostBuilders(IServiceCollection services)
        {
            foreach (Type postBuilderType in AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Select(t => t.GetCustomAttribute<ReportAttribute>()?.PostBuilder)
                .Where(postBuilder => postBuilder != null))
            {
                services.AddScoped(postBuilderType);
            }
        }
    }
}
