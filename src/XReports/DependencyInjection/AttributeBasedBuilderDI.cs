using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="configure">Action that configures attribute handlers that will be used by the <see cref="IAttributeBasedBuilder"/>.</param>
        /// <param name="lifetime">Service lifetime of the <see cref="IAttributeBasedBuilder"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAttributeBasedBuilder(
            this IServiceCollection services,
            Action<TypesCollection<IAttributeHandler>> configure,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            TypesCollection<IAttributeHandler> options = new TypesCollection<IAttributeHandler>();
            configure?.Invoke(options);

            services.Add(new ServiceDescriptor(
                typeof(IAttributeBasedBuilder),
                sp => new AttributeBasedBuilder(
                    sp,
                    options.Types
                        .Select(t =>
                            (IAttributeHandler)ActivatorUtilities.GetServiceOrCreateInstance(sp, t))),
                lifetime));

            return services;
        }
    }
}
