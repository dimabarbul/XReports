using Microsoft.Extensions.DependencyInjection;
using XReports.Html.Writers;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering <see cref="IHtmlStreamWriter"/> in dependency injection service collection.
    /// </summary>
    public static class HtmlStreamWriterDI
    {
        /// <summary>
        /// Registers <see cref="IHtmlStreamWriter"/> and <see cref="IHtmlStreamCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStreamWriter"/>.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStreamWriter(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            return services.AddHtmlStreamWriter<HtmlStreamWriter>(lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IHtmlStreamWriter"/> and default implementation of <see cref="IHtmlStreamCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStreamWriter"/>.</param>
        /// <typeparam name="THtmlStreamWriter">Type of custom implementation of <see cref="IHtmlStreamWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStreamWriter<THtmlStreamWriter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THtmlStreamWriter : IHtmlStreamWriter
        {
            return services.AddHtmlStreamWriter<THtmlStreamWriter, HtmlStreamCellWriter>(lifetime);
        }

        /// <summary>
        /// Registers custom implementation of <see cref="IHtmlStreamWriter"/> and custom implementation of <see cref="IHtmlStreamCellWriter"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of <see cref="IHtmlStreamWriter"/>.</param>
        /// <typeparam name="THtmlStreamWriter">Type of custom implementation of <see cref="IHtmlStreamWriter"/>.</typeparam>
        /// <typeparam name="THtmlStreamCellWriter">Type of custom implementation of <see cref="IHtmlStreamCellWriter"/>.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddHtmlStreamWriter<THtmlStreamWriter, THtmlStreamCellWriter>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THtmlStreamWriter : IHtmlStreamWriter
            where THtmlStreamCellWriter : IHtmlStreamCellWriter
        {
            services.Add(new ServiceDescriptor(typeof(IHtmlStreamWriter), typeof(THtmlStreamWriter), lifetime));
            services.Add(new ServiceDescriptor(typeof(IHtmlStreamCellWriter), typeof(THtmlStreamCellWriter), lifetime));

            return services;
        }
    }
}
