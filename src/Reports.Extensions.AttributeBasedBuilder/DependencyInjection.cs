using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Reports.Core.Extensions;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseAttributeBasedBuilder(this IServiceCollection services)
        {
            Type[] types = typeof(IAttributeHandler).GetImplementingTypes();
            foreach (Type t in types)
            {
                services.AddScoped(t);
            }

            services.AddScoped<AttributeBasedBuilder>(
                sp => new AttributeBasedBuilder(types.Select(t => (IAttributeHandler) sp.GetRequiredService(t)))
            );

            return services;
        }
    }
}
