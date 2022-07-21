using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class StreamWriterDI
    {
        public static IServiceCollection AddStreamWriter(this IServiceCollection services)
        {
            return services.AddStreamWriter<IStreamWriter, StreamWriter>();
        }

        public static IServiceCollection AddStreamWriter<TStreamWriter>(this IServiceCollection services)
            where TStreamWriter : StreamWriter
        {
            return services.AddStreamWriter<IStreamWriter, TStreamWriter>();
        }

        public static IServiceCollection AddStreamWriter<TIStreamWriter, TStreamWriter>(this IServiceCollection services)
            where TIStreamWriter : class, IStreamWriter
            where TStreamWriter : class, TIStreamWriter
        {
            services.AddScoped<TIStreamWriter, TStreamWriter>();

            if (typeof(TIStreamWriter) != typeof(IStreamWriter))
            {
                services.AddScoped<IStreamWriter>(sp => sp.GetRequiredService<TIStreamWriter>());
            }

            return services;
        }

        public static IServiceCollection AddStreamCellWriter(this IServiceCollection services)
        {
            return services.AddStreamCellWriter<IStreamCellWriter, StreamCellWriter>();
        }

        public static IServiceCollection AddStreamCellWriter<TStreamCellWriter>(this IServiceCollection services)
            where TStreamCellWriter : StreamCellWriter
        {
            return services.AddStreamCellWriter<IStreamCellWriter, TStreamCellWriter>();
        }

        public static IServiceCollection AddStreamCellWriter<TIStreamCellWriter, TStreamCellWriter>(this IServiceCollection services)
            where TIStreamCellWriter : class, IStreamCellWriter
            where TStreamCellWriter : class, TIStreamCellWriter
        {
            services.AddScoped<TIStreamCellWriter, TStreamCellWriter>();

            if (typeof(TIStreamCellWriter) != typeof(IStreamCellWriter))
            {
                services.AddScoped<IStreamCellWriter>(sp => sp.GetRequiredService<TIStreamCellWriter>());
            }

            return services;
        }
    }
}
