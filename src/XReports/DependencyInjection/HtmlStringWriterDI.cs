using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class HtmlStringWriterDI
    {
        public static IServiceCollection AddHtmlStringWriter(this IServiceCollection services)
        {
            return services.AddHtmlStringWriter<IHtmlStringWriter, HtmlStringWriter>();
        }

        public static IServiceCollection AddHtmlStringWriter<THtmlStringWriter>(this IServiceCollection services)
            where THtmlStringWriter : HtmlStringWriter
        {
            return services.AddHtmlStringWriter<IHtmlStringWriter, THtmlStringWriter>();
        }

        public static IServiceCollection AddHtmlStringWriter<TIHtmlStringWriter, THtmlStringWriter>(this IServiceCollection services)
            where TIHtmlStringWriter : class, IHtmlStringWriter
            where THtmlStringWriter : class, TIHtmlStringWriter
        {
            services.AddScoped<TIHtmlStringWriter, THtmlStringWriter>();

            if (typeof(TIHtmlStringWriter) != typeof(IHtmlStringWriter))
            {
                services.AddScoped<IHtmlStringWriter>(sp => sp.GetRequiredService<TIHtmlStringWriter>());
            }

            return services;
        }

        public static IServiceCollection AddStringCellWriter(this IServiceCollection services)
        {
            return services.AddStringCellWriter<HtmlStringCellWriter>();
        }

        public static IServiceCollection AddStringCellWriter<TStringCellWriter>(this IServiceCollection services)
            where TStringCellWriter : HtmlStringCellWriter
        {
            return services.AddStringCellWriter<IHtmlStringCellWriter, TStringCellWriter>();
        }

        public static IServiceCollection AddStringCellWriter<TIStringCellWriter, TStringCellWriter>(this IServiceCollection services)
            where TIStringCellWriter : class, IHtmlStringCellWriter
            where TStringCellWriter : class, TIStringCellWriter
        {
            services.AddScoped<TIStringCellWriter, TStringCellWriter>();

            if (typeof(TIStringCellWriter) != typeof(IHtmlStringCellWriter))
            {
                services.AddScoped<IHtmlStringCellWriter>(sp => sp.GetRequiredService<TIStringCellWriter>());
            }

            return services;
        }
    }
}
