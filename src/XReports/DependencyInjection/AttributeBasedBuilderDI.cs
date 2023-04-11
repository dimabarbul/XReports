using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.SchemaBuilders;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering <see cref="IAttributeBasedBuilder"/> in dependency injection service collection.
    /// </summary>
    public static class AttributeBasedBuilderDI
    {
        /// <summary>
        /// Registers <see cref="IAttributeBasedBuilder"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of the <see cref="IAttributeBasedBuilder"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAttributeBasedBuilder(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddAttributeBasedBuilder(null, lifetime);
        }

        /// <summary>
        /// Registers <see cref="IAttributeBasedBuilder"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="configure">Action that configures attribute handlers that will be used by the <see cref="IAttributeBasedBuilder"/>.</param>
        /// <param name="lifetime">Service lifetime of the <see cref="IAttributeBasedBuilder"/>.</param>
        /// <returns>Service collection.</returns>
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
