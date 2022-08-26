using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class HtmlStreamWriterDI
    {
        public static IServiceCollection AddHtmlStreamWriter(this IServiceCollection services)
        {
            return services.AddHtmlStreamWriter<IHtmlStreamWriter, HtmlStreamWriter>();
        }

        public static IServiceCollection AddHtmlStreamWriter<TStreamWriter>(this IServiceCollection services)
            where TStreamWriter : HtmlStreamWriter
        {
            return services.AddHtmlStreamWriter<IHtmlStreamWriter, TStreamWriter>();
        }

        public static IServiceCollection AddHtmlStreamWriter<TIHtmlStreamWriter, TStreamWriter>(this IServiceCollection services)
            where TIHtmlStreamWriter : class, IHtmlStreamWriter
            where TStreamWriter : class, TIHtmlStreamWriter
        {
            services.AddScoped<TIHtmlStreamWriter, TStreamWriter>();

            if (typeof(TIHtmlStreamWriter) != typeof(IHtmlStreamWriter))
            {
                services.AddScoped<IHtmlStreamWriter>(sp => sp.GetRequiredService<TIHtmlStreamWriter>());
            }

            return services;
        }

        public static IServiceCollection AddHtmlStreamCellWriter(this IServiceCollection services)
        {
            return services.AddHtmlStreamCellWriter<IHtmlStreamCellWriter, HtmlStreamCellWriter>();
        }

        public static IServiceCollection AddHtmlStreamCellWriter<TStreamCellWriter>(this IServiceCollection services)
            where TStreamCellWriter : HtmlStreamCellWriter
        {
            return services.AddHtmlStreamCellWriter<IHtmlStreamCellWriter, TStreamCellWriter>();
        }

        public static IServiceCollection AddHtmlStreamCellWriter<TIStreamCellWriter, TStreamCellWriter>(this IServiceCollection services)
            where TIStreamCellWriter : class, IHtmlStreamCellWriter
            where TStreamCellWriter : class, TIStreamCellWriter
        {
            services.AddScoped<TIStreamCellWriter, TStreamCellWriter>();

            if (typeof(TIStreamCellWriter) != typeof(IHtmlStreamCellWriter))
            {
                services.AddScoped<IHtmlStreamCellWriter>(sp => sp.GetRequiredService<TIStreamCellWriter>());
            }

            return services;
        }
    }
}
