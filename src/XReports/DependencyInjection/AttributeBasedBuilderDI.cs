using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.SchemaBuilders;

namespace XReports.DependencyInjection
{
    public static class AttributeBasedBuilderDI
    {
        public static IServiceCollection AddAttributeBasedBuilder(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddAttributeBasedBuilder(null, lifetime);
        }

        public static IServiceCollection AddAttributeBasedBuilder(
            this IServiceCollection services,
            Action<TypesCollection<IAttributeHandler>> configure,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(
                typeof(IAttributeBasedBuilder),
                sp =>
                {
                    TypesCollection<IAttributeHandler> options = new TypesCollection<IAttributeHandler>();
                    configure?.Invoke(options);

                    return new AttributeBasedBuilder(
                        sp,
                        options.Types
                            .Select(t =>
                                (IAttributeHandler)ActivatorUtilities.GetServiceOrCreateInstance(sp, t)));
                },
                lifetime));

            return services;
        }
    }
}
