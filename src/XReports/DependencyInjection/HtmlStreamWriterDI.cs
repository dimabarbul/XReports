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

        public static IServiceCollection AddStreamCellWriter(this IServiceCollection services)
        {
            return services.AddStreamCellWriter<IHtmlStreamCellWriter, HtmlStreamCellWriter>();
        }

        public static IServiceCollection AddStreamCellWriter<TStreamCellWriter>(this IServiceCollection services)
            where TStreamCellWriter : HtmlStreamCellWriter
        {
            return services.AddStreamCellWriter<IHtmlStreamCellWriter, TStreamCellWriter>();
        }

        public static IServiceCollection AddStreamCellWriter<TIStreamCellWriter, TStreamCellWriter>(this IServiceCollection services)
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
