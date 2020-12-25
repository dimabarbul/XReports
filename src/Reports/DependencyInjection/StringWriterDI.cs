using Microsoft.Extensions.DependencyInjection;
using Reports.Interfaces;
using Reports.Writers;

namespace Reports.DependencyInjection
{
    public static class StringWriterDI
    {
        public static IServiceCollection UseStringWriter(this IServiceCollection services)
        {
            return services.UseStringWriter<StringWriter>();
        }

        public static IServiceCollection UseStringWriter<TStringWriter>(this IServiceCollection services)
            where TStringWriter : StringWriter
        {
            return services.UseStringWriter<IStringWriter, TStringWriter>();
        }

        public static IServiceCollection UseStringWriter<TIStringWriter, TStringWriter>(this IServiceCollection services)
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

        public static IServiceCollection UseStringCellWriter(this IServiceCollection services)
        {
            return services.UseStringCellWriter<StringCellWriter>();
        }

        public static IServiceCollection UseStringCellWriter<TStringCellWriter>(this IServiceCollection services)
            where TStringCellWriter : StringCellWriter
        {
            return services.UseStringCellWriter<IStringCellWriter, TStringCellWriter>();
        }

        public static IServiceCollection UseStringCellWriter<TIStringCellWriter, TStringCellWriter>(this IServiceCollection services)
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
