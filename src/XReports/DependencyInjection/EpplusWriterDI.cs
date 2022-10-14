using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class EpplusWriterDI
    {
        public static IServiceCollection AddEpplusWriter(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, Action<EpplusWriterOptions> configure = null)
        {
            return services.AddEpplusWriter<IEpplusWriter, EpplusWriter>(lifetime, configure);
        }

        public static IServiceCollection AddEpplusWriter<TEpplusWriter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, Action<EpplusWriterOptions> configure = null)
            where TEpplusWriter : class, IEpplusWriter
        {
            return services.AddEpplusWriter<IEpplusWriter, TEpplusWriter>(lifetime, configure);
        }

        public static IServiceCollection AddEpplusWriter<TIEpplusWriter, TEpplusWriter>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Action<EpplusWriterOptions> configure = null)
            where TIEpplusWriter : class, IEpplusWriter
            where TEpplusWriter : class, TIEpplusWriter
        {
            if (configure != null)
            {
                EpplusWriterOptions options = new EpplusWriterOptions();
                configure(options);
                foreach (Type type in options.Types)
                {
                    services.TryAddEnumerable(new ServiceDescriptor(
                        typeof(IEpplusFormatter),
                        type,
                        lifetime));
                }
            }

            services.Add(new ServiceDescriptor(
                typeof(TIEpplusWriter),
                typeof(TEpplusWriter),
                lifetime));

            return services;
        }
    }
}
