using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        /// <param name="options">Action to configure EpplusWriter options.</param>
        /// <param name="configure">Action that configures formatters that will be used by <see cref="IEpplusWriter"/>.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IEpplusWriter"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddEpplusWriter(
            this IServiceCollection services,
            Action<EpplusWriterOptions> options = null,
            Action<TypesCollection<IEpplusFormatter>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddEpplusWriter<EpplusWriter>(options, configure, lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IEpplusWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="options">Action to configure EpplusWriter options.</param>
        /// <param name="configure">Action that configures formatters that will be used by <see cref="IEpplusWriter"/>.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IEpplusWriter"/>.</param>
        /// <typeparam name="TEpplusWriter">Type of custom implementation of <see cref="IEpplusWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddEpplusWriter<TEpplusWriter>(
            this IServiceCollection services,
            Action<EpplusWriterOptions> options = null,
            Action<TypesCollection<IEpplusFormatter>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TEpplusWriter : class, IEpplusWriter
        {
            TypesCollection<IEpplusFormatter> formatters = new TypesCollection<IEpplusFormatter>();
            configure?.Invoke(formatters);

            if (options != null)
            {
                services.Configure(options);
            }

            services.Add(new ServiceDescriptor(
                typeof(IEpplusWriter),
                sp => new EpplusWriter(
                    sp.GetService<IOptions<EpplusWriterOptions>>(),
                    formatters.Types
                        .Select(t =>
                            (IEpplusFormatter)ActivatorUtilities.GetServiceOrCreateInstance(sp, t))),
                lifetime));

            return services;
        }
    }
}
