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
            where TStringWriter : StringWriter, new()
        {
            services.AddScoped<StringWriter, TStringWriter>();

            return services;
        }
    }
}
