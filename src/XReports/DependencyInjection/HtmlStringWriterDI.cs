using Microsoft.Extensions.DependencyInjection;
using XReports.Html.Writers;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering <see cref="IHtmlStringWriter"/> in dependency injection service collection.
    /// </summary>
    public static class HtmlStringWriterDI
    {
        /// <summary>
        /// Registers <see cref="IHtmlStringWriter"/> and <see cref="IHtmlStringCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStringWriter"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStringWriter(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            return services.AddHtmlStringWriter<HtmlStringWriter>(lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IHtmlStringWriter"/> and default implementation of <see cref="IHtmlStringCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStringWriter"/>.</param>
        /// <typeparam name="THtmlStringWriter">Type of custom implementation of <see cref="IHtmlStringWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStringWriter<THtmlStringWriter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THtmlStringWriter : IHtmlStringWriter
        {
            return services.AddHtmlStringWriter<THtmlStringWriter, HtmlStringCellWriter>(lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IHtmlStringWriter"/> and custom implementation of <see cref="IHtmlStringCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStringWriter"/>.</param>
        /// <typeparam name="THtmlStringWriter">Type of custom implementation of <see cref="IHtmlStringWriter"/>.</typeparam>
        /// <typeparam name="THtmlStringCellWriter">Type of custom implementation of <see cref="IHtmlStringCellWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStringWriter<THtmlStringWriter, THtmlStringCellWriter>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THtmlStringWriter : IHtmlStringWriter
            where THtmlStringCellWriter : IHtmlStringCellWriter
        {
            services.Add(new ServiceDescriptor(typeof(IHtmlStringWriter), typeof(THtmlStringWriter), lifetime));
            services.Add(new ServiceDescriptor(typeof(IHtmlStringCellWriter), typeof(THtmlStringCellWriter), lifetime));

            return services;
        }
    }
}
