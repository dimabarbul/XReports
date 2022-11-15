using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            if (configure != null)
            {
                TypesCollection<IAttributeHandler> options = new TypesCollection<IAttributeHandler>();
                configure(options);
                foreach (Type type in options.Types)
                {
                    services.TryAddEnumerable(new ServiceDescriptor(
                        typeof(IAttributeHandler),
                        type,
                        lifetime));
                }
            }

            services.Add(new ServiceDescriptor(
                typeof(IAttributeBasedBuilder),
                typeof(AttributeBasedBuilder),
                lifetime));

            return services;
        }
    }
}
