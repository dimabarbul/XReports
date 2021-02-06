using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.SchemaBuilders;

namespace XReports.DependencyInjection
{
    public static class AttributeBasedBuilderDI
    {
        public static IServiceCollection AddAttributeBasedBuilder(this IServiceCollection services)
        {
            services.AddScoped<IAttributeBasedBuilder, AttributeBasedBuilder>(
                sp => new AttributeBasedBuilder(
                    sp,
                    typeof(IAttributeHandler).GetImplementingTypes()
                        .Select(t => (IAttributeHandler)ActivatorUtilities.GetServiceOrCreateInstance(sp, t))));

            return services;
        }
    }
}
