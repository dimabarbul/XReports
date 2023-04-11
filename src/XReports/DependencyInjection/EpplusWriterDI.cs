using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.Excel.Writers;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering <see cref="IEpplusWriter"/> in dependency injection service collection.
    /// </summary>
    public static class EpplusWriterDI
    {
        /// <summary>
        /// Registers <see cref="IEpplusWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="configure">Action that configures formatters that will be used by <see cref="IEpplusWriter"/>.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IEpplusWriter"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddEpplusWriter(
            this IServiceCollection services,
            Action<TypesCollection<IEpplusFormatter>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddEpplusWriter<IEpplusWriter, EpplusWriter>(configure, lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IEpplusWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="configure">Action that configures formatters that will be used by <see cref="IEpplusWriter"/>.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IEpplusWriter"/>.</param>
        /// <typeparam name="TEpplusWriter">Type of custom implementation of <see cref="IEpplusWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddEpplusWriter<TEpplusWriter>(
            this IServiceCollection services,
            Action<TypesCollection<IEpplusFormatter>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TEpplusWriter : class, IEpplusWriter
        {
            return services.AddEpplusWriter<IEpplusWriter, TEpplusWriter>(configure, lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IEpplusWriter"/> with custom service type in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="configure">Action that configures formatters that will be used by <see cref="IEpplusWriter"/>.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IEpplusWriter"/>.</param>
        /// <typeparam name="TIEpplusWriter">Type of custom service type under which custom implementation of <see cref="IEpplusWriter"/> should be registered.</typeparam>
        /// <typeparam name="TEpplusWriter">Type of custom implementation of <see cref="IEpplusWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddEpplusWriter<TIEpplusWriter, TEpplusWriter>(
            this IServiceCollection services,
            Action<TypesCollection<IEpplusFormatter>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TIEpplusWriter : class, IEpplusWriter
            where TEpplusWriter : class, TIEpplusWriter
        {
            if (configure != null)
            {
                TypesCollection<IEpplusFormatter> options = new TypesCollection<IEpplusFormatter>();
                configure(options);
                foreach (Type type in options.Types)
                {
                    services.TryAddEnumerable(new ServiceDescriptor(
                        typeof(IEpplusFormatter),
                        type,
                        lifetime));
                }
            }

            services.Add(new ServiceDescriptor(
                typeof(TIEpplusWriter),
                typeof(TEpplusWriter),
                lifetime));

            return services;
        }
    }
}
