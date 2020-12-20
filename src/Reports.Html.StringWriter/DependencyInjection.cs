using Microsoft.Extensions.DependencyInjection;

namespace Reports.Html.StringWriter
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseStringWriter(this IServiceCollection services)
        {
            return services.UseStringWriter<StringWriter>();
        }

        public static IServiceCollection UseStringWriter<TStringWriter>(this IServiceCollection services)
            where TStringWriter : StringWriter
        {
            services.UseStringWriter<IStringWriter, TStringWriter>();

            return services;
        }

        public static IServiceCollection UseStringWriter<TIStringWriter, TStringWriter>(this IServiceCollection services)
            where TIStringWriter : class, IStringWriter
            where TStringWriter : class, TIStringWriter
        {
            services.AddScoped<TIStringWriter, TStringWriter>();

            return services;
        }

        public static IServiceCollection UseStringCellWriter(this IServiceCollection services)
        {
            return services.UseStringCellWriter<StringCellWriter>();
        }

        public static IServiceCollection UseStringCellWriter<TStringCellWriter>(this IServiceCollection services)
            where TStringCellWriter : StringCellWriter
        {
            services.UseStringCellWriter<IStringCellWriter, TStringCellWriter>();

            return services;
        }

        public static IServiceCollection UseStringCellWriter<TIStringCellWriter, TStringCellWriter>(this IServiceCollection services)
            where TIStringCellWriter : class, IStringCellWriter
            where TStringCellWriter : class, TIStringCellWriter
        {
            services.AddScoped<TIStringCellWriter, TStringCellWriter>();

            return services;
        }
    }
}
