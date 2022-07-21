using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class StringWriterDI
    {
        public static IServiceCollection AddStringWriter(this IServiceCollection services)
        {
            return services.AddStringWriter<IStringWriter, StringWriter>();
        }

        public static IServiceCollection AddStringWriter<TStringWriter>(this IServiceCollection services)
            where TStringWriter : StringWriter
        {
            return services.AddStringWriter<IStringWriter, TStringWriter>();
        }

        public static IServiceCollection AddStringWriter<TIStringWriter, TStringWriter>(this IServiceCollection services)
            where TIStringWriter : class, IStringWriter
            where TStringWriter : class, TIStringWriter
        {
            services.AddScoped<TIStringWriter, TStringWriter>();

            if (typeof(TIStringWriter) != typeof(IStringWriter))
            {
                services.AddScoped<IStringWriter>(sp => sp.GetRequiredService<TIStringWriter>());
            }

            return services;
        }

        public static IServiceCollection AddStringCellWriter(this IServiceCollection services)
        {
            return services.AddStringCellWriter<StringCellWriter>();
        }

        public static IServiceCollection AddStringCellWriter<TStringCellWriter>(this IServiceCollection services)
            where TStringCellWriter : StringCellWriter
        {
            return services.AddStringCellWriter<IStringCellWriter, TStringCellWriter>();
        }

        public static IServiceCollection AddStringCellWriter<TIStringCellWriter, TStringCellWriter>(this IServiceCollection services)
            where TIStringCellWriter : class, IStringCellWriter
            where TStringCellWriter : class, TIStringCellWriter
        {
            services.AddScoped<TIStringCellWriter, TStringCellWriter>();

            if (typeof(TIStringCellWriter) != typeof(IStringCellWriter))
            {
                services.AddScoped<IStringCellWriter>(sp => sp.GetRequiredService<TIStringCellWriter>());
            }

            return services;
        }
    }
}
