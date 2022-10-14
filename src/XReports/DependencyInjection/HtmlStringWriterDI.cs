using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class HtmlStringWriterDI
    {
        public static IServiceCollection AddHtmlStringWriter(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.AddHtmlStringWriter<HtmlStringWriter>(lifetime);
        }

        public static IServiceCollection AddHtmlStringWriter<THtmlStringWriter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THtmlStringWriter : IHtmlStringWriter
        {
            return services.AddHtmlStringWriter<THtmlStringWriter, HtmlStringCellWriter>(lifetime);
        }

        public static IServiceCollection AddHtmlStringWriter<THtmlStringWriter, THtmlStringCellWriter>(
            this IServiceCollection services, ServiceLifetime lifetime)
            where THtmlStringWriter : IHtmlStringWriter
            where THtmlStringCellWriter : IHtmlStringCellWriter
        {
            services.Add(new ServiceDescriptor(typeof(IHtmlStringWriter), typeof(THtmlStringWriter), lifetime));
            services.Add(new ServiceDescriptor(typeof(IHtmlStringCellWriter), typeof(THtmlStringCellWriter), lifetime));

            return services;
        }
    }
}
