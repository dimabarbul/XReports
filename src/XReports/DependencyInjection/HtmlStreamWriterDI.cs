using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class HtmlStreamWriterDI
    {
        public static IServiceCollection AddHtmlStreamWriter(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddHtmlStreamWriter<HtmlStreamWriter>(lifetime);
        }

        public static IServiceCollection AddHtmlStreamWriter<THtmlStreamWriter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THtmlStreamWriter : IHtmlStreamWriter
        {
            return services.AddHtmlStreamWriter<THtmlStreamWriter, HtmlStreamCellWriter>(lifetime);
        }

        public static IServiceCollection AddHtmlStreamWriter<THtmlStreamWriter, THtmlStreamCellWriter>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THtmlStreamWriter : IHtmlStreamWriter
            where THtmlStreamCellWriter : IHtmlStreamCellWriter
        {
            services.Add(new ServiceDescriptor(typeof(IHtmlStreamWriter), typeof(THtmlStreamWriter), lifetime));
            services.Add(new ServiceDescriptor(typeof(IHtmlStreamCellWriter), typeof(THtmlStreamCellWriter), lifetime));

            return services;
        }
    }
}
